# Objective-C Library for 4Jawaly SMS | مكتبة Objective-C لخدمة 4Jawaly SMS | 4Jawaly SMS کے لیے Objective-C لائبریری

## 🇸🇦 عربي

### المتطلبات الأساسية
- Xcode 11.0 أو أحدث
- iOS 11.0 أو أحدث

### التثبيت

#### CocoaPods

```ruby
pod 'JawalySMS-ObjC'
```

#### يدوياً

قم بنسخ الملفات التالية إلى مشروعك:
- `JawalySMSService.h`
- `JawalySMSService.m`

### الإعداد

1. احصل على API Key و Secret من موقع 4Jawaly:
   - قم بتسجيل الدخول إلى حسابك على [4Jawaly](https://4jawaly.com)
   - اذهب إلى البيانات الشخصية
   - اضغط على API Token
   - شاهد [الفيديو التوضيحي](https://youtu.be/oTB6hLbJXPU?si=Zr6-6HsjsKkyHUR6&t=468) للمزيد من التفاصيل

2. قم بإنشاء instance من JawalySMSService:

```objc
JawalySMSService *smsService = [[JawalySMSService alloc] 
    initWithAppID:@"your_api_key" 
        appSecret:@"your_api_secret"];
```

### الاستخدام

#### إرسال رسالة SMS

```objc
NSArray *numbers = @[@"966500000000"];
[smsService sendSMSWithMessage:@"Hello from Objective-C!"
                      numbers:numbers
                      sender:@"SENDER"
                  completion:^(NSString *jobId, NSError *error) {
    if (error) {
        NSLog(@"Error: %@", error.localizedDescription);
    } else {
        NSLog(@"Message sent successfully! Job ID: %@", jobId);
    }
}];
```

#### التحقق من الرصيد

```objc
[smsService checkBalanceWithCompletion:^(double balance, NSError *error) {
    if (error) {
        NSLog(@"Error: %@", error.localizedDescription);
    } else {
        NSLog(@"Balance: %f", balance);
    }
}];
```

#### جلب قائمة المرسلين

```objc
[smsService getSendersWithCompletion:^(NSArray<NSString *> *senders, NSError *error) {
    if (error) {
        NSLog(@"Error: %@", error.localizedDescription);
    } else {
        NSLog(@"Senders: %@", senders);
    }
}];
```

## 🇬🇧 English

### Prerequisites
- Xcode 11.0 or later
- iOS 11.0 or later

### Installation & Setup
1. Add library using CocoaPods
2. Get API Key & Secret from 4Jawaly website
3. Create service instance

### Usage
See examples below for library usage

#### Sending SMS

```objc
NSArray *numbers = @[@"966500000000"];
[smsService sendSMSWithMessage:@"Hello from Objective-C!"
                      numbers:numbers
                      sender:@"SENDER"
                  completion:^(NSString *jobId, NSError *error) {
    if (error) {
        NSLog(@"Error: %@", error.localizedDescription);
    } else {
        NSLog(@"Message sent successfully! Job ID: %@", jobId);
    }
}];
```

#### Checking Balance

```objc
[smsService checkBalanceWithCompletion:^(double balance, NSError *error) {
    if (error) {
        NSLog(@"Error: %@", error.localizedDescription);
    } else {
        NSLog(@"Balance: %f", balance);
    }
}];
```

#### Getting Senders List

```objc
[smsService getSendersWithCompletion:^(NSArray<NSString *> *senders, NSError *error) {
    if (error) {
        NSLog(@"Error: %@", error.localizedDescription);
    } else {
        NSLog(@"Senders: %@", senders);
    }
}];
```

## 🇵🇰 اردو

### پیش شرائط
- Xcode 11.0 یا اس سے نئی ورژن
- iOS 11.0 یا اس سے نئی ورژن

### انسٹالیشن اور سیٹ اپ
1. CocoaPods کے ذریعے لائبریری شامل کریں
2. 4Jawaly ویب سائٹ سے API Key اور Secret حاصل کریں
3. سروس انسٹنس بنائیں

### استعمال
لائبریری کے استعمال کے لیے ذیل کی مثالیں دیکھیں

#### ایس ایم ایس بھیجنا

```objc
NSArray *numbers = @[@"966500000000"];
[smsService sendSMSWithMessage:@"Hello from Objective-C!"
                      numbers:numbers
                      sender:@"SENDER"
                  completion:^(NSString *jobId, NSError *error) {
    if (error) {
        NSLog(@"Error: %@", error.localizedDescription);
    } else {
        NSLog(@"Message sent successfully! Job ID: %@", jobId);
    }
}];
```

#### بیلنس چیک کرنا

```objc
[smsService checkBalanceWithCompletion:^(double balance, NSError *error) {
    if (error) {
        NSLog(@"Error: %@", error.localizedDescription);
    } else {
        NSLog(@"Balance: %f", balance);
    }
}];
```

#### بھیجݨ والوں کی فہرست حاصل کرنا

```objc
[smsService getSendersWithCompletion:^(NSArray<NSString *> *senders, NSError *error) {
    if (error) {
        NSLog(@"Error: %@", error.localizedDescription);
    } else {
        NSLog(@"Senders: %@", senders);
    }
}];
```

## مثال كامل في UIViewController

```objc
@interface SMSViewController : UIViewController

@property (weak, nonatomic) IBOutlet UITextField *messageTextField;
@property (weak, nonatomic) IBOutlet UITextField *phoneTextField;
@property (weak, nonatomic) IBOutlet UITextField *senderTextField;
@property (strong, nonatomic) JawalySMSService *smsService;

@end

@implementation SMSViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    self.smsService = [[JawalySMSService alloc] 
        initWithAppID:@"your_api_key" 
            appSecret:@"your_api_secret"];
}

- (IBAction)sendButtonTapped:(UIButton *)sender {
    NSString *message = self.messageTextField.text;
    NSString *phone = self.phoneTextField.text;
    NSString *senderName = self.senderTextField.text;
    
    if (!message.length || !phone.length || !senderName.length) {
        return;
    }
    
    [self.smsService sendSMSWithMessage:message
                              numbers:@[phone]
                              sender:senderName
                          completion:^(NSString *jobId, NSError *error) {
        dispatch_async(dispatch_get_main_queue(), ^{
            if (error) {
                [self showAlertWithTitle:@"Error" 
                               message:error.localizedDescription];
            } else {
                [self showAlertWithTitle:@"Success" 
                               message:[NSString stringWithFormat:@"Message sent! Job ID: %@", jobId]];
            }
        });
    }];
}

- (void)showAlertWithTitle:(NSString *)title message:(NSString *)message {
    UIAlertController *alert = [UIAlertController 
        alertControllerWithTitle:title
                       message:message
                preferredStyle:UIAlertControllerStyleAlert];
                       
    [alert addAction:[UIAlertAction 
        actionWithTitle:@"OK" 
                style:UIAlertActionStyleDefault 
              handler:nil]];
              
    [self presentViewController:alert animated:YES completion:nil];
}

@end
```

## أفضل الممارسات

1. استخدم blocks للتعامل مع العمليات غير المتزامنة
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

المكتبة تستخدم `NSError` للتعامل مع الأخطاء. تأكد دائماً من فحص الـ error parameter في blocks الخاصة بالإكمال.

## المزيد من المعلومات

- [توثيق 4Jawaly API](https://4jawaly.com/api-docs)
- [Objective-C Blocks Programming](https://developer.apple.com/library/archive/documentation/Cocoa/Conceptual/ProgrammingWithObjectiveC/WorkingwithBlocks/WorkingwithBlocks.html)
- [الأسئلة الشائعة](../FAQ.md)
- [رخصة المشروع](../LICENSE)
