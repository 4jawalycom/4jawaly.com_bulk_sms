#import "JawalySMSService.h"

@interface JawalySMSService ()
@property (nonatomic, strong) NSString *baseURL;
@property (nonatomic, strong) NSString *appID;
@property (nonatomic, strong) NSString *appSecret;
@property (nonatomic, strong) NSURLSession *session;
@end

@implementation JawalySMSService

- (instancetype)initWithAppID:(NSString *)appID appSecret:(NSString *)appSecret {
    self = [super init];
    if (self) {
        _baseURL = @"https://api-sms.4jawaly.com/api/v1/";
        _appID = appID;
        _appSecret = appSecret;
        _session = [NSURLSession sharedSession];
    }
    return self;
}

- (NSString *)authHeader {
    NSString *credentials = [NSString stringWithFormat:@"%@:%@", self.appID, self.appSecret];
    NSData *credentialsData = [credentials dataUsingEncoding:NSUTF8StringEncoding];
    return [NSString stringWithFormat:@"Basic %@", [credentialsData base64EncodedStringWithOptions:0]];
}

- (void)sendSMSWithMessage:(NSString *)message
                  numbers:(NSArray<NSString *> *)numbers
                  sender:(NSString *)sender
              completion:(void (^)(NSString * _Nullable, NSError * _Nullable))completion {
    
    NSString *endpoint = [self.baseURL stringByAppendingString:@"account/area/sms/send"];
    NSURL *url = [NSURL URLWithString:endpoint];
    
    if (!url) {
        NSError *error = [NSError errorWithDomain:@"Invalid URL" code:-1 userInfo:nil];
        completion(nil, error);
        return;
    }
    
    NSDictionary *messageData = @{
        @"messages": @[
            @{
                @"text": message,
                @"numbers": numbers,
                @"sender": sender
            }
        ]
    };
    
    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    request.HTTPMethod = @"POST";
    [request setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
    [request setValue:@"application/json" forHTTPHeaderField:@"Accept"];
    [request setValue:[self authHeader] forHTTPHeaderField:@"Authorization"];
    request.HTTPBody = [NSJSONSerialization dataWithJSONObject:messageData options:0 error:nil];
    
    NSURLSessionDataTask *task = [self.session dataTaskWithRequest:request completionHandler:^(NSData * _Nullable data, NSURLResponse * _Nullable response, NSError * _Nullable error) {
        if (error) {
            completion(nil, error);
            return;
        }
        
        if (!data) {
            NSError *noDataError = [NSError errorWithDomain:@"No data received" code:-1 userInfo:nil];
            completion(nil, noDataError);
            return;
        }
        
        NSError *jsonError;
        NSDictionary *json = [NSJSONSerialization JSONObjectWithData:data options:0 error:&jsonError];
        
        if (jsonError) {
            completion(nil, jsonError);
            return;
        }
        
        NSString *jobId = json[@"job_id"];
        if (jobId) {
            completion(jobId, nil);
        } else {
            NSArray *messages = json[@"messages"];
            if (messages.count > 0) {
                NSString *errText = messages[0][@"err_text"];
                if (errText) {
                    NSError *messageError = [NSError errorWithDomain:errText code:-1 userInfo:nil];
                    completion(nil, messageError);
                    return;
                }
            }
            completion(nil, [NSError errorWithDomain:@"Unknown error" code:-1 userInfo:nil]);
        }
    }];
    
    [task resume];
}

- (void)checkBalanceWithCompletion:(void (^)(double, NSError * _Nullable))completion {
    NSString *endpoint = [self.baseURL stringByAppendingString:@"account/area/me/packages"];
    NSURL *url = [NSURL URLWithString:endpoint];
    
    if (!url) {
        NSError *error = [NSError errorWithDomain:@"Invalid URL" code:-1 userInfo:nil];
        completion(0, error);
        return;
    }
    
    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    request.HTTPMethod = @"GET";
    [request setValue:@"application/json" forHTTPHeaderField:@"Accept"];
    [request setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
    [request setValue:[self authHeader] forHTTPHeaderField:@"Authorization"];
    
    NSURLSessionDataTask *task = [self.session dataTaskWithRequest:request completionHandler:^(NSData * _Nullable data, NSURLResponse * _Nullable response, NSError * _Nullable error) {
        if (error) {
            completion(0, error);
            return;
        }
        
        if (!data) {
            NSError *noDataError = [NSError errorWithDomain:@"No data received" code:-1 userInfo:nil];
            completion(0, noDataError);
            return;
        }
        
        NSError *jsonError;
        NSDictionary *json = [NSJSONSerialization JSONObjectWithData:data options:0 error:&jsonError];
        
        if (jsonError) {
            completion(0, jsonError);
            return;
        }
        
        NSNumber *balance = json[@"total_balance"];
        if (balance) {
            completion(balance.doubleValue, nil);
        } else {
            NSError *formatError = [NSError errorWithDomain:@"Invalid response format" code:-1 userInfo:nil];
            completion(0, formatError);
        }
    }];
    
    [task resume];
}

- (void)getSendersWithCompletion:(void (^)(NSArray<NSString *> * _Nullable, NSError * _Nullable))completion {
    NSString *endpoint = [self.baseURL stringByAppendingString:@"account/area/senders"];
    NSURL *url = [NSURL URLWithString:endpoint];
    
    if (!url) {
        NSError *error = [NSError errorWithDomain:@"Invalid URL" code:-1 userInfo:nil];
        completion(nil, error);
        return;
    }
    
    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    request.HTTPMethod = @"GET";
    [request setValue:@"application/json" forHTTPHeaderField:@"Accept"];
    [request setValue:@"application/json" forHTTPHeaderField:@"Content-Type"];
    [request setValue:[self authHeader] forHTTPHeaderField:@"Authorization"];
    
    NSURLSessionDataTask *task = [self.session dataTaskWithRequest:request completionHandler:^(NSData * _Nullable data, NSURLResponse * _Nullable response, NSError * _Nullable error) {
        if (error) {
            completion(nil, error);
            return;
        }
        
        if (!data) {
            NSError *noDataError = [NSError errorWithDomain:@"No data received" code:-1 userInfo:nil];
            completion(nil, noDataError);
            return;
        }
        
        NSError *jsonError;
        NSDictionary *json = [NSJSONSerialization JSONObjectWithData:data options:0 error:&jsonError];
        
        if (jsonError) {
            completion(nil, jsonError);
            return;
        }
        
        NSArray *items = json[@"items"];
        if (items) {
            NSMutableArray *senders = [NSMutableArray array];
            for (NSDictionary *item in items) {
                NSString *senderName = item[@"sender_name"];
                if (senderName) {
                    [senders addObject:senderName];
                }
            }
            completion(senders, nil);
        } else {
            NSError *formatError = [NSError errorWithDomain:@"Invalid response format" code:-1 userInfo:nil];
            completion(nil, formatError);
        }
    }];
    
    [task resume];
}

@end
