# Kotlin Library for 4Jawaly SMS | مكتبة Kotlin لخدمة 4Jawaly SMS | 4Jawaly SMS کے لیے Kotlin لائبریری

## 🇸🇦 عربي

### المتطلبات الأساسية
- Android Studio
- Kotlin 1.5.0 أو أحدث
- minSdkVersion 21 أو أعلى

### التثبيت والإعداد
1. أضف التبعيات في ملف build.gradle
2. احصل على API Key و Secret من موقع 4Jawaly
3. قم بإنشاء instance من الخدمة

### الاستخدام
راجع الأمثلة أدناه لكيفية استخدام المكتبة

## 🇬🇧 English

### Prerequisites
- Android Studio
- Kotlin 1.5.0 or later
- minSdkVersion 21 or higher

### Installation & Setup
1. Add dependencies to build.gradle
2. Get API Key & Secret from 4Jawaly website
3. Create service instance

### Usage
See examples below for library usage

## 🇵🇰 اردو

### پیش شرائط
- Android Studio
- Kotlin 1.5.0 یا اس سے نئی ورژن
- minSdkVersion 21 یا اس سے زیادہ

### انسٹالیشن اور سیٹ اپ
1. build.gradle میں ڈیپنڈنسیز شامل کریں
2. 4Jawaly ویب سائٹ سے API Key اور Secret حاصل کریں
3. سروس انسٹنس بنائیں

### استعمال
لائبریری کے استعمال کے لیے ذیل کی مثالیں دیکھیں

## التثبيت

### Gradle

أضف التبعيات التالية إلى ملف `build.gradle` الخاص بالتطبيق:

```gradle
dependencies {
    implementation "org.jetbrains.kotlinx:kotlinx-coroutines-android:1.6.4"
    implementation "org.json:json:20231013"
}
```

## الإعداد

1. احصل على API Key و Secret من موقع 4Jawaly:
   - قم بتسجيل الدخول إلى حسابك على [4Jawaly](https://4jawaly.com)
   - اذهب إلى البيانات الشخصية
   - اضغط على API Token
   - شاهد [الفيديو التوضيحي](https://youtu.be/oTB6hLbJXPU?si=Zr6-6HsjsKkyHUR6&t=468) للمزيد من التفاصيل

2. قم بإنشاء instance من JawalySMSService:

```kotlin
val smsService = JawalySMSService(
    appId = "your_api_key",
    appSecret = "your_api_secret"
)
```

## الاستخدام

### إرسال رسالة SMS

```kotlin
// في Coroutine Scope
lifecycleScope.launch {
    val result = smsService.sendSMS(
        message = "Hello from Kotlin!",
        numbers = listOf("966500000000"),
        sender = "SENDER"
    )
    
    when (result) {
        is Result.Success -> println("Message sent successfully! Job ID: ${result.data}")
        is Result.Error -> println("Error: ${result.message}")
    }
}
```

### التحقق من الرصيد

```kotlin
lifecycleScope.launch {
    when (val result = smsService.checkBalance()) {
        is Result.Success -> println("Balance: ${result.data}")
        is Result.Error -> println("Error: ${result.message}")
    }
}
```

### جلب قائمة المرسلين

```kotlin
lifecycleScope.launch {
    when (val result = smsService.getSenders()) {
        is Result.Success -> println("Senders: ${result.data}")
        is Result.Error -> println("Error: ${result.message}")
    }
}
```

## معالجة الأخطاء | Error Handling | خرابی کا علاج

```kotlin
sealed class Result<out T> {
    data class Success<T>(val data: T) : Result<T>()
    data class Error(val message: String) : Result<Nothing>()
}
```

## أفضل الممارسات

1. استخدم Coroutines للعمليات غير المتزامنة
2. تعامل مع الأخطاء بشكل مناسب
3. احمِ API Key و Secret الخاصين بك
4. استخدم HTTPS دائماً
5. تأكد من وجود إذن الإنترنت في AndroidManifest.xml:

```xml
<uses-permission android:name="android.permission.INTERNET" />
```

## مثال كامل في Activity

```kotlin
class MainActivity : AppCompatActivity() {
    private lateinit var smsService: JawalySMSService
    
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        
        smsService = JawalySMSService(
            appId = "your_api_key",
            appSecret = "your_api_secret"
        )
        
        // مثال لإرسال رسالة عند الضغط على زر
        sendButton.setOnClickListener {
            lifecycleScope.launch {
                val result = smsService.sendSMS(
                    message = messageEditText.text.toString(),
                    numbers = listOf(phoneEditText.text.toString()),
                    sender = senderEditText.text.toString()
                )
                
                when (result) {
                    is Result.Success -> showSuccess("Message sent!")
                    is Result.Error -> showError(result.message)
                }
            }
        }
    }
}
```

## المزيد من المعلومات

- [توثيق 4Jawaly API](https://4jawaly.com/api-docs)
- [Kotlin Coroutines](https://kotlinlang.org/docs/coroutines-overview.html)
- [الأسئلة الشائعة](../../FAQ.md)
- [رخصة المشروع](../../LICENSE)
