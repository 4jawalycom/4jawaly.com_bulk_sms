const axios = require('axios');

/**
 * تكوين واجهة برمجة التطبيقات
 * API Configuration
 * اے پی آئی کنفیگریشن
 */
const API_KEY = process.env.SMS_API_KEY;
const API_SECRET = process.env.SMS_API_SECRET;
const BASE_URL = 'https://api-sms.4jawaly.com/api/v1';

// API client configuration
const client = axios.create({
    baseURL: BASE_URL,
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': 'Basic ' + Buffer.from(`${API_KEY}:${API_SECRET}`).toString('base64')
    }
});

/**
 * تقسيم المصفوفة إلى مجموعات
 * Split array into chunks
 * ایرے کو چنکس میں تقسیم کریں
 * @param {Array} array - المصفوفة | Array | ایرے
 * @param {number} size - حجم المجموعة | Chunk size | چنک سائز
 * @returns {Array[]} - مصفوفة من المجموعات | Array of chunks | چنکس کی ایرے
 */
function chunkArray(array, size) {
    const chunks = [];
    for (let i = 0; i < array.length; i += size) {
        chunks.push(array.slice(i, i + size));
    }
    return chunks;
}

/**
 * إرسال رسائل SMS
 * Send SMS messages
 * ایس ایم ایس پیغامات بھیجیں
 * 
 * @param {Object} params - المعلمات | Parameters | پیرامیٹرز
 * @param {string} params.message - محتوى الرسالة | Message content | پیغام کا مواد
 * @param {string[]} params.numbers - مصفوفة أرقام الهواتف | Array of phone numbers | فون نمبرز کی ایرے
 * @param {string} params.sender - معرف المرسل | Sender ID | بھیجنے والے کی شناخت
 * @returns {Promise} - وعد بنتيجة الإرسال | Promise with sending result | بھیجنے کے نتیجے کا وعدہ
 */
async function sendSMS({ message, numbers, sender }) {
    try {
        numbers = Array.isArray(numbers) ? numbers : [numbers];
        
        // تحديد حجم المجموعة بناءً على عدد الأرقام
        // Determine chunk size based on number count
        // نمبروں کی تعداد کی بنیاد پر چنک سائز کا تعین
        let chunkSize;
        if (numbers.length <= 5) {
            chunkSize = numbers.length;
        } else if (numbers.length <= 100) {
            chunkSize = Math.ceil(numbers.length / 5);
        } else {
            chunkSize = 100;
        }

        // تقسيم الأرقام إلى مجموعات
        // Split numbers into chunks
        // نمبروں کو چنکس میں تقسیم کریں
        const chunks = chunkArray(numbers, chunkSize);

        // إرسال كل مجموعة بشكل متوازي
        // Send each chunk in parallel
        // ہر چنک کو متوازی طور پر بھیجیں
        const results = await Promise.all(chunks.map(async (chunk) => {
            try {
                const response = await client.post('/account/area/sms/send', {
                    messages: [{
                        text: message,
                        numbers: chunk,
                        sender: sender
                    }]
                });

                return {
                    success: response.status === 200,
                    chunk,
                    response: response.data
                };
            } catch (error) {
                return {
                    success: false,
                    chunk,
                    error: error.response?.data || error.message
                };
            }
        }));

        // تجميع النتائج
        // Aggregate results
        // نتائج کو جمع کریں
        const aggregatedResult = {
            success: true,
            total_success: 0,
            total_failed: 0,
            job_ids: [],
            errors: {}
        };

        results.forEach(result => {
            if (result.success && !result.response.messages?.[0]?.err_text) {
                aggregatedResult.total_success += result.chunk.length;
                if (result.response.job_id) {
                    aggregatedResult.job_ids.push(result.response.job_id);
                }
            } else {
                aggregatedResult.total_failed += result.chunk.length;
                const errorMessage = result.response?.messages?.[0]?.err_text || result.error;
                if (!aggregatedResult.errors[errorMessage]) {
                    aggregatedResult.errors[errorMessage] = [];
                }
                aggregatedResult.errors[errorMessage].push(...result.chunk);
            }
        });

        aggregatedResult.success = aggregatedResult.total_failed === 0;
        return aggregatedResult;

    } catch (error) {
        console.error('Error sending SMS:', error.response?.data || error.message);
        throw error;
    }
}

/**
 * Check account balance
 * @param {Object} options - Optional parameters
 * @returns {Promise}
 */
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
        throw error;
    }
}

/**
 * Get list of senders
 * @param {Object} options - Optional parameters
 * @returns {Promise}
 */
async function getSenders(options = {}) {
    try {
        const params = {
            page_size: options.page_size ?? 10,
            page: options.page ?? 1,
            status: options.status ?? 1,
            sender_name: options.sender_name ?? '',
            is_ad: options.is_ad ?? '',
            return_collection: options.return_collection ?? 1
        };

        const response = await client.get('/account/area/senders', { params });
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
