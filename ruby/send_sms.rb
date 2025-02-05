# Your code here!

require 'uri'
require 'net/http'
require 'json'
require 'base64'

# يمكنك الحصول على API Key و Secret عن طريق تسجيل الدخول إلى فورجوالي والذهاب إلى البيانات الشخصية والنقر على API Token
app_id = "api key"
app_sec = "api secret"
app_hash = Base64.strict_encode64("#{app_id}:#{app_sec}")

messages = {
  "messages": [
    {
      "text": "test",
      "numbers": ["9665000000"],
      "sender": "4jawaly"
    }
  ]
}

url = URI("https://api-sms.4jawaly.com/api/v1/account/area/sms/send")
http = Net::HTTP.new(url.host, url.port)
http.use_ssl = true

request = Net::HTTP::Post.new(url)
request["Accept"] = "application/json"
request["Content-Type"] = "application/json"
request["Authorization"] = "Basic #{app_hash}"
request.body = JSON.dump(messages)

response = http.request(request)

if response.code.to_i == 200
  response_json = JSON.parse(response.read_body)
  if response_json["messages"][0].key?("err_text")
    puts response_json["messages"][0]["err_text"]
  else
    puts "تم الارسال بنجاح  " + " job id:" + response_json["job_id"]
  end
elsif response.code.to_i == 400
  response_json = JSON.parse(response.read_body)
  puts response_json["message"]
elsif response.code.to_i == 422
  puts "نص الرسالة فارغ"
else
  puts "محظور بواسطة كلاودفلير. Status code: #{response.code}"
end
