<?php

$app_id = "x";
$app_sec = "x";
$app_hash  = base64_encode("$app_id:$app_sec");
$base_url = "https://api-sms.4jawaly.com/api/v1/";
$query = [];
$query["is_active"] = 1; // get active only
$query["order_by"] = "id"; // package_points, current_points, expire_at or id (default)
$query["order_by_type"] = "desc"; // desc or asc
$query["page"] = 1 ;
$query["page_size"] = 10 ;
$query["return_collection"] = 1; // if you want to get all collection
$curl = curl_init();
curl_setopt_array($curl, array(
    CURLOPT_URL => $base_url.'account/area/me/packages?'.http_build_query($query),
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
