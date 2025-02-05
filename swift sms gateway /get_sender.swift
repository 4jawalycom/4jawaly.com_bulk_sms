import Foundation

let appID = "Api Key"
let appSecret = "Api Secret"
let appHash = Data("\(appID):\(appSecret)".utf8).base64EncodedString()
let baseURL = "https://api-sms.4jawaly.com/api/v1/"

let query: [String: String] = [:] // Add query items as needed
let queryString = query.map { "\($0.key)=\($0.value)" }.joined(separator: "&")
let urlString = baseURL + "account/area/senders?" + queryString

guard let url = URL(string: urlString) else {
    print("Invalid URL")
    exit(1)
}

var request = URLRequest(url: url)
request.httpMethod = "GET"
request.setValue("application/json", forHTTPHeaderField: "Accept")
request.setValue("application/json", forHTTPHeaderField: "Content-Type")
request.setValue("Basic \(appHash)", forHTTPHeaderField: "Authorization")

let task = URLSession.shared.dataTask(with: request) { data, response, error in
    guard let data = data, let response = response as? HTTPURLResponse else {
        print("Error: \(error?.localizedDescription ?? "Unknown error")")
        return
    }

    do {
        if let responseJSON = try JSONSerialization.jsonObject(with: data, options: []) as? [String: Any] {
            let errorCode = responseJSON["code"] as? Int ?? -1
            print("Error code: \(errorCode)")

            if errorCode == 200 {
                if let items = responseJSON["items"] as? [[String: Any]] {
                    for item in items {
                        if let senderName = item["sender_name"] as? String {
                            print(senderName)
                        }
                    }
                }
            } else if response.statusCode == 400 {
                if let message = responseJSON["message"] as? String {
                    print(message)
                }
            } else {
                print(response.statusCode)
            }
        }
    } catch {
        print("Error parsing JSON: \(error.localizedDescription)")
    }
}

task.resume()
