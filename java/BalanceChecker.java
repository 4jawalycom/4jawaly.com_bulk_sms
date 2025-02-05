package sms.java;

import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.net.URI;
import org.json.JSONObject;

public class BalanceChecker {
    private final HttpClient httpClient;

    public BalanceChecker() {
        this.httpClient = HttpClient.newHttpClient();
    }

    public void checkBalance() {
        try {
            HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(Config.getBaseUrl() + "account/area/me/packages"))
                .header("Accept", "application/json")
                .header("Content-Type", "application/json")
                .header("Authorization", Config.getAuthHeader())
                .GET()
                .build();

            HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());
            handleResponse(response);
        } catch (Exception e) {
            System.err.println("Error checking balance: " + e.getMessage());
        }
    }

    private void handleResponse(HttpResponse<String> response) {
        if (response.statusCode() == 200) {
            JSONObject jsonResponse = new JSONObject(response.body());
            if (jsonResponse.getInt("code") == 200) {
                System.out.println("Your balance: " + jsonResponse.getDouble("total_balance"));
            } else {
                System.out.println(jsonResponse.getString("message"));
            }
        } else {
            System.out.println("Error code: " + response.statusCode());
        }
    }
}
