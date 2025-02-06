package main

import (
    "bytes"
    "encoding/base64"
    "encoding/json"
    "fmt"
    "net/http"
    "sync"
)

// SMSGateway - بوابة إرسال الرسائل | SMS Gateway | ایس ایم ایس گیٹ وے
type SMSGateway struct {
    APIKey    string
    APISecret string
    BaseURL   string
    Client    *http.Client
}

// SMSResult - نتيجة الإرسال | Sending result | بھیجنے کا نتیجہ
type SMSResult struct {
    Success      bool                `json:"success"`
    TotalSuccess int                 `json:"total_success"`
    TotalFailed  int                 `json:"total_failed"`
    JobIDs       []string            `json:"job_ids"`
    Errors       map[string][]string `json:"errors"`
}

// ChunkResult - نتيجة المجموعة | Chunk result | حصے کا نتیجہ
type ChunkResult struct {
    StatusCode int
    Response   map[string]interface{}
    Numbers    []string
    Error      error
}

// NewSMSGateway - إنشاء بوابة جديدة | Create new gateway | نیا گیٹ وے بنائیں
func NewSMSGateway(apiKey, apiSecret string) *SMSGateway {
    return &SMSGateway{
        APIKey:    apiKey,
        APISecret: apiSecret,
        BaseURL:   "https://api-sms.4jawaly.com/api/v1",
        Client:    &http.Client{},
    }
}

// getAuthHeader - الحصول على ترويسة المصادقة | Get auth header | تصدیق ہیڈر حاصل کریں
func (g *SMSGateway) getAuthHeader() string {
    auth := base64.StdEncoding.EncodeToString([]byte(g.APIKey + ":" + g.APISecret))
    return "Basic " + auth
}

// chunkSlice - تقسيم المصفوفة | Split array | ایرے کو تقسیم کریں
func chunkSlice(slice []string, chunkSize int) [][]string {
    var chunks [][]string
    for i := 0; i < len(slice); i += chunkSize {
        end := i + chunkSize
        if end > len(slice) {
            end = len(slice)
        }
        chunks = append(chunks, slice[i:end])
    }
    return chunks
}

// sendChunk - إرسال مجموعة | Send chunk | حصہ بھیجیں
func (g *SMSGateway) sendChunk(message string, numbers []string, sender string) ChunkResult {
    payload := map[string]interface{}{
        "messages": []map[string]interface{}{
            {
                "text":    message,
                "numbers": numbers,
                "sender":  sender,
            },
        },
    }

    jsonData, err := json.Marshal(payload)
    if err != nil {
        return ChunkResult{Error: err, Numbers: numbers}
    }

    req, err := http.NewRequest("POST", g.BaseURL+"/account/area/sms/send", bytes.NewBuffer(jsonData))
    if err != nil {
        return ChunkResult{Error: err, Numbers: numbers}
    }

    req.Header.Set("Content-Type", "application/json")
    req.Header.Set("Accept", "application/json")
    req.Header.Set("Authorization", g.getAuthHeader())

    resp, err := g.Client.Do(req)
    if err != nil {
        return ChunkResult{Error: err, Numbers: numbers}
    }
    defer resp.Body.Close()

    var response map[string]interface{}
    if err := json.NewDecoder(resp.Body).Decode(&response); err != nil {
        return ChunkResult{Error: err, Numbers: numbers}
    }

    return ChunkResult{
        StatusCode: resp.StatusCode,
        Response:   response,
        Numbers:    numbers,
    }
}

// SendSMS - إرسال رسائل SMS | Send SMS messages | ایس ایم ایس پیغامات بھیجیں
func (g *SMSGateway) SendSMS(message string, numbers []string, sender string) SMSResult {
    // تهيئة النتيجة | Initialize result | نتیجہ کو شروع کریں
    result := SMSResult{
        Success:      true,
        TotalSuccess: 0,
        TotalFailed:  0,
        JobIDs:       make([]string, 0),
        Errors:       make(map[string][]string),
    }

    // تحديد حجم المجموعة | Determine chunk size | حصے کا سائز متعین کریں
    var chunkSize int
    switch {
    case len(numbers) <= 5:
        chunkSize = len(numbers)
    case len(numbers) <= 100:
        chunkSize = (len(numbers) + 4) / 5
    default:
        chunkSize = 100
    }

    // تقسيم الأرقام | Split numbers | نمبروں کو تقسیم کریں
    chunks := chunkSlice(numbers, chunkSize)

    // إنشاء قناة للنتائج | Create results channel | نتائج کے لئے چینل بنائیں
    resultsChan := make(chan ChunkResult, len(chunks))
    var wg sync.WaitGroup

    // إرسال كل مجموعة بشكل متوازي | Send each chunk in parallel | ہر حصہ متوازی طور پر بھیجیں
    for _, chunk := range chunks {
        wg.Add(1)
        go func(chunk []string) {
            defer wg.Done()
            resultsChan <- g.sendChunk(message, chunk, sender)
        }(chunk)
    }

    // انتظار انتهاء جميع العمليات | Wait for all operations | تمام آپریشنز کا انتظار کریں
    go func() {
        wg.Wait()
        close(resultsChan)
    }()

    // معالجة النتائج | Process results | نتائج کی پروسیسنگ
    for chunkResult := range resultsChan {
        if chunkResult.Error != nil {
            result.TotalFailed += len(chunkResult.Numbers)
            errMsg := chunkResult.Error.Error()
            result.Errors[errMsg] = append(result.Errors[errMsg], chunkResult.Numbers...)
            continue
        }

        if chunkResult.StatusCode == http.StatusOK {
            messages := chunkResult.Response["messages"].([]interface{})
            if errText, ok := messages[0].(map[string]interface{})["err_text"]; ok {
                result.TotalFailed += len(chunkResult.Numbers)
                errMsg := errText.(string)
                result.Errors[errMsg] = append(result.Errors[errMsg], chunkResult.Numbers...)
            } else {
                result.TotalSuccess += len(chunkResult.Numbers)
                if jobID, ok := chunkResult.Response["job_id"].(string); ok {
                    result.JobIDs = append(result.JobIDs, jobID)
                }
            }
        } else {
            result.TotalFailed += len(chunkResult.Numbers)
            errMsg := fmt.Sprintf("HTTP Error: %d", chunkResult.StatusCode)
            result.Errors[errMsg] = append(result.Errors[errMsg], chunkResult.Numbers...)
        }
    }

    result.Success = result.TotalFailed == 0
    return result
}

func main() {
    // مثال على الاستخدام | Usage example | استعمال کی مثال
    gateway := NewSMSGateway("your_api_key", "your_api_secret")
    
    result := gateway.SendSMS(
        "اختبار الإرسال المتوازي | Parallel sending test | متوازی بھیجنے کا ٹیسٹ",
        []string{"966500000001", "966500000002", "966500000003"},
        "4jawaly",
    )
    
    // طباعة النتيجة | Print result | نتیجہ پرنٹ کریں
    jsonResult, _ := json.MarshalIndent(result, "", "  ")
    fmt.Println(string(jsonResult))
}
