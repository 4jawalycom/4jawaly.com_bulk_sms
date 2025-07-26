<?php

$app_id = "API_KEY";     // Replace with your actual API KEY
$app_sec = "API_SECRET"; // Replace with your actual API SECRET

$app_hash = base64_encode("$app_id:$app_sec");
$base_url = "https://api-sms.4jawaly.com/api/v1/";

// Build query params
$query = [];
$query["is_active"] = 1; // get active only
$query["order_by"] = "id"; // or package_points, current_points, expire_at
$query["order_by_type"] = "desc"; // desc or asc
$query["page"] = 1;
$query["page_size"] = 10;
$query["return_collection"] = 1; // get all collection

$url = $base_url . 'account/area/me/packages?' . http_build_query($query);

$headers = [
    'Accept: application/json',
    'Content-Type: application/json',
    'Authorization: Basic ' . $app_hash
];

// Initialize curl
$curl = curl_init();
curl_setopt_array($curl, array(
    CURLOPT_URL => $url,
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_ENCODING => '',
    CURLOPT_MAXREDIRS => 10,
    CURLOPT_TIMEOUT => 0,
    CURLOPT_FOLLOWLOCATION => true,
    CURLOPT_HTTP_VERSION => CURL_HTTP_VERSION_1_1,
    CURLOPT_CUSTOMREQUEST => 'GET',
    CURLOPT_HTTPHEADER => $headers,
));

$response = curl_exec($curl);

if (curl_errno($curl)) {
    die("cURL Error: " . curl_error($curl));
}
$http_code = curl_getinfo($curl, CURLINFO_HTTP_CODE);

curl_close($curl);

// Parse response
$data = json_decode($response, true);

header('Content-Type: application/json; charset=utf-8');

// Handle different outcomes
if ($http_code == 200 && isset($data["code"]) && $data["code"] == 200) {
    if (isset($data["collection"]) && !empty($data["collection"])) {
        // Print full collection as pretty JSON
        echo json_encode($data["collection"], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
    } elseif (isset($data["total_balance"])) {
        // Print only total balance
        echo json_encode(["total_balance" => $data["total_balance"]], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
    } else {
        // Unexpected success response
        echo json_encode(["message" => "No package data found.", "response" => $data], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
    }
} else {
    // API error
    $error_msg = $data["message"] ?? "Failed to fetch packages data";
    echo json_encode(["message" => $error_msg, "error" => $data], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
}
