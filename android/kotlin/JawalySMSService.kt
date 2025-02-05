import android.util.Base64
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import org.json.JSONArray
import org.json.JSONObject
import java.io.BufferedReader
import java.io.InputStreamReader
import java.net.HttpURLConnection
import java.net.URL

class JawalySMSService(
    private val appId: String,
    private val appSecret: String
) {
    private val baseUrl = "https://api-sms.4jawaly.com/api/v1/"

    private val authHeader: String
        get() {
            val credentials = "$appId:$appSecret"
            return "Basic " + Base64.encodeToString(credentials.toByteArray(), Base64.NO_WRAP)
        }

    sealed class Result<out T> {
        data class Success<T>(val data: T) : Result<T>()
        data class Error(val message: String) : Result<Nothing>()
    }

    suspend fun sendSMS(
        message: String,
        numbers: List<String>,
        sender: String
    ): Result<String> = withContext(Dispatchers.IO) {
        try {
            val url = URL("${baseUrl}account/area/sms/send")
            val connection = (url.openConnection() as HttpURLConnection).apply {
                requestMethod = "POST"
                setRequestProperty("Content-Type", "application/json")
                setRequestProperty("Accept", "application/json")
                setRequestProperty("Authorization", authHeader)
                doOutput = true
            }

            val messageJson = JSONObject().apply {
                put("messages", JSONArray().apply {
                    put(JSONObject().apply {
                        put("text", message)
                        put("numbers", JSONArray(numbers))
                        put("sender", sender)
                    })
                })
            }

            connection.outputStream.use { os ->
                os.write(messageJson.toString().toByteArray())
            }

            val response = connection.inputStream.use { stream ->
                BufferedReader(InputStreamReader(stream)).use { reader ->
                    reader.readText()
                }
            }

            val jsonResponse = JSONObject(response)
            when {
                jsonResponse.has("job_id") -> {
                    Result.Success(jsonResponse.getString("job_id"))
                }
                jsonResponse.has("messages") -> {
                    val messages = jsonResponse.getJSONArray("messages")
                    if (messages.length() > 0) {
                        val firstMessage = messages.getJSONObject(0)
                        if (firstMessage.has("err_text")) {
                            Result.Error(firstMessage.getString("err_text"))
                        } else {
                            Result.Error("Unknown error")
                        }
                    } else {
                        Result.Error("Empty response")
                    }
                }
                else -> Result.Error("Invalid response format")
            }
        } catch (e: Exception) {
            Result.Error(e.message ?: "Unknown error occurred")
        }
    }

    suspend fun checkBalance(): Result<Double> = withContext(Dispatchers.IO) {
        try {
            val url = URL("${baseUrl}account/area/me/packages")
            val connection = (url.openConnection() as HttpURLConnection).apply {
                requestMethod = "GET"
                setRequestProperty("Accept", "application/json")
                setRequestProperty("Content-Type", "application/json")
                setRequestProperty("Authorization", authHeader)
            }

            val response = connection.inputStream.use { stream ->
                BufferedReader(InputStreamReader(stream)).use { reader ->
                    reader.readText()
                }
            }

            val jsonResponse = JSONObject(response)
            if (jsonResponse.getInt("code") == 200) {
                Result.Success(jsonResponse.getDouble("total_balance"))
            } else {
                Result.Error(jsonResponse.optString("message", "Unknown error"))
            }
        } catch (e: Exception) {
            Result.Error(e.message ?: "Unknown error occurred")
        }
    }

    suspend fun getSenders(): Result<List<String>> = withContext(Dispatchers.IO) {
        try {
            val url = URL("${baseUrl}account/area/senders")
            val connection = (url.openConnection() as HttpURLConnection).apply {
                requestMethod = "GET"
                setRequestProperty("Accept", "application/json")
                setRequestProperty("Content-Type", "application/json")
                setRequestProperty("Authorization", authHeader)
            }

            val response = connection.inputStream.use { stream ->
                BufferedReader(InputStreamReader(stream)).use { reader ->
                    reader.readText()
                }
            }

            val jsonResponse = JSONObject(response)
            if (jsonResponse.getInt("code") == 200) {
                val items = jsonResponse.getJSONArray("items")
                val senders = mutableListOf<String>()
                for (i in 0 until items.length()) {
                    senders.add(items.getJSONObject(i).getString("sender_name"))
                }
                Result.Success(senders)
            } else {
                Result.Error(jsonResponse.optString("message", "Unknown error"))
            }
        } catch (e: Exception) {
            Result.Error(e.message ?: "Unknown error occurred")
        }
    }
}
