# 4Jawaly SMS Gateway - PowerBuilder

A PowerBuilder library for integrating with 4Jawaly SMS Gateway.

## Files

| File | Description |
|------|-------------|
| `n_sms_gateway.sru` | Base class for API connection |
| `n_sms_send.sru` | Send SMS messages |
| `n_sms_senders.sru` | Get sender names |
| `n_sms_balance.sru` | Get balance and packages |

## Setup

### 1. Add Files to Project
Import the `.sru` files into your PowerBuilder project.

### 2. Get API Credentials
Get your `API Key` and `API Secret` from your account at [4jawaly.com](https://4jawaly.com)

## Usage Examples

### Send SMS

```powerscript
// Create send object
n_sms_send ln_sms
ln_sms = CREATE n_sms_send

// Set credentials
ln_sms.of_set_credentials("YOUR_API_KEY", "YOUR_API_SECRET")

// Send SMS to single number
Integer li_result
li_result = ln_sms.of_send_sms_single("966500000000", "Hello, this is a test message", "SENDER_NAME")

If li_result = 1 Then
    MessageBox("Success", "Message sent successfully!~r~nJob ID: " + ln_sms.is_job_id)
Else
    MessageBox("Error", "Failed to send: " + ln_sms.of_get_last_error())
End If

// Send SMS to multiple numbers
li_result = ln_sms.of_send_sms("966500000000,966500000001,966500000002", "Bulk message", "SENDER_NAME")

DESTROY ln_sms
```

### Get Sender Names

```powerscript
// Create senders object
n_sms_senders ln_senders
ln_senders = CREATE n_sms_senders

// Set credentials
ln_senders.of_set_credentials("YOUR_API_KEY", "YOUR_API_SECRET")

// Get all active sender names
Integer li_count
li_count = ln_senders.of_get_sender_names()

If li_count > 0 Then
    Long ll_i
    String ls_list
    
    For ll_i = 1 To li_count
        ls_list += String(ll_i) + ". " + ln_senders.is_sender_names[ll_i] + "~r~n"
    Next
    
    MessageBox("Sender Names", ls_list)
Else
    MessageBox("Error", "Failed to get sender names: " + ln_senders.of_get_last_error())
End If

// Get with filtering
// Status: 1 = active, 2 = inactive
li_count = ln_senders.of_get_sender_names_filtered(1, "", 1, 50)

DESTROY ln_senders
```

### Get Balance

```powerscript
// Create balance object
n_sms_balance ln_balance
ln_balance = CREATE n_sms_balance

// Set credentials
ln_balance.of_set_credentials("YOUR_API_KEY", "YOUR_API_SECRET")

// Get balance
Integer li_count
li_count = ln_balance.of_get_balance()

If li_count > 0 Then
    String ls_info
    ls_info = "Total Points: " + String(ln_balance.id_total_points) + "~r~n"
    ls_info += "Remaining Points: " + String(ln_balance.id_current_points) + "~r~n"
    ls_info += "Number of Packages: " + String(ln_balance.il_package_count) + "~r~n~r~n"
    
    // Package details
    Long ll_i
    For ll_i = 1 To li_count
        ls_info += "Package " + String(ll_i) + ":~r~n"
        ls_info += "  - Points: " + String(ln_balance.id_package_points[ll_i]) + "~r~n"
        ls_info += "  - Remaining: " + String(ln_balance.id_package_current[ll_i]) + "~r~n"
        ls_info += "  - Expires: " + ln_balance.is_package_expire[ll_i] + "~r~n"
    Next
    
    MessageBox("Balance", ls_info)
Else
    MessageBox("Error", "Failed to get balance: " + ln_balance.of_get_last_error())
End If

DESTROY ln_balance
```

## Important Notes

1. **Sender Name**: The sender name must be pre-approved by 4jawaly.com
2. **Number Format**: Use international format without + sign (e.g. 966500000000)
3. **Security**: Do not hardcode API credentials in your code, use external config file
4. **HTTP Object**: The library uses `MSXML2.ServerXMLHTTP.6.0` for connections

## API Endpoints

| Function | Endpoint |
|----------|----------|
| Send SMS | `POST /account/area/sms/send` |
| Sender Names | `GET /account/area/senders` |
| Balance | `GET /account/area/packages` |

## Support

For technical support, contact [info@4jawaly.com](mailto:info@4jawaly.com)

## License

MIT License
