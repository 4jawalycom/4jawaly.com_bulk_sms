import axios from 'axios';
import base64 from 'base-64';

const app_id = "Api Key";
const app_sec = "Api Secrit";
const app_hash = base64.encode(`${app_id}:${app_sec}`);
const base_url = "https://api-sms.4jawaly.com/api/v1/";

const query = {}; // Define the query parameters here if needed

const url = base_url + "account/area/me/packages?" + Object.entries(query).map(([key, value]) => `${key}=${value}`).join("&");
const headers = {
  "Accept": "application/json",
  "Content-Type": "application/json",
  "Authorization": `Basic ${app_hash}`
};

axios.get(url, { headers })
  .then(response => {
    if (response.status !== 200) {
      console.log(`Error code: ${response.status}`);
      return;
    }

    const response_json = response.data;

    if (response_json["code"] === 200) {
      console.log("Your balance: " + response_json["total_balance"]);
    } else {
      console.log(response_json["message"]);
    }
  })
  .catch(error => {
    console.error(error);
  });
