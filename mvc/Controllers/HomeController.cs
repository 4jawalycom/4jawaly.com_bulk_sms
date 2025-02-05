using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Models.Balance;
using mvc.Models.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net;
using System.Text;
using System.Text.Json;


namespace mvc.Controllers
{
    public class HomeController : Controller
    {
        private static string UserAgent = "XXXXXXXXX";//put your useragent here

        private readonly ILogger<HomeController> _logger;
        private readonly HttpContext _httpContext;
        
        public HomeController(ILogger<HomeController> logger , IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public IActionResult Setting()
        {
            string serializedList = HttpContext.Session.GetString("ListUser");
            List<string> myList = new List<string>();
            if (serializedList != null)
            {
               myList = JsonConvert.DeserializeObject<List<string>>(serializedList);
            }
 
                
            // List<String> list= new List<String>();
            ViewData["ListUser"] = myList;
            ViewData["Status"] = TempData["Status"];
            TempData["ListUserSend"] = TempData["ListUser"];

            return View();
        }
        public IActionResult send()
        {
            string serializedList = HttpContext.Session.GetString("ListUser");
            List<string> myList = JsonConvert.DeserializeObject<List<string>>(serializedList);

            ViewData["ListUserSend"] = myList;
            ViewData["Status"] = TempData["Status"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> SendSms(Root root)
        {
            
            List<string> numberList = root.numbers.Split(',').ToList();
            string appKey = HttpContext.Session.GetString("app_key"); // from settings
            string appSecret = HttpContext.Session.GetString("app_secret"); // from settings
            
            List<message> messages = new List<message>();
                message messageSend = new message();
                messageSend.text = root.text;
                messageSend.numbers = numberList;
                messages.Add(messageSend);
           

                var message = new message
                {
                    text = root.text,
                    numbers = numberList
                };

                var sendData = new SendData
                {
                    messages = messages,
                    globals = new Globals
                    {
                        number_iso = "SA",
                        sender = root.sender
                    }
                };

                var url = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send";
                var credentials = appKey + ":" + appSecret;

                using var httpClient = new HttpClient();

                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(sendData), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials)));
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);

                using var response = await httpClient.PostAsync(url, content);
            var resultJson = await response.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<_4jawalyRoot>(resultJson);

            if (response.IsSuccessStatusCode)
                {
                    
                TempData["Status"] = result.code + " " + result.message ;
                return RedirectToAction("send");
            }
                else
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                TempData["Status"] = result.code + " " + result.message ;
                return RedirectToAction("send");
                throw new Exception(errorJson.ToString());

                }
           
        }
        public   IActionResult Connect(_4jawalyProvider SmSprovider)
        {
            try
            {
               
                var url = "https://api-sms.4jawaly.com/api/v1/account/area/senders";

                string credentials = SmSprovider.app_key + ":" + SmSprovider.app_secret;
                List<string> Username = new List<string>();

                var httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = "GET";
                httpRequest.Accept = "application/json";
                httpRequest.UserAgent = UserAgent;
                httpRequest.ContentType = "application/json";

                httpRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {

                    using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string responseData = streamReader.ReadToEnd();
                        Sender Response = Newtonsoft.Json.JsonConvert.DeserializeObject<Sender>(responseData);
                  
                        foreach (var item in Response.items.data)
                        {
                            Username.Add(item.sender_name);
                           
                        }
                        
                        TempData["ListUser"] = Username;

                        TempData["Status"] = "تم الاتصال";
                        // Do something with the response data
                        HttpContext.Session.SetString("app_key", SmSprovider.app_key);
                        HttpContext.Session.SetString("app_secret", SmSprovider.app_secret);
                        string serializedList = JsonConvert.SerializeObject(Username);
                        HttpContext.Session.SetString("ListUser", serializedList);

                    }
                    return RedirectToAction("Setting");

                }
                else
                {
                    TempData["Status"] = "هناك خطا فى بيانات";
                    return RedirectToAction("Setting");
                  
                }

            }
            catch (Exception ex)
            {
                TempData["Status"] = "هناك خطا حاول مره اخره";
                return RedirectToAction("Setting");
            }

        } 
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}