import axios from 'axios';
import base64 from 'base-64';

const app_id = "api key";
const app_sec = "api secret";
const app_hash = base64.encode(`${app_id}:${app_sec}`);
const base_url = "https://api-sms.4jawaly.com/api/v1/";

const query = {}; // Define the query parameters here if needed

const url = base_url + "account/area/senders?" + Object.entries(query).map(([key, value]) => `${key}=${value}`).join("&");
const headers = {
  "Accept": "application/json",
  "Content-Type": "application/json",
  "Authorization": `Basic ${app_hash}`
};

axios.get(url, { headers })
  .then(response => {
    const response_json = response.data;

    if (response_json["code"] === 200) {
      const senders = response_json["items"]["data"].map(item => item.sender_name);

      console.log(senders.join("\n"));
    } else if (!response_json["is_token_expired"]) {
      console.log(response_json["message"]);
    }
  })
  .catch(error => {
    console.error(error);
  });
