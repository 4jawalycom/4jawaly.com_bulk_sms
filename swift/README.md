# Swift Library for 4Jawaly SMS | مكتبة Swift لخدمة 4Jawaly SMS | 4Jawaly SMS کے لیے Swift لائبریری

## 🇸🇦 عربي

### المتطلبات الأساسية
- Xcode 12.0 أو أحدث
- iOS 13.0 أو أحدث
- Swift 5.0 أو أحدث

### التثبيت

#### CocoaPods

```ruby
pod 'JawalySMS'
```

#### Swift Package Manager

```swift
dependencies: [
    .package(url: "https://github.com/yourusername/JawalySMS.git", from: "1.0.0")
]
```

#### يدوياً

قم بنسخ ملف `JawalySMSService.swift` إلى مشروعك.

### الإعداد

1. احصل على API Key و Secret من موقع 4Jawaly:
   - قم بتسجيل الدخول إلى حسابك على [4Jawaly](https://4jawaly.com)
   - اذهب إلى البيانات الشخصية
   - اضغط على API Token
   - شاهد [الفيديو التوضيحي](https://youtu.be/oTB6hLbJXPU?si=Zr6-6HsjsKkyHUR6&t=468) للمزيد من التفاصيل

2. قم بإنشاء instance من JawalySMSService:

```swift
let smsService = JawalySMSService(
    appID: "your_api_key",
    appSecret: "your_api_secret"
)
```

### الاستخدام

#### إرسال رسالة SMS

```swift
smsService.sendSMS(
    message: "Hello from Swift!",
    numbers: ["966500000000"],
    sender: "SENDER"
) { result in
    switch result {
    case .success(let jobId):
        print("Message sent successfully! Job ID: \(jobId)")
    case .failure(let error):
        print("Error: \(error.localizedDescription)")
    }
}
```

#### التحقق من الرصيد

```swift
smsService.checkBalance { result in
    switch result {
    case .success(let balance):
        print("Balance: \(balance)")
    case .failure(let error):
        print("Error: \(error.localizedDescription)")
    }
}
```

#### جلب قائمة المرسلين

```swift
smsService.getSenders { result in
    switch result {
    case .success(let senders):
        print("Senders: \(senders)")
    case .failure(let error):
        print("Error: \(error.localizedDescription)")
    }
}
```

## 🇬🇧 English

### Prerequisites
- Xcode 12.0 or later
- iOS 13.0 or later
- Swift 5.0 or later

### Installation & Setup
1. Add library using CocoaPods or Swift Package Manager
2. Get API Key & Secret from 4Jawaly website
3. Create service instance

### Usage
See examples below for library usage

#### Sending an SMS

```swift
smsService.sendSMS(
    message: "Hello from Swift!",
    numbers: ["966500000000"],
    sender: "SENDER"
) { result in
    switch result {
    case .success(let jobId):
        print("Message sent successfully! Job ID: \(jobId)")
    case .failure(let error):
        print("Error: \(error.localizedDescription)")
    }
}
```

#### Checking Balance

```swift
smsService.checkBalance { result in
    switch result {
    case .success(let balance):
        print("Balance: \(balance)")
    case .failure(let error):
        print("Error: \(error.localizedDescription)")
    }
}
```

#### Getting Senders List

```swift
smsService.getSenders { result in
    switch result {
    case .success(let senders):
        print("Senders: \(senders)")
    case .failure(let error):
        print("Error: \(error.localizedDescription)")
    }
}
```

## 🇵🇰 اردو

### پیش شرائط
- Xcode 12.0 یا اس سے نئی ورژن
- iOS 13.0 یا اس سے نئی ورژن
- Swift 5.0 یا اس سے نئی ورژن

### انسٹالیشن اور سیٹ اپ
1. CocoaPods یا Swift Package Manager کے ذریعے لائبریری شامل کریں
2. 4Jawaly ویب سائٹ سے API Key اور Secret حاصل کریں
3. سروس انسٹنس بنائیں

### استعمال
لائبریری کے استعمال کے لیے ذیل کی مثالیں دیکھیں

#### ایس ایم ایس بھیجنا

```swift
smsService.sendSMS(
    message: "Hello from Swift!",
    numbers: ["966500000000"],
    sender: "SENDER"
) { result in
    switch result {
    case .success(let jobId):
        print("Message sent successfully! Job ID: \(jobId)")
    case .failure(let error):
        print("Error: \(error.localizedDescription)")
    }
}
```

#### بیلنس چیک کرنا

```swift
smsService.checkBalance { result in
    switch result {
    case .success(let balance):
        print("Balance: \(balance)")
    case .failure(let error):
        print("Error: \(error.localizedDescription)")
    }
}
```

#### سندرز کی فہرست حاصل کرنا

```swift
smsService.getSenders { result in
    switch result {
    case .success(let senders):
        print("Senders: \(senders)")
    case .failure(let error):
        print("Error: \(error.localizedDescription)")
    }
}
```

## مثال كامل في UIViewController

```swift
class SMSViewController: UIViewController {
    private let smsService: JawalySMSService
    
    @IBOutlet weak var messageTextField: UITextField!
    @IBOutlet weak var phoneTextField: UITextField!
    @IBOutlet weak var senderTextField: UITextField!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        smsService = JawalySMSService(
            appID: "your_api_key",
            appSecret: "your_api_secret"
        )
    }
    
    @IBAction func sendButtonTapped(_ sender: UIButton) {
        guard let message = messageTextField.text,
              let phone = phoneTextField.text,
              let sender = senderTextField.text else {
            return
        }
        
        smsService.sendSMS(
            message: message,
            numbers: [phone],
            sender: sender
        ) { [weak self] result in
            DispatchQueue.main.async {
                switch result {
                case .success(let jobId):
                    self?.showAlert(title: "Success", message: "Message sent! Job ID: \(jobId)")
                case .failure(let error):
                    self?.showAlert(title: "Error", message: error.localizedDescription)
                }
            }
        }
    }
    
    private func showAlert(title: String, message: String) {
        let alert = UIAlertController(
            title: title,
            message: message,
            preferredStyle: .alert
        )
        alert.addAction(UIAlertAction(title: "OK", style: .default))
        present(alert, animated: true)
    }
}

## أفضل الممارسات

1. استخدم `Result` type للتعامل مع النجاح والفشل
2. قم بتنفيذ استدعاءات UI في الـ main thread
3. احمِ API Key و Secret الخاصين بك
4. تأكد من إضافة إذن الشبكة في Info.plist:

```xml
<key>NSAppTransportSecurity</key>
<dict>
    <key>NSAllowsArbitraryLoads</key>
    <true/>
</dict>
```

## معالجة الأخطاء

المكتبة تستخدم `Result` type القياسي في Swift:

```swift
public enum Result<Success, Failure: Error> {
    case success(Success)
    case failure(Failure)
}
```

## المزيد من المعلومات

- [توثيق 4Jawaly API](https://4jawaly.com/api-docs)
- [Swift Concurrency](https://docs.swift.org/swift-book/LanguageGuide/Concurrency.html)
- [الأسئلة الشائعة](../FAQ.md)
- [رخصة المشروع](../LICENSE)
