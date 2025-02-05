import Foundation
import NIO
import AsyncHTTPClient

// You can get the API key and secret by logging in to the 4jawaly website,
// going to your personal data, and clicking on the API token.

let appID = "api key"
let appSecret = "api secret"

let appHash = Data("\(appID):\(appSecret)".utf8).base64EncodedString()

let messages: [String: Any] = [
    "messages": [
        [
            "text": "test",
            "numbers": ["9665000000"],
            "sender": "4jawaly"
        ]
    ]
]

let url = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send"
let jsonData = try JSONSerialization.data(withJSONObject: messages, options: [])

let httpClient = HTTPClient(eventLoopGroupProvider: .createNew)

let request = try HTTPClient.Request(
    url: url,
    method: .POST,
    headers: [
        "Accept": "application/json",
        "Content-Type": "application/json",
        "Authorization": "Basic \(appHash)"
    ],
    body: .data(jsonData)
)

httpClient.execute(request: request).whenComplete { result in
    defer { httpClient.shutdown() }
    
    switch result {
    case .success(let response):
        let statusCode = response.status.code
        
        do {
            let responseJSON = try JSONSerialization.jsonObject(with: response.body!, options: []) as? [String: Any]
            
            if statusCode == 200,
               let messages = responseJSON?["messages"] as? [[String: Any]],
               let firstMessage = messages.first {
                if let errText = firstMessage["err_text"] as? String {
                    print(errText)
                } else if let jobId = responseJSON?["job_id"] as? String {
                    print("تم الارسال بنجاح  " + " job id: \(jobId)")
                }
            } else if statusCode == 400,
                      let message = responseJSON?["message"] as? String {
                print(message)
            } else if statusCode == 422 {
                print("نص الرسالة فارغ")
            } else {
                print("محظور بواسطة كلاودفلير. Status code: \(statusCode)")
           
