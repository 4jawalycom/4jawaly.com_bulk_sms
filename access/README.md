# MS Access Module for 4Jawaly SMS | وحدة MS Access لخدمة 4Jawaly SMS | 4Jawaly SMS کے لیے MS Access ماڈیول

## 🇸🇦 عربي

### المتطلبات الأساسية
- Microsoft Access 2010 أو أحدث
- اتصال بالإنترنت
- مكتبة MSXML2 (متوفرة عادةً مع Windows)

### التثبيت
1. قم بإنشاء قاعدة بيانات جديدة في MS Access
2. استورد ملف `SMSLog.vb` كوحدة جديدة
3. قم بتنفيذ سكربت `CreateTables.sql` لإنشاء جدول السجلات

### خطوات الاستيراد
1. في MS Access، افتح قاعدة البيانات الخاصة بك
2. اذهب إلى تبويب "Create"
3. اضغط على "Module"
4. انسخ محتوى `SMSLog.vb` إلى المحرر
5. احفظ الوحدة باسم "SMSLog"

### إنشاء الجدول
1. اذهب إلى تبويب "Create"
2. اضغط على "Query Design"
3. أغلق نافذة "Show Table"
4. اضغط على "SQL View"
5. انسخ محتوى `CreateTables.sql`
6. قم بتنفيذ الاستعلام

### الإعداد
1. احصل على API Key و Secret من موقع 4Jawaly:
   - قم بتسجيل الدخول إلى حسابك على [4Jawaly](https://4jawaly.com)
   - اذهب إلى البيانات الشخصية
   - اضغط على API Token
   - شاهد [الفيديو التوضيحي](https://youtu.be/oTB6hLbJXPU?si=Zr6-6HsjsKkyHUR6&t=468) للمزيد من التفاصيل

2. قم بتهيئة الخدمة في نموذج أو وحدة:

```vb
' تهيئة الخدمة
InitializeService "your_api_key", "your_api_secret"
```

### الاستخدام
#### إرسال رسالة SMS

```vb
' إرسال رسالة إلى رقم واحد
Dim result As String
result = SendSMS("Hello from Access!", "966500000000", "SENDER")
MsgBox result

' إرسال رسالة إلى عدة أرقام
result = SendSMS("Hello from Access!", "966500000000,966500000001", "SENDER")
MsgBox result
```

#### التحقق من الرصيد

```vb
Dim balance As String
balance = CheckBalance()
MsgBox balance
```

#### جلب قائمة المرسلين

```vb
Dim senders As String
senders = GetSenders()
MsgBox senders
```

## 🇬🇧 English

### Prerequisites
- Microsoft Access 2010 or later
- Internet connection
- MSXML2 library (usually available with Windows)

### Installation & Setup
1. Create a new database
2. Import the module
3. Create the log table

### Usage
#### Send SMS

```vb
' Send message to one number
Dim result As String
result = SendSMS("Hello from Access!", "966500000000", "SENDER")
MsgBox result

' Send message to multiple numbers
result = SendSMS("Hello from Access!", "966500000000,966500000001", "SENDER")
MsgBox result
```

#### Check Balance

```vb
Dim balance As String
balance = CheckBalance()
MsgBox balance
```

#### Get Senders

```vb
Dim senders As String
senders = GetSenders()
MsgBox senders
```

## 🇵🇰 اردو

### پیش شرائط
- Microsoft Access 2010 یا اس سے نئی ورژن
- انٹرنیٹ کنکشن
- MSXML2 لائبریری (عام طور پر Windows کے ساتھ دستیاب)

### انسٹالیشن اور سیٹ اپ
1. نیا ڈیٹا بیس بنائیں
2. ماڈیول امپورٹ کریں
3. لاگ ٹیبل بنائیں

### استعمال
#### ایس ایم ایس بھیجیں

```vb
' ایک نمبر پر پیغام بھیجیں
Dim result As String
result = SendSMS("Hello from Access!", "966500000000", "SENDER")
MsgBox result

' کئی نمبروں پر پیغام بھیجیں
result = SendSMS("Hello from Access!", "966500000000,966500000001", "SENDER")
MsgBox result
```

#### بیلنس چیک کریں

```vb
Dim balance As String
balance = CheckBalance()
MsgBox balance
```

#### بھیجے جانے والوں کی فہرست حاصل کریں

```vb
Dim senders As String
senders = GetSenders()
MsgBox senders
```

## مثال لنموذج كامل

```vb
Private Sub Form_Load()
    ' تهيئة الخدمة عند تحميل النموذج
    InitializeService "your_api_key", "your_api_secret"
End Sub

Private Sub btnSend_Click()
    ' التحقق من الحقول
    If Len(txtMessage.Value) = 0 Or Len(txtNumbers.Value) = 0 Or Len(txtSender.Value) = 0 Then
        MsgBox "الرجاء تعبئة جميع الحقول", vbExclamation
        Exit Sub
    End If
    
    ' إرسال الرسالة
    Dim result As String
    result = SendSMS(txtMessage.Value, txtNumbers.Value, txtSender.Value)
    
    ' عرض النتيجة
    MsgBox result
End Sub

Private Sub btnCheckBalance_Click()
    ' التحقق من الرصيد
    Dim balance As String
    balance = CheckBalance()
    MsgBox balance
End Sub

Private Sub btnGetSenders_Click()
    ' جلب قائمة المرسلين
    Dim senders As String
    senders = GetSenders()
    MsgBox senders
End Sub
```

## جدول السجلات

يتم تسجيل جميع الرسائل المرسلة في جدول `tblSMSLog` مع المعلومات التالية:
- ID: معرف تسلسلي
- Message: نص الرسالة
- Numbers: أرقام المستلمين
- Sender: اسم المرسل
- Response: الرد من الخادم
- SendDate: تاريخ ووقت الإرسال

## استعلام عن السجلات

```sql
-- جلب آخر 10 رسائل تم إرسالها
SELECT TOP 10 *
FROM tblSMSLog
ORDER BY SendDate DESC;

-- البحث عن رسائل لرقم معين
SELECT *
FROM tblSMSLog
WHERE Numbers LIKE '*966500000000*'
ORDER BY SendDate DESC;
```

## أفضل الممارسات

1. تأكد من وجود اتصال بالإنترنت قبل إرسال الرسائل
2. قم بمعالجة الأخطاء في التطبيق الخاص بك
3. احمِ API Key و Secret الخاصين بك
4. قم بتنظيف السجلات القديمة دورياً

## المزيد من المعلومات

- [توثيق 4Jawaly API](https://4jawaly.com/api-docs)
- [VBA Programming Guide](https://docs.microsoft.com/en-us/office/vba/api/overview/)
- [الأسئلة الشائعة](../FAQ.md)
- [رخصة المشروع](../LICENSE)
