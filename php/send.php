<?php

$app_id = "api key";
$app_sec = "api secret";
$app_hash = base64_encode("{$app_id}:{$app_sec}");

$messages = [
    "messages" => [
        [
            "text" => "test",
            "numbers" => ["9665000000"],
            "sender" => "4jawaly"
        ]
    ]
];

$url = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send";
$headers = [
    "Accept: application/json",
    "Content-Type: application/json",
    "Authorization: Basic {$app_hash}"
];

$curl = curl_init($url);
curl_setopt($curl, CURLOPT_POST, true);
curl_setopt($curl, CURLOPT_POSTFIELDS, json_encode($messages));
curl_setopt($curl, CURLOPT_HTTPHEADER, $headers);
curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);

$response = curl_exec($curl);
$status_code = curl_getinfo($curl, CURLINFO_HTTP_CODE);
curl_close($curl);

$response_json = json_decode($response, true);

if ($status_code == 200) {
    if (isset($response_json["messages"][0]["err_text"])) {
        echo $response_json["messages"][0]["err_text"];
    } else {
        echo "تم الارسال بنجاح  " . " job id:" . $response_json["job_id"];
    }
} elseif ($status_code == 400) {
    echo $response_json["message"];
} elseif ($status_code == 422) {
    echo "نص الرسالة فارغ";
} else {
    echo "محظور بواسطة كلاودفلير. Status code: {$status_code}";
}

