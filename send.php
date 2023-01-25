<?php
$curl = curl_init();
$app_id = "x";
$app_sec = "y";
$app_hash  = base64_encode("$app_id:$app_sec");
$messages = [];
$messages["messages"] = [];
$messages["messages"][0]["text"] = "test php";
$messages["messages"][0]["numbers"][] = "966xxxx";
$messages["messages"][0]["sender"] = "test";

curl_setopt_array($curl, array(
    CURLOPT_URL => 'https://api-sms.4jawaly.com/api/v1/account/area/sms/send',
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_ENCODING => '',
    CURLOPT_MAXREDIRS => 10,
    CURLOPT_TIMEOUT => 0,
    CURLOPT_FOLLOWLOCATION => true,
    CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
    CURLOPT_CUSTOMREQUEST => 'POST',
    CURLOPT_POSTFIELDS =>json_encode($messages),
    CURLOPT_HTTPHEADER => array(
        'Accept: application/json',
        'Content-Type: application/json',
        'Authorization: Basic '.$app_hash
    ),
));

$response = curl_exec($curl);
curl_close($curl);
var_dump(json_decode($response));
