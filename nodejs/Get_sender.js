const request = require('request');
const base64 = require('base-64');

const app_id = 'Api Key';
const app_sec = 'Api Secret';
const app_hash = base64.encode(`${app_id}:${app_sec}`);
const base_url = 'https://api-sms.4jawaly.com/api/v1/';

const query = {};  // Define the query parameters here if needed

let url = base_url + 'account/area/senders?' + Object.entries(query).map(([k, v]) => `${k}=${v}`).join('&');

const headers = {
    'Accept': 'application/json',
    'Content-Type': 'application/json',
    'Authorization': `Basic ${app_hash}`,
};

request.get({url, headers}, (err, response, body) => {
    if (err) {
        console.error('Error executing request:', err);
        return;
    }

    const responseJSON = JSON.parse(body);

    const code = responseJSON.code;
    console.log('error code:', code);

    if (code === 200) {
        const data = responseJSON.items.data;
        for (const item of data) {
            console.log(item.sender_name);
        }
    } else {
        console.log('message:', responseJSON.message);
    }
});
