<?php

// Enter your API credentials
$app_id = "API_KEY";     // Replace with your real API KEY
$app_sec = "API_SECRET"; // Replace with your real API SECRET

// Encode the credentials for Basic Auth
$app_hash = base64_encode("$app_id:$app_sec");

// API base URL
$base_url = "https://api-sms.4jawaly.com/api/v1/";

// Request parameters
$query = [
    "page_size" => 10,
    "page" => 1,
    "status" => 1,          // 1 = only active senders
    "return_collection" => 1
];

// Build the full request URL
$url = $base_url . "account/area/senders?" . http_build_query($query);

// Request headers
$headers = [
    "Accept: application/json",
    "Content-Type: application/json",
    "Authorization: Basic $app_hash"
];

// Initialize cURL
$ch = curl_init();
curl_setopt($ch, CURLOPT_URL, $url);
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false); // Only disable in safe/dev environment

// Execute the request
$response = curl_exec($ch);
$http_code = curl_getinfo($ch, CURLINFO_HTTP_CODE);

// Check for cURL errors
if (curl_errno($ch)) {
    die("cURL Error: " . curl_error($ch));
}
curl_close($ch);

// Parse the response
$data = json_decode($response, true);

// Handle the result
if ($http_code == 200 && isset($data['items']) && !empty($data['items'])) {
    $senders_list = [];
    foreach ($data['items'] as $item) {
        $senders_list[] = [
            "sender_name" => $item['sender_name'],
            "is_default" => (bool)$item['is_default']
        ];
    }
    header('Content-Type: application/json; charset=utf-8');
    echo json_encode($senders_list, JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
} else {
    $error_msg = $data['message'] ?? "Failed to fetch sender names";
    header('Content-Type: application/json; charset=utf-8');
    echo json_encode(["message" => $error_msg, "error" => $data], JSON_UNESCAPED_UNICODE | JSON_PRETTY_PRINT);
}
