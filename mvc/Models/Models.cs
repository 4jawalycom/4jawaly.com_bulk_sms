namespace mvc.Models.Models
{
    public class _4jawalyProvider
    {
        public string app_key { get; set; }
        public string app_secret { get; set; }
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
    public class SendResult
    {
        public bool Sent { get; set; }
        public string Message { get; set; }
    }
    public class SendData
    {
        public List<message> messages { get; set; }
        public Globals globals { get; set; }
    }
    
    public class Account
    {
        public int id { get; set; }
        public int account_id { get; set; }
        public int user_id { get; set; }
        public int currency_id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string name_hash { get; set; }
        public string username_hash { get; set; }
        public string email_hash { get; set; }
        public string mobile_hash { get; set; }
        public object main_account_id { get; set; }
        public int status { get; set; }
        public int is_marketer { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string queue_number { get; set; }
    }

    public class Message
    {
        public int inserted_numbers { get; set; }
        public Message2 message { get; set; }
    }

    public class Message2
    {
        public int account_id { get; set; }
        public string job_id { get; set; }
        public string text { get; set; }
        public int sender_id { get; set; }
        public string sender_name { get; set; }
        public string encoding { get; set; }
        public int length { get; set; }
        public int per_message { get; set; }
        public int remaining { get; set; }
        public int messages { get; set; }
        public object send_at { get; set; }
        public object send_at_zone { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public int id { get; set; }
        public Account account { get; set; }
    }

    public class _4jawalyRoot
    {
        public string job_id { get; set; }
        public List<Message> messages { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string sender_name { get; set; }
        public int is_ad { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
    }

    public class Items
    {
        public int current_page { get; set; }
        public List<Datum> data { get; set; }
        public string first_page_url { get; set; }
        public int from { get; set; }
        public int last_page { get; set; }
        public string last_page_url { get; set; }
        public List<Link> links { get; set; }
        public object next_page_url { get; set; }
        public string path { get; set; }
        public int per_page { get; set; }
        public object prev_page_url { get; set; }
        public int to { get; set; }
        public int total { get; set; }
    }

    public class Link
    {
        public string url { get; set; }
        public string label { get; set; }
        public bool active { get; set; }
    }

    public class Sender
    {
        public int code { get; set; }
        public string message { get; set; }
        public Items items { get; set; }
    }
    public class _4JawalyUser
    {
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string country_iso { get; set; }
        public string company_name { get; set; }
        public string is_marketer { get; set; }
        public string account_type { get; set; }
    }

    
    public class Item
    {
        public string email { get; set; }
        public string mobile { get; set; }
        public int by_account_id { get; set; }
        public int account_id { get; set; }
        public string country_iso { get; set; }
        public string name { get; set; }
        public string account_type { get; set; }
        public string company_name { get; set; }
        public int company_field_id { get; set; }
        public int is_developer { get; set; }
        public object site_user_id { get; set; }
        public object site_username { get; set; }
        public object site_groupid { get; set; }
        public int site_createdby { get; set; }
        public string is_marketer { get; set; }
        public int currency_id { get; set; }
        public string odoo_id { get; set; }
        public int status { get; set; }
        public string source { get; set; }
        public bool is_transfer { get; set; }
        public object fb_id { get; set; }
        public object google_id { get; set; }
        public object zoho_id { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public int id { get; set; }
        public string id_number { get; set; }
        public bool is_first_login { get; set; }
        public bool is_first_login_after_transfer { get; set; }
    }

    public class AddUserRepons
    {
        public int code { get; set; }
        public string message { get; set; }
        public Item item { get; set; }
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
