$PBExportHeader$n_sms_senders.sru
forward
global type n_sms_senders from n_sms_gateway
end type
end forward

global type n_sms_senders from n_sms_gateway autoinstantiate
end type

type variables
// Sender Names Variables
Public String is_sender_names[]
Public Long il_sender_count = 0
Public String is_response = ""
end variables

forward prototypes
public function integer of_get_sender_names ()
public function integer of_get_sender_names_filtered (integer ai_status, string as_search, integer ai_page, integer ai_page_size)
public function integer of_parse_senders (string as_json)
end prototypes

public function integer of_get_sender_names ();// Get all sender names
//
// Return:
//   Number of sender names
//  -1 = Failure

Return of_get_sender_names_filtered(1, "", 1, 100)

end function

public function integer of_get_sender_names_filtered (integer ai_status, string as_search, integer ai_page, integer ai_page_size);// Get sender names with filtering
//
// Parameters:
//   ai_status: Status (1 = active, 2 = inactive, 0 = all)
//   as_search: Search by sender name
//   ai_page: Page number
//   ai_page_size: Items per page
//
// Return:
//   Number of sender names
//  -1 = Failure

String ls_endpoint, ls_response

// Build endpoint with parameters
ls_endpoint = "account/area/senders?"
ls_endpoint += "page=" + String(ai_page)
ls_endpoint += "&page_size=" + String(ai_page_size)

If ai_status > 0 Then
    ls_endpoint += "&status=" + String(ai_status)
End If

If Len(Trim(as_search)) > 0 Then
    ls_endpoint += "&sender_name=" + as_search
End If

// Send Request
ls_response = of_http_request("GET", ls_endpoint, "")
is_response = ls_response

// Parse Response
If Pos(Lower(ls_response), '"error"') > 0 Then
    is_last_error = ls_response
    Return -1
End If

Return of_parse_senders(ls_response)

end function

public function integer of_parse_senders (string as_json);// Parse JSON to extract sender names

String ls_temp, ls_sender
Long ll_start, ll_end, ll_count

// Reset array
il_sender_count = 0

// Search for sender_name in JSON
ls_temp = as_json
ll_count = 0

Do While Pos(ls_temp, '"sender_name"') > 0
    ll_start = Pos(ls_temp, '"sender_name"')
    ll_start = Pos(ls_temp, ":", ll_start) + 1
    
    // Skip spaces and quotes
    Do While Mid(ls_temp, ll_start, 1) = " " Or Mid(ls_temp, ll_start, 1) = '"'
        ll_start++
    Loop
    
    ll_end = Pos(ls_temp, '"', ll_start)
    If ll_end = 0 Then ll_end = Pos(ls_temp, ",", ll_start)
    If ll_end = 0 Then ll_end = Pos(ls_temp, "}", ll_start)
    
    ls_sender = Mid(ls_temp, ll_start, ll_end - ll_start)
    
    If Len(Trim(ls_sender)) > 0 Then
        ll_count++
        is_sender_names[ll_count] = ls_sender
    End If
    
    ls_temp = Mid(ls_temp, ll_end + 1)
Loop

il_sender_count = ll_count

Return ll_count

end function

on n_sms_senders.create
call super::create
end on

on n_sms_senders.destroy
call super::destroy
end on
