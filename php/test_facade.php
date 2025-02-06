<?php

require_once __DIR__ . '/vendor/autoload.php';

use Jawalycom\SMSGateway4Jawaly\Facades\SMSGateway;

// تهيئة بوابة الرسائل
// Initialize SMS Gateway
// ایس ایم ایس گیٹ وے کو شروع کریں
$config = [
    'api_key' => 'YOUR_API_KEY',
    'api_secret' => 'YOUR_API_SECRET',
    'default_sender' => '4jawaly'
];

SMSGateway::init($config);

// إرسال رسالة SMS
// Send SMS message
// ایس ایم ایس پیغام بھیجیں
$result = SMSGateway::send([
    'numbers' => ['966500000000'],  // رقم الجوال مع كود الدولة
    'message' => 'تجربة إرسال الرسائل',
    'sender' => '4jawaly'  // اسم المرسل
]);

print_r($result);
