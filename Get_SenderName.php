<?php

$app_id = "x";
$app_sec = "x";
$app_hash  = base64_encode("$app_id:$app_sec");
$base_url = "https://api-sms.4jawaly.com/api/v1/";
$query = [];
$query["page_size"] = 10; // if you want pagination how many items per page
$query["page"] = 1;// page number
$query["status"] = 1; // get active 1 in active 2
$query["sender_name"] = ''; // search sender name full name
$query["is_ad"] = ''; // for ads 1 and 2 for not ads
$query["return_collection"] = 1; // if you want to get collection for all not pagination
$curl = curl_init();
curl_setopt_array($curl, array(
    CURLOPT_URL => $base_url.'account/area/senders?'.http_build_query($query),
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_ENCODING => '',
    CURLOPT_MAXREDIRS => 10,
    CURLOPT_TIMEOUT => 0,
    CURLOPT_FOLLOWLOCATION => true,
    CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
    CURLOPT_CUSTOMREQUEST => 'GET',
    CURLOPT_HTTPHEADER => array(
        'Accept: application/json',
        'Content-Type: application/json',
        'Authorization: Basic '.$app_hash
    ),
));

$response = curl_exec($curl);

curl_close($curl);

var_dump(json_decode($response));
