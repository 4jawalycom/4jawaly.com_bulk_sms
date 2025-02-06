<?php

/**
 * نتيجة إرسال الرسائل النصية
 * SMS Sending Result Class
 * ایس ایم ایس بھیجنے کا نتیجہ
 */
class SMSResult {
    private $total_success = 0;
    private $total_failed = 0;
    private $errors = [];
    private $job_ids = [];

    /**
     * إضافة نتيجة إرسال لمجموعة من الأرقام
     * Add sending result for a batch of numbers
     * نمبروں کے بیچ کے لئے بھیجنے کا نتیجہ شامل کریں
     */
    public function addBatchResult($numbers, $response, $status_code) {
        if ($status_code == 200) {
            if (isset($response["job_id"])) {
                $this->job_ids[] = $response["job_id"];
            }
            
            if (isset($response["messages"][0]["err_text"])) {
                $this->addError($numbers, $response["messages"][0]["err_text"]);
                $this->total_failed += count($numbers);
            } else {
                $success_count = count($numbers);
                if (isset($response["messages"][0]["error_numbers"]) && !empty($response["messages"][0]["error_numbers"])) {
                    foreach ($response["messages"][0]["error_numbers"] as $error) {
                        $this->addError([$error["number"]], $error["error"]);
                        $success_count--;
                        $this->total_failed++;
                    }
                }
                $this->total_success += $success_count;
            }
        } else {
            $this->addError($numbers, "خطأ في الإرسال - Status code: {$status_code}");
            $this->total_failed += count($numbers);
        }
    }

    /**
     * إضافة خطأ للقائمة
     * Add error to the list
     * فہرست میں خرابی شامل کریں
     */
    private function addError($numbers, $error) {
        if (!isset($this->errors[$error])) {
            $this->errors[$error] = [];
        }
        $this->errors[$error] = array_merge($this->errors[$error], $numbers);
    }

    /**
     * إنشاء تقرير مفصل عن نتيجة الإرسال
     * Generate detailed report about sending result
     * بھیجنے کے نتیجے کے بارے میں تفصیلی رپورٹ تیار کریں
     */
    public function generateReport() {
        $report = "";
        
        // إضافة ملخص الإرسال
        // Add sending summary
        // بھیجنے کا خلاصہ شامل کریں
        $report .= "\n=== ملخص الإرسال ===\n";
        $report .= "إجمالي الرسائل الناجحة: " . $this->total_success . "\n";
        $report .= "إجمالي الرسائل الفاشلة: " . $this->total_failed . "\n";
        
        // إضافة قائمة الأخطاء
        // Add error list
        // خرابیوں کی فہرست شامل کریں
        if (!empty($this->errors)) {
            $report .= "\n=== تفاصيل الأخطاء ===\n";
            foreach ($this->errors as $error => $numbers) {
                $report .= "الخطأ: " . $error . "\n";
                $report .= "الأرقام المتأثرة: " . implode(", ", $numbers) . "\n\n";
            }
        }
        
        return $report;
    }
}

/**
 * إرسال رسائل SMS لمجموعة من الأرقام باستخدام 4jawaly
 * Send SMS messages to a group of numbers using 4jawaly
 * 4jawaly کا استعمال کرتے ہوئے نمبروں کے گروپ کو ایس ایم ایس پیغامات بھیجیں
 * 
 * @param array  $numbers قائمة الأرقام | List of numbers | نمبروں کی فہرست
 * @param string $message نص الرسالة | Message text | پیغام کا متن
 * @param string $sender اسم المرسل | Sender name | بھیجنے والے کا نام
 * @param string $app_id معرف التطبيق | Application ID | ایپلیکیشن آئی ڈی
 * @param string $app_sec كلمة السر | Application secret | ایپلیکیشن خفیہ
 * @return SMSResult نتيجة الإرسال | Sending result | بھیجنے کا نتیجہ
 */
function sendSMSBatch($numbers, $message, $sender, $app_id, $app_sec) {
    $result = new SMSResult();
    
    try {
        // تقسيم الأرقام إلى مجموعات
        // Split numbers into groups
        // نمبروں کو گروپوں میں تقسیم کریں
        $batch_size = count($numbers) <= 5 ? 1 : (count($numbers) <= 100 ? 5 : 100);
        $number_chunks = array_chunk($numbers, $batch_size);
        
        // تهيئة multi curl
        // Initialize multi curl
        // ملٹی کرل کو شروع کریں
        $mh = curl_multi_init();
        $channels = [];
        
        // إعداد طلب لكل مجموعة
        // Setup request for each group
        // ہر گروپ کے لئے درخواست تیار کریں
        foreach ($number_chunks as $chunk_index => $number_chunk) {
            $ch = createCurlHandle($number_chunk, $message, $sender, $app_id, $app_sec);
            curl_multi_add_handle($mh, $ch);
            $channels[$chunk_index] = [
                'handle' => $ch,
                'numbers' => $number_chunk
            ];
        }
        
        // تنفيذ الطلبات بشكل متوازي
        // Execute requests in parallel
        // درخواستوں کو متوازی طور پر چلائیں
        $active = null;
        do {
            $mrc = curl_multi_exec($mh, $active);
        } while ($mrc == CURLM_CALL_MULTI_PERFORM);
        
        while ($active && $mrc == CURLM_OK) {
            if (curl_multi_select($mh) != -1) {
                do {
                    $mrc = curl_multi_exec($mh, $active);
                } while ($mrc == CURLM_CALL_MULTI_PERFORM);
            }
        }
        
        // معالجة النتائج
        // Process results
        // نتائج کی پروسیسنگ
        foreach ($channels as $chunk_index => $channel) {
            $ch = $channel['handle'];
            $numbers = $channel['numbers'];
            
            $response = curl_multi_getcontent($ch);
            $status_code = curl_getinfo($ch, CURLINFO_HTTP_CODE);
            $response_json = json_decode($response, true);
            
            // تحديث النتيجة
            // Update result
            // نتیجہ کو اپ ڈیٹ کریں
            $result->addBatchResult($numbers, $response_json, $status_code);
            
            curl_multi_remove_handle($mh, $ch);
            curl_close($ch);
        }
        
        curl_multi_close($mh);
        
    } catch (Exception $e) {
        // في حالة حدوث خطأ، إضافة جميع الأرقام كفاشلة
        // In case of error, add all numbers as failed
        // خرابی کی صورت میں، تمام نمبروں کو ناکام کے طور پر شامل کریں
        $result->addBatchResult($numbers, ["error" => $e->getMessage()], 500);
    }
    
    return $result;
}

/**
 * إنشاء طلب cURL جديد
 * Create new cURL request
 * نئی cURL درخواست بنائیں
 */
function createCurlHandle($numbers, $message, $sender, $app_id, $app_sec) {
    $url = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send";
    $headers = [
        "Accept: application/json",
        "Content-Type: application/json",
        "Authorization: Basic " . base64_encode("{$app_id}:{$app_sec}")
    ];
    
    $messages = [
        "messages" => [
            [
                "text" => $message,
                "numbers" => $numbers,
                "sender" => $sender
            ]
        ]
    ];
    
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($messages));
    curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    
    return $ch;
}

// بيانات الإرسال
// Sending data
// بھیجنے کا ڈیٹا
$app_id = "";
$app_sec = "";

// الأرقام للتجربة
// Test numbers
// ٹیسٹ نمبرز
$numbers = array_fill(0, 24, "966500000000");
$message = "تجربة ارسال الرسائل النصية من موقع فورجوالي";
$sender = "4jawaly";

// إرسال الرسائل
// Send messages
// پیغامات بھیجیں
echo "=== بدء الإرسال ===\n";
$result = sendSMSBatch($numbers, $message, $sender, $app_id, $app_sec);

// عرض التقرير
// Display report
// رپورٹ کو دکھائیں
echo $result->generateReport();
