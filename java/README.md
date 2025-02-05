# مكتبة Java لخدمة 4Jawaly SMS

## المتطلبات الأساسية

- JDK 11 أو أحدث
- Maven (إذا كنت تستخدم إدارة التبعيات)

## التثبيت

### باستخدام Maven

أضف التبعية التالية إلى ملف `pom.xml`:

```xml
<dependency>
    <groupId>org.json</groupId>
    <artifactId>json</artifactId>
    <version>20231013</version>
</dependency>
```

## الإعداد

1. احصل على API Key و Secret من موقع 4Jawaly:
   - قم بتسجيل الدخول إلى حسابك على [4Jawaly](https://4jawaly.com)
   - اذهب إلى البيانات الشخصية
   - اضغط على API Token
   - شاهد [الفيديو التوضيحي](https://youtu.be/oTB6hLbJXPU?si=Zr6-6HsjsKkyHUR6&t=468) للمزيد من التفاصيل

2. قم بتحديث ملف `Config.java` بمعلومات API الخاصة بك:
```java
public class Config {
    private static final String APP_ID = "your_api_key";
    private static final String APP_SECRET = "your_api_secret";
    // ...
}
```

## الاستخدام

### إرسال رسالة SMS

```java
JawalySMSService smsService = new JawalySMSService("your_api_key", "your_api_secret");

List<String> numbers = Arrays.asList("966500000000");
Future<Result<String>> future = smsService.sendSMS("Hello from Java!", numbers, "SENDER");

// انتظار النتيجة
Result<String> result = future.get();
if (result.isSuccess()) {
    System.out.println("Message sent successfully! Job ID: " + result.getData());
} else {
    System.out.println("Error: " + result.getError());
}
```

### التحقق من الرصيد

```java
Future<Result<Double>> future = smsService.checkBalance();
Result<Double> result = future.get();
if (result.isSuccess()) {
    System.out.println("Balance: " + result.getData());
} else {
    System.out.println("Error: " + result.getError());
}
```

### جلب قائمة المرسلين

```java
Future<Result<List<String>>> future = smsService.getSenders();
Result<List<String>> result = future.get();
if (result.isSuccess()) {
    System.out.println("Senders: " + result.getData());
} else {
    System.out.println("Error: " + result.getError());
}
```

## إغلاق الخدمة

```java
smsService.shutdown();
```

## معالجة الأخطاء

المكتبة تستخدم class `Result` للتعامل مع النجاح والفشل:

```java
public class Result<T> {
    private final T data;
    private final String error;
    
    public boolean isSuccess() {
        return error == null;
    }
    
    public T getData() {
        return data;
    }
    
    public String getError() {
        return error;
    }
}
```

## ملاحظات هامة

- تأكد من حماية API Key و Secret الخاصين بك
- استخدم HTTPS دائماً للاتصال بالخدمة
- تعامل مع الأخطاء بشكل مناسب في تطبيقك
- أغلق الخدمة عند الانتهاء لتحرير الموارد

## المزيد من المعلومات

- [توثيق 4Jawaly API](https://4jawaly.com/api-docs)
- [الأسئلة الشائعة](../FAQ.md)
- [رخصة المشروع](../LICENSE)
