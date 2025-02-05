package sms.java;

import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.net.URI;
import org.json.JSONObject;
import org.json.JSONArray;

public class SMSSender {
    private final HttpClient httpClient;

    public SMSSender() {
        this.httpClient = HttpClient.newHttpClient();
    }

    public void sendSMS(String message, String[] numbers, String sender) {
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
            handleResponse(response);
        } catch (Exception e) {
            System.err.println("Error sending SMS: " + e.getMessage());
        }
    }

    private void handleResponse(HttpResponse<String> response) {
        int statusCode = response.statusCode();
        String body = response.body();
        JSONObject jsonResponse = new JSONObject(body);

        if (statusCode == 200) {
            if (jsonResponse.has("job_id")) {
                System.out.println("تم الارسال بنجاح. job id: " + jsonResponse.getString("job_id"));
            } else {
                System.out.println(jsonResponse.getJSONArray("messages").getJSONObject(0).getString("err_text"));
            }
        } else if (statusCode == 400) {
            System.out.println(jsonResponse.getString("message"));
        } else if (statusCode == 422) {
            System.out.println("نص الرسالة فارغ");
        } else {
            System.out.println("محظور بواسطة كلاودفلير. Status code: " + statusCode);
        }
    }
}
