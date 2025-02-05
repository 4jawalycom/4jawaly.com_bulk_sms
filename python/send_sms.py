import requests
import json
import base64

'''يمكنك الحصول علي api key ,secrtet 
من خلال تسجيل الدخول بموقع فورجوالي و الذهاب الي البيانات الشخصية ثم الضغط علي 
api token
'''

app_id = "api key"
app_sec = "api secret"
app_hash = base64.b64encode(f"{app_id}:{app_sec}".encode()).decode()

messages = {
    "messages": [
        {
            "text": "test",
            "numbers": ["9665000000"],
            "sender": "4jawaly"
        }
    ]
}

url = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send"
headers = {
     "Accept": "application/json",
     "Content-Type": "application/json",
    "Authorization": f"Basic {app_hash}"
}

response = requests.post(url, headers=headers, json=messages)
#response_json = json.loads(response.text)

#print(response.text)
#print(response.status_code)
response_json = response.json()


if response.status_code == 200:
    response_json = json.loads(response.text)
    if "err_text" in response_json["messages"][0]:
        print(response_json["messages"][0]["err_text"])
    else:
        print("تم الارسال بنجاح  " + " job id:" + response_json["job_id"])
elif response.status_code == 400:
    response_json = json.loads(response.text)
    print(response_json["message"])
elif response.status_code == 422:
    print("نص الرسالة فارغ")
else :
    print( f'محظور بواسطة كلاودفلير. Status code: {response.status_code}')
