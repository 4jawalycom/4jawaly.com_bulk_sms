namespace mvc.Models
{
    public class SmSprovider
    {
        public string app_key { get; set; }
        public string app_secret { get; set; }
    }
    public class SendData
    {
        public List<message> messages { get; set; }
        public Globals globals { get; set; }
    }
    public class _4jawalyRoot
    {
        public string job_id { get; set; }
        public List<message> messages { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }

    public class message
    {
        public string text { get; set; }
        public List<string> numbers { get; set; }

    }
    public class Globals
    {
        public string number_iso { get; set; }
        public string sender { get; set; }

    }
    public class Root
    {
        public string text { get; set; }
        public string numbers { get; set; }
        public string sender { get; set; }
        // public message messages { get; set; }
        //public Globals Global { get; set; }
    }

}
