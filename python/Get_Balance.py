import requests
import json
import base64

app_id = "API_KEY"      # Replace with your actual API Key
app_sec = "API_SECRET"  # Replace with your actual API Secret
app_hash = base64.b64encode(f"{app_id}:{app_sec}".encode()).decode()
base_url = "https://api-sms.4jawaly.com/api/v1/"

# Query parameters
query = {
    "is_active": 1,               # get active only
    "order_by": "id",             # or package_points, current_points, expire_at
    "order_by_type": "desc",      # desc or asc
    "page": 1,
    "page_size": 10,
    "return_collection": 1        # get all collection
}

url = base_url + "account/area/me/packages?" + "&".join(f"{k}={v}" for k, v in query.items())
headers = {
    "Accept": "application/json",
    "Content-Type": "application/json",
    "Authorization": f"Basic {app_hash}"
}

response = requests.get(url, headers=headers)
http_code = response.status_code

try:
    response_json = response.json()
except Exception as e:
    print(json.dumps({"message": f"Error parsing response: {e}", "response": response.text}, ensure_ascii=False, indent=2))
    exit()

# Output as JSON with proper formatting
if http_code == 200 and response_json.get("code") == 200:
    # Show the packages collection if exists
    if response_json.get("collection"):
        print(json.dumps(response_json["collection"], ensure_ascii=False, indent=2))
    elif "total_balance" in response_json:
        # Or show only the total balance
        print(json.dumps({"total_balance": response_json["total_balance"]}, ensure_ascii=False, indent=2))
    else:
        print(json.dumps({"message": "No package data found.", "response": response_json}, ensure_ascii=False, indent=2))
else:
    error_msg = response_json.get("message", "Failed to fetch packages data")
    print(json.dumps({"message": error_msg, "error": response_json}, ensure_ascii=False, indent=2))
