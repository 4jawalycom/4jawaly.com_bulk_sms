using API_Example.Models;
using API_Example.Models.Balance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace API_Example
{
    public static class _4jawaly
    {
        private static string UserAgent = "XXXXXXXXX";//put your useragent here

        public static SendResult Send4jawaly(string App_key, string App_secret, string SenderName, message messages)
        {


            var senddata = new SendData();


            senddata.messages = messages;
            senddata.sender = SenderName;
            //senddata.globals = new Globals()
            //{
            //    number_iso = "SA",
            //    sender = SenderName
            //};


          
            var url = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send";

            string credentials =App_key + ":" + App_secret;

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.Accept = "application/json";
            httpRequest.UserAgent = UserAgent;
            httpRequest.ContentType = "application/json";
           
            httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

            var serializer = new JavaScriptSerializer();

            var data = serializer.Serialize(senddata);

            try
            {
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(data);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                string result = "";
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }

               

                _4jawalyRoot myDeserializedClass = JsonConvert.DeserializeObject<_4jawalyRoot>(result);
                return new SendResult() { Sent = true, Message = result };
            }
            catch (Exception ex)
            {
             
                return new SendResult() { Sent = false, Message = ex.Message };
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
        public static Balance GetBalance4jawaly(string App_key, int App_secret)
        {
            try
            {



              
                var url = "https://api-sms.4jawaly.com/api/v1/account/area/me/packages?is_active=1&order_by=id&order_by_type=desc&page=1&page_size=10&return_collection=1";

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

               

                return JsonConvert.DeserializeObject<Balance>(result);
               
            }
            catch (Exception ex)
            {
               
                return null;
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
