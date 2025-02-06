# فئة بوابة إرسال الرسائل النصية
# SMS Gateway Class
# ایس ایم ایس گیٹ وے کلاس
class SMSGateway
  def initialize(api_key, api_secret)
    @api_key = api_key
    @api_secret = api_secret
    @base_url = "https://api-sms.4jawaly.com/api/v1"
    @thread_pool = Concurrent::FixedThreadPool.new(5)
  end

  # إرسال رسائل SMS مع دعم الإرسال المتوازي
  # Send SMS messages with parallel sending support
  # متوازی بھیجنے کی سہولت کے ساتھ ایس ایم ایس پیغامات بھیجیں
  def send_sms(message, numbers, sender)
    # تهيئة النتيجة | Initialize result | نتیجہ کو شروع کریں
    result = {
      success: true,
      total_success: 0,
      total_failed: 0,
      job_ids: [],
      errors: {}
    }

    # تحديد حجم المجموعة | Determine chunk size | حصے کا سائز متعین کریں
    chunk_size = case numbers.length
                 when 0..5 then numbers.length
                 when 6..100 then (numbers.length + 4) / 5
                 else 100
                 end

    # تقسيم الأرقام | Split numbers | نمبروں کو تقسیم کریں
    chunks = numbers.each_slice(chunk_size).to_a

    # إرسال متوازي | Parallel sending | متوازی بھیجنا
    futures = chunks.map do |chunk|
      Concurrent::Future.execute(executor: @thread_pool) do
        send_chunk(message, chunk, sender)
      end
    end

    # معالجة النتائج | Process results | نتائج کی پروسیسنگ
    futures.each do |future|
      chunk_result = future.value
      if chunk_result[:error]
        result[:total_failed] += chunk_result[:numbers].length
        error_msg = chunk_result[:error]
        result[:errors][error_msg] ||= []
        result[:errors][error_msg].concat(chunk_result[:numbers])
      else
        response_json = chunk_result[:response]
        if response_json["messages"][0].key?("err_text")
          result[:total_failed] += chunk_result[:numbers].length
          error_msg = response_json["messages"][0]["err_text"]
          result[:errors][error_msg] ||= []
          result[:errors][error_msg].concat(chunk_result[:numbers])
        else
          result[:total_success] += chunk_result[:numbers].length
          result[:job_ids] << response_json["job_id"] if response_json["job_id"]
        end
      end
    end

    result[:success] = result[:total_failed].zero?
    result
  end

  private

  # إنشاء ترويسة المصادقة | Create auth header | تصدیق ہیڈر بنائیں
  def auth_header
    "Basic #{Base64.strict_encode64("#{@api_key}:#{@api_secret}")}"
  end

  # إرسال مجموعة من الأرقام | Send chunk of numbers | نمبروں کا حصہ بھیجیں
  def send_chunk(message, numbers, sender)
    messages = {
      messages: [
        {
          text: message,
          numbers: numbers,
          sender: sender
        }
      ]
    }

    url = URI("#{@base_url}/account/area/sms/send")
    http = Net::HTTP.new(url.host, url.port)
    http.use_ssl = true

    request = Net::HTTP::Post.new(url)
    request["Accept"] = "application/json"
    request["Content-Type"] = "application/json"
    request["Authorization"] = auth_header
    request.body = JSON.dump(messages)

    begin
      response = http.request(request)
      {
        numbers: numbers,
        response: JSON.parse(response.read_body),
        status_code: response.code.to_i
      }
    rescue StandardError => e
      {
        numbers: numbers,
        error: e.message
      }
    end
  end
end

# مثال على الاستخدام | Usage example | استعمال کی مثال
if __FILE__ == $0
  gateway = SMSGateway.new(ENV["SMS_API_KEY"] || "your_api_key", ENV["SMS_API_SECRET"] || "your_api_secret")
  
  result = gateway.send_sms(
    "اختبار الإرسال المتوازي | Parallel sending test | متوازی بھیجنے کا ٹیسٹ",
    ["966500000001", "966500000002", "966500000003"],
    "4jawaly"
  )
  
  puts JSON.pretty_generate(result)
end
