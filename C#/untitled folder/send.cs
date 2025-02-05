using System;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

class Program {
    static void Main(string[] args) {
        string appId = "api key";
        string appSec = "api secret";
        string appHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{appId}:{appSec}"));
        
        var request = (HttpWebRequest)WebRequest.Create("https://api-sms.4jawaly.com/api/v1/account/area/sms/send");
        request.Method = "POST";
        request.Headers["Authorization"] = "Basic " + appHash;
        request.ContentType = "application/json";

        using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
            string json = "{\"messages\":[{\"text\":\"test\",\"numbers\":[\"96650000\"],\"sender\":\"test\"}]}";
            streamWriter.Write(json);
        }

        string responseContent;
        try {
            using (WebResponse response = request.GetResponse()) {
                using (Stream responseStream = response.GetResponseStream()) {
                    using (StreamReader streamReader = new StreamReader(responseStream)) {
                        responseContent = streamReader.ReadToEnd();
                    }
                }
            }
        } catch (WebException e) {
            using (WebResponse response = e.Response) {
                using (Stream responseStream = response.GetResponseStream()) {
                    using (StreamReader streamReader = new StreamReader(responseStream)) {
                        responseContent = streamReader.ReadToEnd();
                    }
                }
            }
        }

        JObject responseJson = JObject.Parse(responseContent);

        if (responseJson["code"].ToString() == "200") {
            if (responseJson["messages"][0].Type == JTokenType.Object && responseJson["messages"][0]["err_text"] != null) {
                Console.WriteLine(responseJson["messages"][0]["err_text"].ToString());
            } else {
                Console.WriteLine("تم الإرسال بنجاح");
                Console.WriteLine("job_id: " + responseJson["job_id"].ToString());
            }
        } else if (responseJson["code"].ToString() == "400") {
            Console.WriteLine("error 400");
            Console.WriteLine(responseJson["message"].ToString());
        } else {
            Console.WriteLine("error " + responseJson["code"].ToString());
        }
    }
}
