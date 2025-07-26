import requests
import json
import base64

app_id = "API_KEY"
app_sec = "API_secret"
app_hash = base64.b64encode(f"{app_id}:{app_sec}".encode()).decode()
base_url = "https://api-sms.4jawaly.com/api/v1/"
query = {
    "page_size": 10,
    "page": 1,
    "status": 1,
    "return_collection": 1
}

url = base_url + "account/area/senders?" + "&".join(f"{k}={v}" for k, v in query.items())

headers = {
    "Accept": "application/json",
    "Content-Type": "application/json",
    "Authorization": f"Basic {app_hash}"
}

response = requests.get(url, headers=headers)
response_json = response.json()

senders_list = []
if response.status_code == 200 and "items" in response_json and response_json["items"]:
    for item in response_json["items"]:
        sender_name = item.get("sender_name", "")
        is_default = bool(item.get("is_default", 0))
        senders_list.append({
            "sender_name": sender_name,
            "is_default": is_default
        })
    print(json.dumps(senders_list, ensure_ascii=False, indent=2))
else:
    print(json.dumps({"message": "No senders found"}, ensure_ascii=False, indent=2))
