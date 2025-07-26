require 'net/http'
require 'uri'
require 'json'
require 'base64'

app_id = "API_KEY"
app_sec = "API_SECRET"
app_hash = Base64.strict_encode64("#{app_id}:#{app_sec}")

base_url = "https://api-sms.4jawaly.com/api/v1/"

# Example query, you can modify
query = {
  "page_size" => 10,
  "page" => 1,
  "status" => 1,
  "return_collection" => 1
}

url = URI.parse(base_url + "account/area/senders?" + URI.encode_www_form(query))

headers = {
  "Accept" => "application/json",
  "Content-Type" => "application/json",
  "Authorization" => "Basic #{app_hash}"
}

http = Net::HTTP.new(url.host, url.port)
http.use_ssl = true
request = Net::HTTP::Get.new(url.request_uri, headers)

response = http.request(request)
response_json = JSON.parse(response.body) rescue {}

puts "HTTP code: #{response.code}"
puts "API code: #{response_json['code']}"

if response.code == '200' && response_json['code'] == 200
  if response_json['items'] && !response_json['items'].empty?
    response_json['items'].each do |item|
      puts "#{item['sender_name']} (default: #{item['is_default'] == 1})"
    end
  else
    puts "No senders found!"
  end
elsif response_json['message']
  puts "Error: #{response_json['message']}"
else
  puts "Error code: #{response.code}"
end
