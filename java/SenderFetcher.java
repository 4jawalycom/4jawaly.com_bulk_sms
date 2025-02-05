package sms.java;

import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.net.URI;
import org.json.JSONObject;
import org.json.JSONArray;

public class SenderFetcher {
    private final HttpClient httpClient;

    public SenderFetcher() {
        this.httpClient = HttpClient.newHttpClient();
    }

    public void fetchSenders() {
        try {
            HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(Config.getBaseUrl() + "account/area/senders"))
                .header("Accept", "application/json")
                .header("Content-Type", "application/json")
                .header("Authorization", Config.getAuthHeader())
                .GET()
                .build();

            HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());
            handleResponse(response);
        } catch (Exception e) {
            System.err.println("Error fetching senders: " + e.getMessage());
        }
    }

    private void handleResponse(HttpResponse<String> response) {
        JSONObject jsonResponse = new JSONObject(response.body());
        System.out.println("Error code: " + jsonResponse.getInt("code"));
        
        if (jsonResponse.getInt("code") == 200) {
            JSONArray items = jsonResponse.getJSONArray("items");
            for (int i = 0; i < items.length(); i++) {
                System.out.println(items.getJSONObject(i).getString("sender_name"));
            }
        } else if (response.statusCode() == 400) {
            System.out.println(jsonResponse.getString("message"));
        } else {
            System.out.println("Status code: " + response.statusCode());
        }
    }
}
