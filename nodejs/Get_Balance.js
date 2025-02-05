const https = require('https');
const base64 = require('base-64');

const app_id = "Api key";
const app_sec = "Api Secret";
const app_hash = base64.encode(`${app_id}:${app_sec}`);
const base_url = "https://api-sms.4jawaly.com/api/v1/";

const query = {}; // Define the query parameters here if needed

const url = new URL(base_url + "account/area/me/packages");
url.search = new URLSearchParams(query).toString();

const headers = {
    "Accept": "application/json",
    "Content-Type": "application/json",
    "Authorization": `Basic ${app_hash}`
};

const req = https.get(url, { headers }, res => {
    let response_text = '';

    res.on('data', chunk => {
        response_text += chunk;
    });

    res.on('end', () => {
        const response_json = JSON.parse(response_text);
        if (res.statusCode === 200) {
            console.log(`Your balance: ${response_json['total_balance']}`);
        } else if (res.statusCode === 400) {
            console.log(response_json['message']);
        } else {
            console.log(`Error code: ${res.statusCode}`);
        }
    });
});

req.on('error', error => {
    console.error('Error:', error);
});

req.end();
