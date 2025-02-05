import Foundation

let appID = "Api Key"
let appSecret = "Api Secret"
let appHash = Data("\(appID):\(appSecret)".utf8).base64EncodedString()
let baseURL = "https://api-sms.4jawaly.com/api/v1/"

let query: [String: String] = [:] // Define the query parameters here if needed
let queryString = query.map { "\($0.key)=\($0.value)" }.joined(separator: "&")
let urlString = baseURL + "account/area/me/packages?" + queryString

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

    if response.statusCode != 200 {
        print("Error code: \(response.statusCode)")
        exit(0)
    }

    do {
        if let responseJSON = try JSONSerialization.jsonObject(with: data, options: []) as? [String: Any] {
            if let errorCode = responseJSON["code"] as? Int, errorCode == 200 {
                if let totalBalance = responseJSON["total_balance"] as? Double {
                    print("Your balance: \(totalBalance)")
                }
            } else {
                if let message = responseJSON["message"] as? String {
                    print(message)
                }
            }
        }
    } catch {
        print("Error parsing JSON: \(error.localizedDescription)")
    }
}

task.resume()
