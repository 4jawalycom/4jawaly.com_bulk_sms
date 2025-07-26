package main

import (
	"encoding/base64"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"net/http"
	"strings"
)

func main() {
	appID := "API_KEY"      // ضع هنا API KEY الخاص بك
	appSec := "API_SECRET"  // ضع هنا API SECRET الخاص بك
	appHash := base64.StdEncoding.EncodeToString([]byte(appID + ":" + appSec))
	baseURL := "https://api-sms.4jawaly.com/api/v1/"

	// لو تحب تعدل الفلاتر
	query := map[string]string{
		"page_size":         "10",
		"page":              "1",
		"status":            "1", // active only
		"return_collection": "1",
	}
	var params []string
	for k, v := range query {
		params = append(params, fmt.Sprintf("%s=%s", k, v))
	}
	url := baseURL + "account/area/senders"
	if len(params) > 0 {
		url += "?" + strings.Join(params, "&")
	}

	headers := map[string]string{
		"Accept":        "application/json",
		"Content-Type":  "application/json",
		"Authorization": "Basic " + appHash,
	}

	client := &http.Client{}
	req, err := http.NewRequest("GET", url, nil)
	if err != nil {
		fmt.Println("Error creating request:", err)
		return
	}
	for k, v := range headers {
		req.Header.Set(k, v)
	}

	resp, err := client.Do(req)
	if err != nil {
		fmt.Println("Error executing request:", err)
		return
	}
	defer resp.Body.Close()

	if resp.StatusCode != 200 {
		fmt.Println("Error code:", resp.StatusCode)
		return
	}

	body, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		fmt.Println("Error reading response body:", err)
		return
	}

	var responseJSON map[string]interface{}
	err = json.Unmarshal(body, &responseJSON)
	if err != nil {
		fmt.Println("Error unmarshalling response JSON:", err)
		return
	}

	// استخرج أسماء المرسلين مع الحقل الافتراضي
	if items, ok := responseJSON["items"].([]interface{}); ok && len(items) > 0 {
		type Sender struct {
			SenderName string `json:"sender_name"`
			IsDefault  bool   `json:"is_default"`
		}
		var senders []Sender
		for _, v := range items {
			row := v.(map[string]interface{})
			name, _ := row["sender_name"].(string)
			isDefault := false
			if def, exists := row["is_default"]; exists {
				if defNum, ok := def.(float64); ok && defNum == 1 {
					isDefault = true
				}
			}
			senders = append(senders, Sender{
				SenderName: name,
				IsDefault:  isDefault,
			})
		}
		result, _ := json.MarshalIndent(senders, "", "  ")
		fmt.Println(string(result))
	} else if msg, ok := responseJSON["message"].(string); ok {
		fmt.Println("API message:", msg)
	} else {
		fmt.Println("Unexpected response:", string(body))
	}
}
