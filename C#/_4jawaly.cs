using API_Example.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace API_Example
{
    public static class _4jawaly
    {
        private static string UserAgent = "XXXXXXXXX";//put your useragent here

        public static string Send4jawaly(string App_key, string App_secret, string SenderName, string Mobiles, string message)
        {
            if (App_key == null)
                return "key required";
            if (App_secret == null)
                return "secret required";
            if (SenderName == null)
                return "Sender required";
            if (Mobiles == null)
                return "Mobile number required";
            if (message == null)
                return "Message required";
            try
            {
                List<message> messages = new List<message>();
                List<string> mobilesList = Mobiles.Split(',').ToList();
                messages.Add(new message()
                {
                    text = message,
                    numbers = mobilesList
                });


                var senddata = new SendData();


                senddata.messages = messages;
                senddata.globals = new Globals()
                {
                    number_iso = "SA",
                    sender = SenderName
                };


                var url = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send";

                string credentials = App_key + ":" + App_secret;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";

                httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

                var serializer = new JavaScriptSerializer();

                var data = serializer.Serialize(senddata);

                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                string result = "";

                if ((int)httpResponse.StatusCode == 200)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }

                    _4jawalyRoot myDeserializedClass = JsonConvert.DeserializeObject<_4jawalyRoot>(result);
                    if (myDeserializedClass.messages.FirstOrDefault().err_text == null)
                        return "تم الإرسال بنجاح" + "  job_id: " + myDeserializedClass.job_id + "  Code: " + myDeserializedClass.code + "  Message: " + myDeserializedClass.message;
                    else
                        return "لم يتم الارسال" + "  Message: " + myDeserializedClass.messages.First().err_text;

                }
                else if ((int)httpResponse.StatusCode == 400)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }

                    _4jawalyRoot myDeserializedClass = JsonConvert.DeserializeObject<_4jawalyRoot>(result);

                    return "Error 400 " + "  Message: " + myDeserializedClass.message;

                }
                else if ((int)httpResponse.StatusCode == 403)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }

                    _4jawalyRoot myDeserializedClass = JsonConvert.DeserializeObject<_4jawalyRoot>(result);
                    return "Error 403 محضور Firewall" + "  Message: " + myDeserializedClass.message;


                }

                else if (httpResponse.StatusCode == HttpStatusCode.BadRequest || httpResponse.StatusCode == HttpStatusCode.InternalServerError)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    var x = JsonConvert.DeserializeObject<Response>(result);

                    return "Code:" + x.code + ",Message:" + x.message;
                }


                else
                {

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    var x = JsonConvert.DeserializeObject<_4jawalyRoot>(result);
                    if (x.code == 0)
                        return "Error 0 " + "  Message: " + "No text in message";
                    else if (x.messages.FirstOrDefault().err_text == null)
                        return " Error Code: " + x.code + "  Message: " + x.message;
                    else
                        return " HTTP_Code: " + (int)httpResponse.StatusCode + " Error Code: " + x.code + "  Message: " + x.message + "Messages: " + x.messages.First().err_text;

                }
            }

            catch (WebException ex)
            {
                var errorResponse = (HttpWebResponse)ex.Response;
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    var errorText = reader.ReadToEnd();
                    // Handle error message
                    var Response = Newtonsoft.Json.JsonConvert.DeserializeObject<_4jawalyRoot>(errorText);
                    return "Error code:" + Response.code + " Message: " + Response.message;
                }
            }
        }

        public static Sender GetSenders4jawaly(string App_key, string App_secret)
        {
            try
            {

                var url = "https://api-sms.4jawaly.com/api/v1/account/area/senders";

                string credentials = App_key + ":" + App_secret;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";
                httpRequest.Accept = "application/json";
                httpRequest.UserAgent = UserAgent;
                httpRequest.ContentType = "application/json";

                httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));




                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                string result = "";
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }



                return JsonConvert.DeserializeObject<Sender>(result);

            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public static string GetBalance(string App_key, string App_secret)
        {
            if (App_key == null)
                return "key required";
            if (App_secret == null)
                return "secret required";

            try
            {

                var url = "https://api-sms.4jawaly.com/api/v1/account/area/me/packages?is_active=1&order_by=id&order_by_type=desc&page=1&page_size=10&return_collection=1";

                string credentials = App_key + ":" + App_secret;
                List<string> Username = new List<string>();

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";
                httpRequest.Accept = "application/json";
                //httpRequest.UserAgent = UserAgent;
                httpRequest.ContentType = "application/json";

                httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                if ((int)httpResponse.StatusCode == 200)
                {

                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string responseData = streamReader.ReadToEnd();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Balance>(responseData);

                        return "Balance: " + result.total_balance + " Code: " + result.code + "  Message: " + result.message;


                    }
                }
                else if ((int)httpResponse.StatusCode == 400)
                {

                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string responseData = streamReader.ReadToEnd();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<_4jawalyRoot>(responseData);

                        return "Error 400 " + " Code: " + result.code + "  Message: " + result.message;

                    }
                }
                else if ((int)httpResponse.StatusCode == 403)
                {

                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string responseData = streamReader.ReadToEnd();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<_4jawalyRoot>(responseData);

                        return "Error 400 " + " Code: " + result.code + "  Message: " + result.message;


                    }
                }
                else
                {
                    return " Error Code: " + (int)httpResponse.StatusCode + "  Message: error in data";


                }

            }
            catch (WebException ex)
            {
                if (App_key == null)
                    return "key required";
                if (App_secret == null)
                    return "secret required";
                //else
                //    return "Try again";


                var errorResponse = (HttpWebResponse)ex.Response;
                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                {
                    var errorText = reader.ReadToEnd();
                    // Handle error message
                    var Response = Newtonsoft.Json.JsonConvert.DeserializeObject<_4jawalyRoot>(errorText);
                    return "Error code:" + Response.code + " Message: " + Response.message;
                }

            }
        }

            public static bool Check4jawaly(string App_key, string App_secret)
        {
            try
            {

                var url = "https://api-sms.4jawaly.com/api/v1/account/area/senders";

                string credentials = App_key + ":" + App_secret;

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";
                httpRequest.Accept = "application/json";
                httpRequest.UserAgent = UserAgent;
                httpRequest.ContentType = "application/json";

                httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));


                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {

                return false;
            }
        }

       
    }
}
