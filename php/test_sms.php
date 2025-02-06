<?php

require_once __DIR__ . '/vendor/autoload.php';

use Jawalycom\SMSGateway4Jawaly\SMSGateway;

class SMSBatchSender
{
    private $config;

    public function __construct($config)
    {
        $this->config = $config;
    }

    public function sendSMSBatch($numbers, $message, $sender, $app_id, $app_sec)
    {
        $smsGateway = new SMSGateway($this->config);

        try {
            // تقسيم الأرقام إلى مجموعات من 5
            $number_chunks = array_chunk($numbers, 5);

            // تهيئة multi curl
            $mh = curl_multi_init();
            $channels = [];

            // إعداد طلب منفصل لكل مجموعة من 5 أرقام
            foreach ($number_chunks as $chunk_index => $number_chunk) {
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
                            "numbers" => $number_chunk,
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
                
                curl_multi_add_handle($mh, $ch);
                $channels[$chunk_index] = [
                    'handle' => $ch,
                    'numbers' => $number_chunk
                ];
            }

            // تنفيذ الطلبات بشكل متوازي
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

            // جمع النتائج
            $results = [];
            foreach ($channels as $chunk_index => $channel) {
                $ch = $channel['handle'];
                $numbers = $channel['numbers'];
                
                $response = curl_multi_getcontent($ch);
                $status_code = curl_getinfo($ch, CURLINFO_HTTP_CODE);
                $response_json = json_decode($response, true);
                
                $results[] = [
                    'numbers' => $numbers,
                    'status_code' => $status_code,
                    'response' => $response_json
                ];
                
                curl_multi_remove_handle($mh, $ch);
                curl_close($ch);
            }

            curl_multi_close($mh);

            return new SMSBatchResult($results);
        } catch (Exception $e) {
            echo "Error occurred: " . $e->getMessage() . "\n";
        }
    }
}

class SMSBatchResult
{
    private $results;

    public function __construct($results)
    {
        $this->results = $results;
    }

    public function generateReport()
    {
        $report = "";
        foreach ($this->results as $result) {
            $report .= "\nنتيجة الإرسال للمجموعة:\n";
            $report .= "الأرقام: " . implode(", ", $result['numbers']) . "\n";
            
            if ($result['status_code'] == 200) {
                if (isset($result['response']["messages"][0]["err_text"])) {
                    $report .= "خطأ: " . $result['response']["messages"][0]["err_text"] . "\n";
                } else {
                    $report .= "تم الإرسال بنجاح - job id: " . $result['response']["job_id"] . "\n";
                    if (isset($result['response']["messages"][0]["error_numbers"]) && !empty($result['response']["messages"][0]["error_numbers"])) {
                        $report .= "الأرقام التي بها مشاكل:\n";
                        foreach ($result['response']["messages"][0]["error_numbers"] as $error) {
                            $report .= "- الرقم: " . $error["number"] . " - " . $error["error"] . "\n";
                        }
                    }
                }
            } else {
                $report .= "خطأ في الإرسال - Status code: {$result['status_code']}\n";
            }
        }
        return $report;
    }
}

$config = [
    'api_key' => 'YOUR_API_KEY',
    'api_secret' => 'YOUR_API_SECRET',
    'default_sender' => '4jawaly'
];

$sender = new SMSBatchSender($config);

// بيانات الإرسال
$app_id = "YOUR_API_KEY";
$app_sec = "YOUR_API_SECRET";

// قائمة الأرقام للتجربة
$numbers = [
    "966500000000", "966500000001", "966500000002", "966500000003",
    "966500000004", "966500000005", "966500000006", "966500000007"
];

// نص الرسالة
$message = "تجربة الارسال المتعدد";

// إرسال الرسائل
echo "=== بدء الإرسال ===\n";
$result = $sender->sendSMSBatch($numbers, $message, "4jawaly", $app_id, $app_sec);

// عرض التقرير
echo $result->generateReport();
