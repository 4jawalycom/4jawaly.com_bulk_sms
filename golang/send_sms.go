package main

import (
    "encoding/base64"
    "encoding/json"
    "fmt"
    "net/http"
    "strings"
)

func main() {
    appID := "Api Key "
    appSecret := "Api secret"
    appHash := base64.StdEncoding.EncodeToString([]byte(appID + ":" + appSecret))

    messages := map[string]interface{}{
        "messages": []map[string]interface{}{
            {
                "text":    "نص الرسالة هنا",
                "numbers": []string{"9665000000"},
                "sender":  "test",
            },
        },
    }

    jsonStr, _ := json.Marshal(messages)
    req, _ := http.NewRequest("POST", "https://api-sms.4jawaly.com/api/v1/account/area/sms/send", strings.NewReader(string(jsonStr)))
    req.Header.Set("Content-Type", "application/json")
    req.Header.Set("Accept", "application/json")
    req.Header.Set("Authorization", "Basic "+appHash)

    client := &http.Client{}
    resp, err := client.Do(req)
    if err != nil {
        fmt.Println(err)
        return
    }
    defer resp.Body.Close()

    var responseJSON map[string]interface{}
    err = json.NewDecoder(resp.Body).Decode(&responseJSON)
    if err != nil {
        fmt.Println(err)
        return
    }

    switch resp.StatusCode {
    case http.StatusOK:
        if errText, ok := responseJSON["messages"].([]interface{})[0].(map[string]interface{})["err_text"]; ok {
            fmt.Println(errText)
        } else {
            fmt.Printf("تم الارسال بنجاح job id: %v\n", responseJSON["job_id"])
        }
    case http.StatusBadRequest:
        fmt.Println(responseJSON["message"])
    case http.StatusUnprocessableEntity:
        fmt.Println("نص الرسالة فارغ")
    default:
        fmt.Printf("محظور بواسطة كلاودفلير. Status code: %v\n", resp.StatusCode)
    }
}
