require 'net/http'
require 'uri'
require 'json'
require 'base64'

app_id = "api key"
app_sec = "api secret"
app_hash = Base64.strict_encode64("#{app_id}:#{app_sec}")

base_url = "https://api-sms.4jawaly.com/api/v1/"

query = {} # Define the query parameters here if needed

url = URI.parse(base_url + "account/area/me/packages?" + query.map { |k, v| "#{k}=#{v}" }.join("&"))

headers = {
  "Accept" => "application/json",
  "Content-Type" => "application/json",
  "Authorization" => "Basic #{app_hash}"
}

http = Net::HTTP.new(url.host, url.port)
http.use_ssl = true
request = Net::HTTP::Get.new(url.request_uri, headers)

response = http.request(request)

if response.code != '200'
  puts "Error code: #{response.code}"
  exit
end

begin
  response_json = JSON.parse(response.body)
rescue JSON::ParserError => e
  puts "Error parsing response content: #{e}"
  exit
end

if response_json['code'] == 200
  puts "Your balance: #{response_json['total_balance']}"
else
  puts response_json['message']
end
