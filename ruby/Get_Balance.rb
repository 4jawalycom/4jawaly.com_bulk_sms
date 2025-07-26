require 'net/http'
require 'uri'
require 'json'
require 'base64'

app_id = "API_KEY"
app_sec = "API_SECRET"
app_hash = Base64.strict_encode64("#{app_id}:#{app_sec}")

base_url = "https://api-sms.4jawaly.com/api/v1/"

query = {
  "is_active" => 1,
  "order_by" => "id",
  "order_by_type" => "desc",
  "page" => 1,
  "page_size" => 10,
  "return_collection" => 1
}

url = URI.parse(base_url + "account/area/me/packages?" + URI.encode_www_form(query))

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
  # إذا كان فيه total_balance اطبعه
  if response_json['total_balance']
    puts "Your balance: #{response_json['total_balance']}"
  elsif response_json['collection'] && !response_json['collection'].empty?
    puts "Packages collection:"
    puts JSON.pretty_generate(response_json['collection'])
  else
    puts "No balance or package data found!"
  end
else
  puts response_json['message'] || "Unknown error"
end
