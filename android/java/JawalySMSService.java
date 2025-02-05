package com.jawaly.sms;

import android.util.Base64;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;

public class JawalySMSService {
    private final String baseUrl = "https://api-sms.4jawaly.com/api/v1/";
    private final String appId;
    private final String appSecret;
    private final ExecutorService executorService;

    public JawalySMSService(String appId, String appSecret) {
        this.appId = appId;
        this.appSecret = appSecret;
        this.executorService = Executors.newCachedThreadPool();
    }

    private String getAuthHeader() {
        String credentials = appId + ":" + appSecret;
        return "Basic " + Base64.encodeToString(credentials.getBytes(), Base64.NO_WRAP);
    }

    public static class Result<T> {
        private final T data;
        private final String error;

        private Result(T data, String error) {
            this.data = data;
            this.error = error;
        }

        public static <T> Result<T> success(T data) {
            return new Result<>(data, null);
        }

        public static <T> Result<T> error(String message) {
            return new Result<>(null, message);
        }

        public boolean isSuccess() {
            return error == null;
        }

        public T getData() {
            return data;
        }

        public String getError() {
            return error;
        }
    }

    public Future<Result<String>> sendSMS(final String message, final List<String> numbers, final String sender) {
        return executorService.submit(new Callable<Result<String>>() {
            @Override
            public Result<String> call() {
                try {
                    URL url = new URL(baseUrl + "account/area/sms/send");
                    HttpURLConnection connection = (HttpURLConnection) url.openConnection();
                    connection.setRequestMethod("POST");
                    connection.setRequestProperty("Content-Type", "application/json");
                    connection.setRequestProperty("Accept", "application/json");
                    connection.setRequestProperty("Authorization", getAuthHeader());
                    connection.setDoOutput(true);

                    JSONObject messageJson = new JSONObject();
                    JSONArray messagesArray = new JSONArray();
                    JSONObject messageObj = new JSONObject();
                    messageObj.put("text", message);
                    messageObj.put("numbers", new JSONArray(numbers));
                    messageObj.put("sender", sender);
                    messagesArray.put(messageObj);
                    messageJson.put("messages", messagesArray);

                    try (OutputStream os = connection.getOutputStream()) {
                        byte[] input = messageJson.toString().getBytes("utf-8");
                        os.write(input, 0, input.length);
                    }

                    try (BufferedReader br = new BufferedReader(new InputStreamReader(connection.getInputStream(), "utf-8"))) {
                        StringBuilder response = new StringBuilder();
                        String responseLine;
                        while ((responseLine = br.readLine()) != null) {
                            response.append(responseLine.trim());
                        }

                        JSONObject jsonResponse = new JSONObject(response.toString());
                        if (jsonResponse.has("job_id")) {
                            return Result.success(jsonResponse.getString("job_id"));
                        } else if (jsonResponse.has("messages")) {
                            JSONArray messages = jsonResponse.getJSONArray("messages");
                            if (messages.length() > 0) {
                                JSONObject firstMessage = messages.getJSONObject(0);
                                if (firstMessage.has("err_text")) {
                                    return Result.error(firstMessage.getString("err_text"));
                                }
                            }
                        }
                        return Result.error("Unknown error");
                    }
                } catch (Exception e) {
                    return Result.error(e.getMessage());
                }
            }
        });
    }

    public Future<Result<Double>> checkBalance() {
        return executorService.submit(new Callable<Result<Double>>() {
            @Override
            public Result<Double> call() {
                try {
                    URL url = new URL(baseUrl + "account/area/me/packages");
                    HttpURLConnection connection = (HttpURLConnection) url.openConnection();
                    connection.setRequestMethod("GET");
                    connection.setRequestProperty("Accept", "application/json");
                    connection.setRequestProperty("Content-Type", "application/json");
                    connection.setRequestProperty("Authorization", getAuthHeader());

                    try (BufferedReader br = new BufferedReader(new InputStreamReader(connection.getInputStream(), "utf-8"))) {
                        StringBuilder response = new StringBuilder();
                        String responseLine;
                        while ((responseLine = br.readLine()) != null) {
                            response.append(responseLine.trim());
                        }

                        JSONObject jsonResponse = new JSONObject(response.toString());
                        if (jsonResponse.getInt("code") == 200) {
                            return Result.success(jsonResponse.getDouble("total_balance"));
                        } else {
                            return Result.error(jsonResponse.optString("message", "Unknown error"));
                        }
                    }
                } catch (Exception e) {
                    return Result.error(e.getMessage());
                }
            }
        });
    }

    public Future<Result<List<String>>> getSenders() {
        return executorService.submit(new Callable<Result<List<String>>>() {
            @Override
            public Result<List<String>> call() {
                try {
                    URL url = new URL(baseUrl + "account/area/senders");
                    HttpURLConnection connection = (HttpURLConnection) url.openConnection();
                    connection.setRequestMethod("GET");
                    connection.setRequestProperty("Accept", "application/json");
                    connection.setRequestProperty("Content-Type", "application/json");
                    connection.setRequestProperty("Authorization", getAuthHeader());

                    try (BufferedReader br = new BufferedReader(new InputStreamReader(connection.getInputStream(), "utf-8"))) {
                        StringBuilder response = new StringBuilder();
                        String responseLine;
                        while ((responseLine = br.readLine()) != null) {
                            response.append(responseLine.trim());
                        }

                        JSONObject jsonResponse = new JSONObject(response.toString());
                        if (jsonResponse.getInt("code") == 200) {
                            List<String> senders = new ArrayList<>();
                            JSONArray items = jsonResponse.getJSONArray("items");
                            for (int i = 0; i < items.length(); i++) {
                                senders.add(items.getJSONObject(i).getString("sender_name"));
                            }
                            return Result.success(senders);
                        } else {
                            return Result.error(jsonResponse.optString("message", "Unknown error"));
                        }
                    }
                } catch (Exception e) {
                    return Result.error(e.getMessage());
                }
            }
        });
    }

    public void shutdown() {
        executorService.shutdown();
    }
}
