require 'net/http'
require 'uri'
require 'json'
require 'base64'

app_id = "Api Key"
app_sec = "Api Secret"
app_hash = Base64.encode64("#{app_id}:#{app_sec}").strip

base_url = "https://api-sms.4jawaly.com/api/v1/"

query = {} # Replace with your query parameters if needed

url = URI.parse(base_url + "account/area/senders?" + query.map { |k, v| "#{k}=#{v}" }.join("&"))

headers = {
  "Accept" => "application/json",
  "Content-Type" => "application/json",
  "Authorization" => "Basic #{app_hash}"
}

http = Net::HTTP.new(url.host, url.port)
http.use_ssl = true
request = Net::HTTP::Get.new(url.request_uri, headers)

response = http.request(request)
response_json = JSON.parse(response.body)

puts "Error code: #{response_json['code']}"

if response_json['code'] == 200
  response_json['items'].each do |item|
    puts item['sender_name']
  end
elsif response.code == '400'
  puts response_json['message']
else
  puts response.code
end
