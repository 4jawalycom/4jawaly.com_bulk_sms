#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface JawalySMSService : NSObject

- (instancetype)initWithAppID:(NSString *)appID appSecret:(NSString *)appSecret;

- (void)sendSMSWithMessage:(NSString *)message
                  numbers:(NSArray<NSString *> *)numbers
                  sender:(NSString *)sender
              completion:(void (^)(NSString * _Nullable jobId, NSError * _Nullable error))completion;

- (void)checkBalanceWithCompletion:(void (^)(double balance, NSError * _Nullable error))completion;

- (void)getSendersWithCompletion:(void (^)(NSArray<NSString *> * _Nullable senders, NSError * _Nullable error))completion;

@end

NS_ASSUME_NONNULL_END
