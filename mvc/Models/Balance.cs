namespace mvc.Models.Balance
{
    public class Item
    {
        public int id { get; set; }
        public int package_points { get; set; }
        public int current_points { get; set; }
        public DateTime expire_at { get; set; }
        public int account_id { get; set; }
        public string system_uuid { get; set; }
        public int is_open { get; set; }
        public int? ticket_id { get; set; }
        public int? payment_method_id { get; set; }
        public int? bank_id { get; set; }
        public string price { get; set; }
        public string tax { get; set; }
        public string fees { get; set; }
        public string total { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<Network> networks { get; set; }
        public int used_balance { get; set; }
        public bool is_completed { get; set; }
        public Package package { get; set; }
    }

    public class Network
    {
        public int id { get; set; }
        public string image { get; set; }
        public string image_path { get; set; }
        public List<NetworkTag> network_tags { get; set; }
    }

    public class NetworkTag
    {
        public int id { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public int network_id { get; set; }
        public string image_path { get; set; }
    }

    public class Package
    {
        public int id { get; set; }
        public string title_ar { get; set; }
    }

    public class Balance
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<Item> items { get; set; }
        public int total_balance { get; set; }
    }
}
