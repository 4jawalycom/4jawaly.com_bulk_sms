const axios = require('axios');

// =========[ 1. API Credentials ]=========
const API_KEY = 'API_KEY';         // ضع مفتاح API هنا
const API_SECRET = 'API_SECRET';   // ضع السر هنا
const BASE_URL = 'https://api-sms.4jawaly.com/api/v1';

// =========[ 2. API Client Configuration ]=========
const client = axios.create({
    baseURL: BASE_URL,
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'Basic ' + Buffer.from(`${API_KEY}:${API_SECRET}`).toString('base64')
    }
});

// =========[ 3. Get Senders Function ]=========
async function getSenders(options = {}) {
    try {
        const params = {
            page_size: options.page_size ?? 10,
            page: options.page ?? 1,
            status: options.status ?? 1,
            return_collection: options.return_collection ?? 1
        };
        const response = await client.get('/account/area/senders', { params });
        return response.data;
    } catch (error) {
        console.error('Error getting senders:', error.response?.data || error.message);
        return null;
    }
}

// =========[ 4. Get Balance Function ]=========
async function checkBalance(options = {}) {
    try {
        const params = {
            is_active: options.is_active ?? 1,
            order_by: options.order_by ?? 'id',
            order_by_type: options.order_by_type ?? 'desc',
            page: options.page ?? 1,
            page_size: options.page_size ?? 10,
            return_collection: options.return_collection ?? 1
        };
        const response = await client.get('/account/area/me/packages', { params });
        return response.data;
    } catch (error) {
        console.error('Error checking balance:', error.response?.data || error.message);
        return null;
    }
}

// =========[ 5. Main Script ]=========
(async () => {
    // Get and display senders
    const sendersList = await getSenders();
    if (sendersList && Array.isArray(sendersList.items)) {
        const formattedSenders = sendersList.items.map(item => ({
            sender_name: item.sender_name,
            is_default: !!item.is_default
        }));
        console.log("Senders:");
        console.log(JSON.stringify(formattedSenders, null, 2));
    } else {
        console.log("No senders found or error occurred.");
    }

    // Get and display balance
    const balanceInfo = await checkBalance();
    if (balanceInfo && typeof balanceInfo.total_balance !== 'undefined') {
        console.log("Your balance:", balanceInfo.total_balance);
    } else {
        console.log("No balance info found or error occurred.");
    }
})();
