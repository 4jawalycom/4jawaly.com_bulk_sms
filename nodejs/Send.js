const https = require('https');

const app_id = 'Api Key';
const app_sec = 'Api Secret';
const app_hash = Buffer.from(`${app_id}:${app_sec}`).toString('base64');

const messages = {
  messages: [
    {
      text: 'test',
      numbers: ['966500000'],
      sender: 'test',
    },
  ],
};

const url = new URL('https://api-sms.4jawaly.com/api/v1/account/area/sms/send');
const options = {
  method: 'POST',
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json',
    Authorization: `Basic ${app_hash}`,
  },
};

const req = https.request(url, options, (res) => {
  let response_data = '';
  res.on('data', (chunk) => {
    response_data += chunk;
  });

  res.on('end', () => {
    const response_json = JSON.parse(response_data);
    const status_code = res.statusCode;
    if (status_code == 200) {
      if (response_json['messages'][0]['err_text']) {
        console.log(response_json['messages'][0]['err_text']);
      } else {
        console.log('تم الارسال بنجاح job id: ' + response_json['job_id']);
      }
    } else if (status_code == 400) {
      console.log(response_json['message']);
    } else if (status_code == 422) {
      console.log('نص الرسالة فارغ');
    } else {
      console.log('محظور بواسطة كلاودفلير. Status code: ' + status_code);
    }
  });
});

req.on('error', (error) => {
  console.log('Error sending request:', error);
});

req.write(JSON.stringify(messages));
req.end();
