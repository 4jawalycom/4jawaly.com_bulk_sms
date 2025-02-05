# Node.js Library for 4Jawaly SMS | مكتبة Node.js لخدمة 4Jawaly SMS | 4Jawaly SMS کے لیے Node.js لائبریری

## 🇸🇦 عربي

### المتطلبات الأساسية
- Node.js 14.0.0 أو أحدث
- npm (مدير حزم Node.js)

### التثبيت
```bash
npm install axios
```

### الإعداد
1. احصل على API Key و Secret من موقع 4Jawaly:
   - قم بتسجيل الدخول إلى حسابك على [4Jawaly](https://4jawaly.com)
   - اذهب إلى البيانات الشخصية
   - اضغط على API Token
   - شاهد [الفيديو التوضيحي](https://youtu.be/oTB6hLbJXPU?si=Zr6-6HsjsKkyHUR6&t=468)

2. قم بإعداد متغيرات البيئة:
```bash
export JAWALY_API_KEY="your_api_key"
export JAWALY_API_SECRET="your_api_secret"
```

### الاستخدام

#### إرسال رسالة SMS
```javascript
const { sendSMS } = require('./sms');

// إرسال رسالة
sendSMS({
    message: "مرحباً من Node.js!",
    numbers: ["966500000000"],
    sender: "SENDER"
})
.then(response => console.log('تم الإرسال:', response))
.catch(error => console.error('خطأ:', error));
```

#### التحقق من الرصيد
```javascript
const { checkBalance } = require('./sms');

// التحقق من الرصيد
checkBalance()
.then(balance => console.log('الرصيد:', balance))
.catch(error => console.error('خطأ:', error));
```

#### جلب قائمة المرسلين
```javascript
const { getSenders } = require('./sms');

// جلب المرسلين
getSenders()
.then(senders => console.log('المرسلون:', senders))
.catch(error => console.error('خطأ:', error));
```

## 🇬🇧 English

### Prerequisites
- Node.js 14.0.0 or later
- npm (Node.js package manager)

### Installation
```bash
npm install axios
```

### Setup
1. Get API Key & Secret from 4Jawaly website:
   - Login to your [4Jawaly](https://4jawaly.com) account
   - Go to Personal Information
   - Click on API Token
   - Watch the [tutorial video](https://youtu.be/oTB6hLbJXPU?si=Zr6-6HsjsKkyHUR6&t=468)

2. Set up environment variables:
```bash
export JAWALY_API_KEY="your_api_key"
export JAWALY_API_SECRET="your_api_secret"
```

### Usage

#### Send SMS
```javascript
const { sendSMS } = require('./sms');

// Send message
sendSMS({
    message: "Hello from Node.js!",
    numbers: ["966500000000"],
    sender: "SENDER"
})
.then(response => console.log('Sent:', response))
.catch(error => console.error('Error:', error));
```

#### Check Balance
```javascript
const { checkBalance } = require('./sms');

// Check balance
checkBalance()
.then(balance => console.log('Balance:', balance))
.catch(error => console.error('Error:', error));
```

#### Get Senders List
```javascript
const { getSenders } = require('./sms');

// Get senders
getSenders()
.then(senders => console.log('Senders:', senders))
.catch(error => console.error('Error:', error));
```

## 🇵🇰 اردو

### پیش شرائط
- Node.js 14.0.0 یا اس سے نئی ورژن
- npm (Node.js پیکج مینیجر)

### انسٹالیشن
```bash
npm install axios
```

### سیٹ اپ
1. 4Jawaly ویب سائٹ سے API Key اور Secret حاصل کریں:
   - اپنے [4Jawaly](https://4jawaly.com) اکاؤنٹ میں لاگ ان کریں
   - ذاتی معلومات پر جائیں
   - API Token پر کلک کریں
   - [ٹیوٹوریل ویڈیو](https://youtu.be/oTB6hLbJXPU?si=Zr6-6HsjsKkyHUR6&t=468) دیکھیں

2. ماحولیاتی متغیرات سیٹ کریں:
```bash
export JAWALY_API_KEY="your_api_key"
export JAWALY_API_SECRET="your_api_secret"
```

### استعمال

#### ایس ایم ایس بھیجیں
```javascript
const { sendSMS } = require('./sms');

// پیغام بھیجیں
sendSMS({
    message: "Node.js سے ہیلو!",
    numbers: ["966500000000"],
    sender: "SENDER"
})
.then(response => console.log('بھیج دیا گیا:', response))
.catch(error => console.error('خرابی:', error));
```

#### بیلنس چیک کریں
```javascript
const { checkBalance } = require('./sms');

// بیلنس چیک کریں
checkBalance()
.then(balance => console.log('بیلنس:', balance))
.catch(error => console.error('خرابی:', error));
```

#### بھیجنے والوں کی فہرست حاصل کریں
```javascript
const { getSenders } = require('./sms');

// بھیجنے والے حاصل کریں
getSenders()
.then(senders => console.log('بھیجنے والے:', senders))
.catch(error => console.error('خرابی:', error));
