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
	appID := "Api key"
	appSec := "Api Secret"
	appHash := base64.StdEncoding.EncodeToString([]byte(appID + ":" + appSec))
	baseURL := "https://api-sms.4jawaly.com/api/v1/"

	query := make(map[string]string) // Define the query parameters here if needed

	url := baseURL + "account/area/me/packages?"
	for k, v := range query {
		url += fmt.Sprintf("%s=%s&", k, v)
	}
	url = strings.TrimRight(url, "&")

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

	responseJSON := make(map[string]interface{})
	err = json.Unmarshal(body, &responseJSON)
	if err != nil {
		fmt.Println("Error unmarshalling response JSON:", err)
		return
	}

	code := int(responseJSON["code"].(float64))
	if code == 200 {
		fmt.Println("Your balance:", responseJSON["total_balance"].(float64))
	} else {
		fmt.Println(responseJSON["message"].(string))
	}
}
