package sms.java;

public class Config {
    private static final String BASE_URL = "https://api-sms.4jawaly.com/api/v1/";
    private static final String APP_ID = "your_api_key";
    private static final String APP_SECRET = "your_api_secret";

    public static String getBaseUrl() {
        return BASE_URL;
    }

    public static String getAppId() {
        return APP_ID;
    }

    public static String getAppSecret() {
        return APP_SECRET;
    }

    public static String getAuthHeader() {
        String credentials = APP_ID + ":" + APP_SECRET;
        return "Basic " + java.util.Base64.getEncoder().encodeToString(credentials.getBytes());
    }
}
