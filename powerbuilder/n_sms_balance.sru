$PBExportHeader$n_sms_balance.sru
forward
global type n_sms_balance from n_sms_gateway
end type
end forward

global type n_sms_balance from n_sms_gateway autoinstantiate
end type

type variables
// Balance Variables
Public Decimal{2} id_total_points = 0
Public Decimal{2} id_current_points = 0
Public Long il_package_count = 0
Public String is_response = ""

// Package Details
Public Long il_package_ids[]
Public Decimal{2} id_package_points[]
Public Decimal{2} id_package_current[]
Public String is_package_expire[]
end variables

forward prototypes
public function integer of_get_balance ()
public function integer of_get_balance_filtered (integer ai_is_active, string as_order_by, string as_order_type, integer ai_page, integer ai_page_size)
public function integer of_parse_balance (string as_json)
end prototypes

public function integer of_get_balance ();// Get Account Balance
//
// Return:
//   Number of packages
//  -1 = Failure

Return of_get_balance_filtered(1, "id", "desc", 1, 100)

end function

public function integer of_get_balance_filtered (integer ai_is_active, string as_order_by, string as_order_type, integer ai_page, integer ai_page_size);// Get balance with filtering
//
// Parameters:
//   ai_is_active: Active packages only (1 = yes, 0 = all)
//   as_order_by: Order by (id, package_points, current_points, expire_at)
//   as_order_type: Order type (asc, desc)
//   ai_page: Page number
//   ai_page_size: Items per page
//
// Return:
//   Number of packages
//  -1 = Failure

String ls_endpoint, ls_response

// Build endpoint with parameters
ls_endpoint = "account/area/packages?"
ls_endpoint += "page=" + String(ai_page)
ls_endpoint += "&page_size=" + String(ai_page_size)
ls_endpoint += "&is_active=" + String(ai_is_active)
ls_endpoint += "&order_by=" + as_order_by
ls_endpoint += "&order_by_type=" + as_order_type

// Send Request
ls_response = of_http_request("GET", ls_endpoint, "")
is_response = ls_response

// Parse Response
If Pos(Lower(ls_response), '"error"') > 0 Then
    is_last_error = ls_response
    Return -1
End If

Return of_parse_balance(ls_response)

end function

public function integer of_parse_balance (string as_json);// Parse JSON to extract balance data

String ls_temp, ls_value
Long ll_start, ll_end, ll_count
Decimal ld_total, ld_current

// Reset variables
il_package_count = 0
id_total_points = 0
id_current_points = 0
ld_total = 0
ld_current = 0

ls_temp = as_json
ll_count = 0

// Search for packages
Do While Pos(ls_temp, '"package_points"') > 0
    ll_count++
    
    // Extract package_points
    ll_start = Pos(ls_temp, '"package_points"')
    ll_start = Pos(ls_temp, ":", ll_start) + 1
    ll_end = Pos(ls_temp, ",", ll_start)
    If ll_end = 0 Then ll_end = Pos(ls_temp, "}", ll_start)
    ls_value = Trim(Mid(ls_temp, ll_start, ll_end - ll_start))
    ls_value = Replace(ls_value, '"', '')
    id_package_points[ll_count] = Dec(ls_value)
    ld_total += Dec(ls_value)
    
    // Extract current_points
    ll_start = Pos(ls_temp, '"current_points"')
    If ll_start > 0 Then
        ll_start = Pos(ls_temp, ":", ll_start) + 1
        ll_end = Pos(ls_temp, ",", ll_start)
        If ll_end = 0 Then ll_end = Pos(ls_temp, "}", ll_start)
        ls_value = Trim(Mid(ls_temp, ll_start, ll_end - ll_start))
        ls_value = Replace(ls_value, '"', '')
        id_package_current[ll_count] = Dec(ls_value)
        ld_current += Dec(ls_value)
    End If
    
    // Extract expire_at
    ll_start = Pos(ls_temp, '"expire_at"')
    If ll_start > 0 Then
        ll_start = Pos(ls_temp, ":", ll_start) + 1
        Do While Mid(ls_temp, ll_start, 1) = " " Or Mid(ls_temp, ll_start, 1) = '"'
            ll_start++
        Loop
        ll_end = Pos(ls_temp, '"', ll_start)
        If ll_end = 0 Then ll_end = Pos(ls_temp, ",", ll_start)
        is_package_expire[ll_count] = Mid(ls_temp, ll_start, ll_end - ll_start)
    End If
    
    // Extract id
    ll_start = Pos(ls_temp, '"id"')
    If ll_start > 0 Then
        ll_start = Pos(ls_temp, ":", ll_start) + 1
        ll_end = Pos(ls_temp, ",", ll_start)
        If ll_end = 0 Then ll_end = Pos(ls_temp, "}", ll_start)
        ls_value = Trim(Mid(ls_temp, ll_start, ll_end - ll_start))
        il_package_ids[ll_count] = Long(ls_value)
    End If
    
    // Move to next package
    ll_start = Pos(ls_temp, "}")
    If ll_start > 0 Then
        ls_temp = Mid(ls_temp, ll_start + 1)
    Else
        Exit
    End If
Loop

il_package_count = ll_count
id_total_points = ld_total
id_current_points = ld_current

Return ll_count

end function

on n_sms_balance.create
call super::create
end on

on n_sms_balance.destroy
call super::destroy
end on
