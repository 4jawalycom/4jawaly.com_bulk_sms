import requests
import json
import base64

app_id = "Api Key"
app_sec = "Api Secrit"
app_hash = base64.b64encode(f"{app_id}:{app_sec}".encode()).decode()
base_url = "https://api-sms.4jawaly.com/api/v1/"

query = {}  # Define the query parameters here if needed

url = base_url + "account/area/me/packages?" + "&".join(f"{k}={v}" for k, v in query.items())
headers = {
    "Accept": "application/json",
    "Content-Type": "application/json",
    "Authorization": f"Basic {app_hash}"
}

response = requests.get(url, headers=headers)

if response.status_code != 200:
    print(f"Error code: {response.status_code}")
    exit()

try:
    response_json = json.loads(response.text)
except json.JSONDecodeError as e:
    print(f"Error parsing response content: {e}")
    exit()

if response_json["code"] == 200:
    print("Your balance: " + str(response_json["total_balance"]))
else:
    print(response_json["message"])
