import Foundation

class JawalySMSService {
    private let baseURL = "https://api-sms.4jawaly.com/api/v1/"
    private let appID: String
    private let appSecret: String
    private let session: URLSession
    
    init(appID: String, appSecret: String) {
        self.appID = appID
        self.appSecret = appSecret
        self.session = URLSession.shared
    }
    
    private var authHeader: String {
        let credentials = "\(appID):\(appSecret)"
        let encodedCredentials = Data(credentials.utf8).base64EncodedString()
        return "Basic \(encodedCredentials)"
    }
    
    func sendSMS(message: String, numbers: [String], sender: String, completion: @escaping (Result<String, Error>) -> Void) {
        let endpoint = baseURL + "account/area/sms/send"
        guard let url = URL(string: endpoint) else {
            completion(.failure(NSError(domain: "Invalid URL", code: -1)))
            return
        }
        
        let messageData: [String: Any] = [
            "messages": [
                [
                    "text": message,
                    "numbers": numbers,
                    "sender": sender
                ]
            ]
        ]
        
        var request = URLRequest(url: url)
        request.httpMethod = "POST"
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue("application/json", forHTTPHeaderField: "Accept")
        request.setValue(authHeader, forHTTPHeaderField: "Authorization")
        request.httpBody = try? JSONSerialization.data(withJSONObject: messageData)
        
        let task = session.dataTask(with: request) { data, response, error in
            if let error = error {
                completion(.failure(error))
                return
            }
            
            guard let data = data else {
                completion(.failure(NSError(domain: "No data received", code: -1)))
                return
            }
            
            do {
                if let json = try JSONSerialization.jsonObject(with: data) as? [String: Any] {
                    if let jobId = json["job_id"] as? String {
                        completion(.success(jobId))
                    } else if let messages = json["messages"] as? [[String: Any]],
                              let firstMessage = messages.first,
                              let errText = firstMessage["err_text"] as? String {
                        completion(.failure(NSError(domain: errText, code: -1)))
                    }
                }
            } catch {
                completion(.failure(error))
            }
        }
        task.resume()
    }
    
    func checkBalance(completion: @escaping (Result<Double, Error>) -> Void) {
        let endpoint = baseURL + "account/area/me/packages"
        guard let url = URL(string: endpoint) else {
            completion(.failure(NSError(domain: "Invalid URL", code: -1)))
            return
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "GET"
        request.setValue("application/json", forHTTPHeaderField: "Accept")
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue(authHeader, forHTTPHeaderField: "Authorization")
        
        let task = session.dataTask(with: request) { data, response, error in
            if let error = error {
                completion(.failure(error))
                return
            }
            
            guard let data = data else {
                completion(.failure(NSError(domain: "No data received", code: -1)))
                return
            }
            
            do {
                if let json = try JSONSerialization.jsonObject(with: data) as? [String: Any],
                   let balance = json["total_balance"] as? Double {
                    completion(.success(balance))
                } else {
                    completion(.failure(NSError(domain: "Invalid response format", code: -1)))
                }
            } catch {
                completion(.failure(error))
            }
        }
        task.resume()
    }
    
    func getSenders(completion: @escaping (Result<[String], Error>) -> Void) {
        let endpoint = baseURL + "account/area/senders"
        guard let url = URL(string: endpoint) else {
            completion(.failure(NSError(domain: "Invalid URL", code: -1)))
            return
        }
        
        var request = URLRequest(url: url)
        request.httpMethod = "GET"
        request.setValue("application/json", forHTTPHeaderField: "Accept")
        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
        request.setValue(authHeader, forHTTPHeaderField: "Authorization")
        
        let task = session.dataTask(with: request) { data, response, error in
            if let error = error {
                completion(.failure(error))
                return
            }
            
            guard let data = data else {
                completion(.failure(NSError(domain: "No data received", code: -1)))
                return
            }
            
            do {
                if let json = try JSONSerialization.jsonObject(with: data) as? [String: Any],
                   let items = json["items"] as? [[String: Any]] {
                    let senders = items.compactMap { $0["sender_name"] as? String }
                    completion(.success(senders))
                } else {
                    completion(.failure(NSError(domain: "Invalid response format", code: -1)))
                }
            } catch {
                completion(.failure(error))
            }
        }
        task.resume()
    }
}
