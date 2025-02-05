const axios = require('axios');

// Configuration
const API_KEY = process.env.JAWALY_API_KEY;
const API_SECRET = process.env.JAWALY_API_SECRET;
const BASE_URL = 'https://api.4jawaly.com/api/v1';

// API client configuration
const client = axios.create({
    baseURL: BASE_URL,
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'API-KEY': API_KEY,
        'API-SECRET': API_SECRET
    }
});

/**
 * Send SMS messages
 * @param {Object} params
 * @param {string} params.message - Message content
 * @param {string[]} params.numbers - Array of phone numbers
 * @param {string} params.sender - Sender ID
 * @returns {Promise}
 */
async function sendSMS({ message, numbers, sender }) {
    try {
        const response = await client.post('/account/area/sms/send', {
            messages: numbers.map(number => ({
                text: message,
                numbers: [number],
                sender: sender
            }))
        });
        return response.data;
    } catch (error) {
        console.error('Error sending SMS:', error.response?.data || error.message);
        throw error;
    }
}

/**
 * Check account balance
 * @returns {Promise}
 */
async function checkBalance() {
    try {
        const response = await client.get('/account/area/me/balance');
        return response.data;
    } catch (error) {
        console.error('Error checking balance:', error.response?.data || error.message);
        throw error;
    }
}

/**
 * Get list of senders
 * @returns {Promise}
 */
async function getSenders() {
    try {
        const response = await client.get('/account/area/sender/list');
        return response.data;
    } catch (error) {
        console.error('Error getting senders:', error.response?.data || error.message);
        throw error;
    }
}

module.exports = {
    sendSMS,
    checkBalance,
    getSenders
};
