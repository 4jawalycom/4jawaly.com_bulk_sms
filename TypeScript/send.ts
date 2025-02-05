import axios from 'axios';
import base64 from 'base-64';

const app_id = "api key";
const app_sec = "api secret";
const app_hash = base64.encode(`${app_id}:${app_sec}`);

const messages = {
  "messages": [
    {
      "text": "test",
      "numbers": ["9665000000"],
      "sender": "test"
    }
  ]
};

const url = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send";
const headers = {
  "Accept": "application/json",
  "Content-Type": "application/json",
  "Authorization": `Basic ${app_hash}`
};

axios.post(url, messages, { headers })
  .then(response => {
    const response_json = response.data;

    if (response.status === 200) {
      if ("err_text" in response_json["messages"][0]) {
        console.log(response_json["messages"][0]["err_text"]);
      } else {
        console.log(`تم الارسال بنجاح job id:${response_json["job_id"]}`);
      }
    } else if (response.status === 400) {
      console.log(response_json["message"]);
    } else if (response.status === 422) {
      console.log("نص الرسالة فارغ");
    } else {
      console.log(`محظور بواسطة كلاودفلير. Status code: ${response.status}`);
    }
  })
  .catch(error => {
    console.error(error);
  });
