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
	appID := "API_KEY"
	appSec := "API_SECRET"
	appHash := base64.StdEncoding.EncodeToString([]byte(appID + ":" + appSec))
	baseURL := "https://api-sms.4jawaly.com/api/v1/"

	query := map[string]string{
		"is_active":          "1",
		"order_by":           "id",
		"order_by_type":      "desc",
		"page":               "1",
		"page_size":          "10",
		"return_collection":  "1",
	}
	// Build URL with query params
	var params []string
	for k, v := range query {
		params = append(params, fmt.Sprintf("%s=%s", k, v))
	}
	url := baseURL + "account/area/me/packages"
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

	// Safe parsing
	code, ok := responseJSON["code"].(float64)
	if !ok {
		fmt.Println("No code field in response")
		return
	}
	if int(code) == 200 {
		if balance, ok := responseJSON["total_balance"].(float64); ok {
			fmt.Printf("Your balance: %.0f\n", balance)
		} else if collection, ok := responseJSON["collection"].([]interface{}); ok {
			b, _ := json.MarshalIndent(collection, "", "  ")
			fmt.Println("Your packages collection:")
			fmt.Println(string(b))
		} else {
			fmt.Println("No total_balance or collection in response!")
		}
	} else if msg, ok := responseJSON["message"].(string); ok {
		fmt.Println(msg)
	} else {
		fmt.Println("Unexpected response:", string(body))
	}
}
