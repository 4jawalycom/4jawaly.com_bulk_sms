import requests
import json
import base64

app_id = "Api Key"
app_sec = "Api Secret"
app_hash = base64.b64encode(f"{app_id}:{app_sec}".encode()).decode()
base_url = "https://api-sms.4jawaly.com/api/v1/"

url = base_url + "account/area/senders?" + "&".join(f"{k}={v}" for k, v in query.items())

headers = {
    "Accept": "application/json",
    "Content-Type": "application/json",
    "Authorization": f"Basic {app_hash}"
}

response = requests.get(url, headers=headers)
response_json = json.loads(response.text)

print("Error code: " + str(response_json["code"]))
if response_json["code"] == 200:
    for item in response_json["items"]:
        print(item["sender_name"])
elif response.status_code == 400:
    response_json = json.loads(response.text)
    print(response_json["message"])
else:
    print(response.status_code)
