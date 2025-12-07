$PBExportHeader$n_sms_send.sru
forward
global type n_sms_send from n_sms_gateway
end type
end forward

global type n_sms_send from n_sms_gateway autoinstantiate
end type

type variables
// Send Result Variables
Public Boolean ib_success = False
Public String is_job_id = ""
Public String is_response = ""
end variables

forward prototypes
public function integer of_send_sms (string as_numbers, string as_message, string as_sender)
public function integer of_send_sms_single (string as_number, string as_message, string as_sender)
public function string of_build_json_body (string as_numbers[], string as_message, string as_sender)
end prototypes

public function integer of_send_sms (string as_numbers, string as_message, string as_sender);// Send SMS to multiple numbers (comma separated)
//
// Parameters:
//   as_numbers: Phone numbers comma separated (e.g. "966500000000,966500000001")
//   as_message: Message text
//   as_sender: Sender name (must be approved by 4jawaly.com)
//
// Return:
//   1 = Success
//  -1 = Failure

String ls_body, ls_response
String ls_numbers[]
Long ll_count, ll_i
String ls_temp

// Split numbers
ll_count = 1
ls_temp = as_numbers
Do While Pos(ls_temp, ",") > 0
    ls_numbers[ll_count] = Trim(Left(ls_temp, Pos(ls_temp, ",") - 1))
    ls_temp = Mid(ls_temp, Pos(ls_temp, ",") + 1)
    ll_count++
Loop
ls_numbers[ll_count] = Trim(ls_temp)

// Build JSON body
ls_body = of_build_json_body(ls_numbers, as_message, as_sender)

// Send Request
ls_response = of_http_request("POST", "account/area/sms/send", ls_body)
is_response = ls_response

// Parse Response
If Pos(Lower(ls_response), '"code":200') > 0 Or Pos(Lower(ls_response), '"code": 200') > 0 Then
    ib_success = True
    
    // Extract job_id
    Long ll_start, ll_end
    ll_start = Pos(ls_response, '"job_id"')
    If ll_start > 0 Then
        ll_start = Pos(ls_response, ":", ll_start) + 1
        ll_end = Pos(ls_response, ",", ll_start)
        If ll_end = 0 Then ll_end = Pos(ls_response, "}", ll_start)
        is_job_id = Trim(Mid(ls_response, ll_start, ll_end - ll_start))
        is_job_id = Replace(is_job_id, '"', '')
    End If
    
    Return 1
Else
    ib_success = False
    is_last_error = ls_response
    Return -1
End If

end function

public function integer of_send_sms_single (string as_number, string as_message, string as_sender);// Send SMS to a single number
//
// Parameters:
//   as_number: Phone number (e.g. "966500000000")
//   as_message: Message text
//   as_sender: Sender name (must be approved by 4jawaly.com)
//
// Return:
//   1 = Success
//  -1 = Failure

Return of_send_sms(as_number, as_message, as_sender)

end function

public function string of_build_json_body (string as_numbers[], string as_message, string as_sender);// Build JSON body for sending SMS

String ls_json, ls_numbers_json
Long ll_i, ll_upper

ll_upper = UpperBound(as_numbers)

// Build numbers array
ls_numbers_json = "["
For ll_i = 1 To ll_upper
    If ll_i > 1 Then ls_numbers_json += ","
    ls_numbers_json += '"' + as_numbers[ll_i] + '"'
Next
ls_numbers_json += "]"

// Build complete JSON
ls_json = '{'
ls_json += '"messages": ['
ls_json += '{'
ls_json += '"text": "' + as_message + '",'
ls_json += '"numbers": ' + ls_numbers_json + ','
ls_json += '"sender": "' + as_sender + '"'
ls_json += '}'
ls_json += ']'
ls_json += '}'

Return ls_json

end function

on n_sms_send.create
call super::create
end on

on n_sms_send.destroy
call super::destroy
end on
