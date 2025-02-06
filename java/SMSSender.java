package sms.java;

import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.net.URI;
import java.util.*;
import java.util.concurrent.*;
import java.util.stream.*;
import org.json.JSONObject;
import org.json.JSONArray;

/**
 * فئة إرسال الرسائل النصية مع دعم الإرسال المتوازي
 * SMS Sender class with parallel sending support
 * متوازی بھیجنے کی سہولت کے ساتھ ایس ایم ایس بھیجنے والی کلاس
 */
public class SMSSender {
    private final HttpClient httpClient;
    private final ExecutorService executor;

    /**
     * تهيئة المرسل
     * Initialize sender
     * بھیجنے والے کو شروع کریں
     */
    public SMSSender() {
        this.httpClient = HttpClient.newHttpClient();
        this.executor = Executors.newFixedThreadPool(5);
    }

    /**
     * تقسيم المصفوفة إلى مجموعات
     * Split array into chunks
     * ایرے کو حصوں میں تقسیم کریں
     */
    private List<String[]> chunkArray(String[] array, int chunkSize) {
        return IntStream.range(0, (array.length + chunkSize - 1) / chunkSize)
            .mapToObj(i -> Arrays.copyOfRange(array, 
                i * chunkSize, 
                Math.min((i + 1) * chunkSize, array.length)))
            .collect(Collectors.toList());
    }

    /**
     * إرسال مجموعة من الأرقام
     * Send a chunk of numbers
     * نمبروں کا ایک حصہ بھیجیں
     */
    private CompletableFuture<JSONObject> sendChunk(String message, String[] numbers, String sender) {
        return CompletableFuture.supplyAsync(() -> {
            try {
                JSONObject jsonMessage = new JSONObject();
                JSONArray messages = new JSONArray();
                JSONObject messageObj = new JSONObject();
                messageObj.put("text", message);
                messageObj.put("numbers", numbers);
                messageObj.put("sender", sender);
                messages.put(messageObj);
                jsonMessage.put("messages", messages);

                HttpRequest request = HttpRequest.newBuilder()
                    .uri(URI.create(Config.getBaseUrl() + "account/area/sms/send"))
                    .header("Accept", "application/json")
                    .header("Content-Type", "application/json")
                    .header("Authorization", Config.getAuthHeader())
                    .POST(HttpRequest.BodyPublishers.ofString(jsonMessage.toString()))
                    .build();

                HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());
                
                JSONObject result = new JSONObject();
                result.put("statusCode", response.statusCode());
                result.put("response", new JSONObject(response.body()));
                result.put("numbers", numbers);
                return result;
            } catch (Exception e) {
                JSONObject error = new JSONObject();
                error.put("error", e.getMessage());
                error.put("numbers", numbers);
                return error;
            }
        }, executor);
    }

    /**
     * إرسال رسائل SMS مع دعم الإرسال المتوازي
     * Send SMS messages with parallel sending support
     * متوازی بھیجنے کی سہولت کے ساتھ ایس ایم ایس پیغامات بھیجیں
     * 
     * @param message نص الرسالة | Message text | پیغام کا متن
     * @param numbers مصفوفة الأرقام | Array of numbers | نمبروں کی ایرے
     * @param sender اسم المرسل | Sender name | بھیجنے والے کا نام
     * @return نتيجة الإرسال | Sending result | بھیجنے کا نتیجہ
     */
    public JSONObject sendSMS(String message, String[] numbers, String sender) {
        try {
            // تحديد حجم المجموعة | Determine chunk size | حصے کا سائز متعین کریں
            int chunkSize;
            if (numbers.length <= 5) {
                chunkSize = numbers.length;
            } else if (numbers.length <= 100) {
                chunkSize = (numbers.length + 4) / 5;
            } else {
                chunkSize = 100;
            }

            // تقسيم الأرقام | Split numbers | نمبروں کو تقسیم کریں
            List<String[]> chunks = chunkArray(numbers, chunkSize);

            // إرسال متوازي | Parallel sending | متوازی بھیجنا
            List<CompletableFuture<JSONObject>> futures = chunks.stream()
                .map(chunk -> sendChunk(message, chunk, sender))
                .collect(Collectors.toList());

            // انتظار النتائج | Wait for results | نتائج کا انتظار کریں
            CompletableFuture.allOf(futures.toArray(new CompletableFuture[0])).join();

            // تجميع النتائج | Aggregate results | نتائج کو یکجا کریں
            JSONObject result = new JSONObject();
            result.put("success", true);
            result.put("total_success", 0);
            result.put("total_failed", 0);
            JSONArray jobIds = new JSONArray();
            JSONObject errors = new JSONObject();

            for (CompletableFuture<JSONObject> future : futures) {
                JSONObject chunkResult = future.get();
                String[] chunkNumbers = chunkResult.getJSONArray("numbers").toList().toArray(new String[0]);

                if (chunkResult.has("statusCode") && chunkResult.getInt("statusCode") == 200) {
                    JSONObject response = chunkResult.getJSONObject("response");
                    if (!response.getJSONArray("messages").getJSONObject(0).has("err_text")) {
                        result.put("total_success", result.getInt("total_success") + chunkNumbers.length);
                        if (response.has("job_id")) {
                            jobIds.put(response.getString("job_id"));
                        }
                    } else {
                        String errorText = response.getJSONArray("messages").getJSONObject(0).getString("err_text");
                        addError(errors, errorText, chunkNumbers);
                        result.put("total_failed", result.getInt("total_failed") + chunkNumbers.length);
                    }
                } else {
                    String errorMessage = chunkResult.has("error") ? 
                        chunkResult.getString("error") : 
                        "HTTP Error: " + chunkResult.getInt("statusCode");
                    addError(errors, errorMessage, chunkNumbers);
                    result.put("total_failed", result.getInt("total_failed") + chunkNumbers.length);
                }
            }

            result.put("job_ids", jobIds);
            result.put("errors", errors);
            result.put("success", result.getInt("total_failed") == 0);

            return result;

        } catch (Exception e) {
            JSONObject error = new JSONObject();
            error.put("success", false);
            error.put("error", e.getMessage());
            return error;
        }
    }

    /**
     * إضافة خطأ إلى قائمة الأخطاء
     * Add error to errors map
     * غلطیوں کے نقشے میں غلطی شامل کریں
     */
    private void addError(JSONObject errors, String errorText, String[] numbers) {
        if (!errors.has(errorText)) {
            errors.put(errorText, new JSONArray());
        }
        Arrays.stream(numbers).forEach(number -> errors.getJSONArray(errorText).put(number));
    }

    /**
     * إغلاق الموارد
     * Close resources
     * وسائل کو بند کریں
     */
    public void close() {
        executor.shutdown();
        try {
            if (!executor.awaitTermination(60, TimeUnit.SECONDS)) {
                executor.shutdownNow();
            }
        } catch (InterruptedException e) {
            executor.shutdownNow();
        }
    }
}
