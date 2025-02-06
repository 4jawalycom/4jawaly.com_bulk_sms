<?php
require_once __DIR__ . '/vendor/autoload.php';

use Jawalycom\SMSGateway4Jawaly\SMSGateway;

// تهيئة بوابة الرسائل
// Initialize SMS Gateway
// ایس ایم ایس گیٹ وے کو شروع کریں
$config = [
    'api_key' => 'YOUR_API_KEY',
    'api_secret' => 'YOUR_API_SECRET',
    'default_sender' => '4jawaly'
];

$smsGateway = new SMSGateway($config);

// مثال 1: إرسال رسالة لرقم واحد
// Example 1: Send message to one number
// مثال 1: ایک نمبر پر پیغام بھیجیں
try {
    $result = $smsGateway->send(
        numbers: ['966500000000'],
        message: 'تجربة رسالة 1',
        sender: '4jawaly'
    );
    var_dump($result);
} catch (Exception $e) {
    echo "Error: " . $e->getMessage() . "\n";
}

// مثال 2: إرسال رسالة لعدة أرقام
// Example 2: Send message to multiple numbers
// مثال 2: متعدد نمبروں پر پیغام بھیجیں
try {
    $result = $smsGateway->send(
        numbers: ['966500000000', '966500000001'],
        message: 'تجربة رسالة 2',
        sender: '4jawaly'
    );
    var_dump($result);
} catch (Exception $e) {
    echo "Error: " . $e->getMessage() . "\n";
}

// مثال 3: الحصول على الرصيد
// Example 3: Get balance
// مثال 3: بیلنس حاصل کریں
try {
    $balance = $smsGateway->getBalance();
    var_dump($balance);
} catch (Exception $e) {
    echo "Error: " . $e->getMessage() . "\n";
}

// مثال 4: الحصول على أسماء المرسلين
// Example 4: Get sender names
// مثال 4: بھیجنے والوں کے نام حاصل کریں
try {
    $senders = $smsGateway->getSenderNames();
    var_dump($senders);
} catch (Exception $e) {
    echo "Error: " . $e->getMessage() . "\n";
}
