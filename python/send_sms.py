import requests
import json
import base64
import os
from concurrent.futures import ThreadPoolExecutor
from typing import List, Dict, Union

class SMSGateway:
    """
    فئة بوابة إرسال الرسائل النصية
    SMS Gateway Class
    ایس ایم ایس گیٹ وے کلاس
    """
    
    def __init__(self, api_key: str, api_secret: str):
        """
        تهيئة الفئة مع بيانات الاعتماد
        Initialize with credentials
        اسناد کے ساتھ شروع کریں
        """
        self.api_key = api_key
        self.api_secret = api_secret
        self.base_url = "https://api-sms.4jawaly.com/api/v1"
        self.headers = {
            "Accept": "application/json",
            "Content-Type": "application/json",
            "Authorization": f"Basic {self._get_auth_hash()}"
        }
    
    def _get_auth_hash(self) -> str:
        """
        إنشاء هاش المصادقة
        Generate authentication hash
        تصدیق ہیش بنائیں
        """
        return base64.b64encode(
            f"{self.api_key}:{self.api_secret}".encode()
        ).decode()
    
    def _chunk_numbers(self, numbers: List[str], chunk_size: int) -> List[List[str]]:
        """
        تقسيم الأرقام إلى مجموعات
        Split numbers into chunks
        نمبروں کو حصوں میں تقسیم کریں
        """
        return [numbers[i:i + chunk_size] for i in range(0, len(numbers), chunk_size)]
    
    def _send_chunk(self, message: str, numbers: List[str], sender: str) -> Dict:
        """
        إرسال مجموعة من الأرقام
        Send a chunk of numbers
        نمبروں کا ایک حصہ بھیجیں
        """
        try:
            payload = {
                "messages": [{
                    "text": message,
                    "numbers": numbers,
                    "sender": sender
                }]
            }
            
            response = requests.post(
                f"{self.base_url}/account/area/sms/send",
                headers=self.headers,
                json=payload
            )
            
            if response.status_code == 200:
                return {
                    "success": True,
                    "chunk": numbers,
                    "response": response.json()
                }
            else:
                return {
                    "success": False,
                    "chunk": numbers,
                    "error": response.json().get("message", f"HTTP Error: {response.status_code}")
                }
        except Exception as e:
            return {
                "success": False,
                "chunk": numbers,
                "error": str(e)
            }
    
    def send_sms(self, message: str, numbers: Union[List[str], str], sender: str) -> Dict:
        """
        إرسال رسائل SMS مع دعم الإرسال المتوازي
        Send SMS messages with parallel sending support
        متوازی بھیجنے کی سہولت کے ساتھ ایس ایم ایس پیغامات بھیجیں
        
        Args:
            message (str): نص الرسالة | Message text | پیغام کا متن
            numbers (Union[List[str], str]): رقم أو قائمة أرقام | Number or list of numbers | نمبر یا نمبروں کی فہرست
            sender (str): اسم المرسل | Sender name | بھیجنے والے کا نام
            
        Returns:
            Dict: نتيجة الإرسال | Sending result | بھیجنے کا نتیجہ
        """
        if isinstance(numbers, str):
            numbers = [numbers]
        
        # تحديد حجم المجموعة | Determine chunk size | حصے کا سائز متعین کریں
        if len(numbers) <= 5:
            chunk_size = len(numbers)
        elif len(numbers) <= 100:
            chunk_size = len(numbers) // 5 + (1 if len(numbers) % 5 else 0)
        else:
            chunk_size = 100
        
        # تقسيم الأرقام | Split numbers | نمبروں کو تقسیم کریں
        chunks = self._chunk_numbers(numbers, chunk_size)
        
        # إرسال متوازي | Parallel sending | متوازی بھیجنا
        with ThreadPoolExecutor() as executor:
            results = list(executor.map(
                lambda chunk: self._send_chunk(message, chunk, sender),
                chunks
            ))
        
        # تجميع النتائج | Aggregate results | نتائج کو یکجا کریں
        aggregated = {
            "success": True,
            "total_success": 0,
            "total_failed": 0,
            "job_ids": [],
            "errors": {}
        }
        
        for result in results:
            if result["success"] and "err_text" not in result["response"].get("messages", [{}])[0]:
                aggregated["total_success"] += len(result["chunk"])
                if "job_id" in result["response"]:
                    aggregated["job_ids"].append(result["response"]["job_id"])
            else:
                aggregated["total_failed"] += len(result["chunk"])
                error_msg = (
                    result["response"].get("messages", [{}])[0].get("err_text")
                    or result.get("error", "Unknown error")
                )
                if error_msg not in aggregated["errors"]:
                    aggregated["errors"][error_msg] = []
                aggregated["errors"][error_msg].extend(result["chunk"])
        
        aggregated["success"] = aggregated["total_failed"] == 0
        return aggregated


# مثال على الاستخدام | Usage example | استعمال کی مثال
if __name__ == "__main__":
    # تهيئة البوابة | Initialize gateway | گیٹ وے کو شروع کریں
    gateway = SMSGateway(
        api_key=os.getenv("SMS_API_KEY", "your_api_key"),
        api_secret=os.getenv("SMS_API_SECRET", "your_api_secret")
    )
    
    # إرسال رسالة | Send message | پیغام بھیجیں
    result = gateway.send_sms(
        message="اختبار الإرسال المتوازي | Parallel sending test | متوازی بھیجنے کا ٹیسٹ",
        numbers=["966500000001", "966500000002", "966500000003"],
        sender="4jawaly"
    )
    
    # طباعة النتيجة | Print result | نتیجہ پرنٹ کریں
    print(json.dumps(result, indent=2, ensure_ascii=False))
