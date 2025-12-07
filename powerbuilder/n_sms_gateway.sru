$PBExportHeader$n_sms_gateway.sru
forward
global type n_sms_gateway from nonvisualobject
end type
end forward

global type n_sms_gateway from nonvisualobject autoinstantiate
end type

type variables
// API Configuration
Private String is_api_key = ""
Private String is_api_secret = ""
Private String is_base_url = "https://api-sms.4jawaly.com/api/v1/"
Private String is_default_sender = ""
Private Integer ii_timeout = 30

// HTTP Object
Private OLEObject iole_http
Private Boolean ib_initialized = False

// Last Error
Public String is_last_error = ""
end variables

forward prototypes
public function integer of_initialize ()
public function string of_base64_encode (string as_text)
public function string of_http_request (string as_method, string as_endpoint, string as_body)
public subroutine of_set_credentials (string as_api_key, string as_api_secret)
public subroutine of_set_sender (string as_sender)
public function string of_get_last_error ()
end prototypes

public function integer of_initialize ();// Initialize HTTP Object

Try
    iole_http = CREATE OLEObject
    iole_http.ConnectToNewObject("MSXML2.ServerXMLHTTP.6.0")
    ib_initialized = True
    Return 1
Catch (OLERuntimeError ex)
    is_last_error = "Failed to initialize HTTP object: " + ex.Description
    Return -1
End Try

end function

public function string of_base64_encode (string as_text);// Base64 Encode

String ls_result
OLEObject lole_dom, lole_node

Try
    lole_dom = CREATE OLEObject
    lole_dom.ConnectToNewObject("MSXML2.DOMDocument.6.0")
    
    lole_node = lole_dom.createElement("b64")
    lole_node.dataType = "bin.base64"
    lole_node.nodeTypedValue = Blob(as_text, EncodingUTF8!)
    ls_result = String(lole_node.text)
    
    DESTROY lole_node
    DESTROY lole_dom
Catch (OLERuntimeError ex)
    ls_result = ""
    is_last_error = "Failed to encode Base64: " + ex.Description
End Try

Return ls_result

end function

public function string of_http_request (string as_method, string as_endpoint, string as_body);// Send HTTP Request

String ls_response, ls_url, ls_auth

If Not ib_initialized Then
    If of_initialize() < 0 Then
        Return '{"error": "' + is_last_error + '"}'
    End If
End If

ls_url = is_base_url + as_endpoint

// Create Authorization Token
ls_auth = "Basic " + of_base64_encode(is_api_key + ":" + is_api_secret)

Try
    iole_http.open(as_method, ls_url, False)
    iole_http.setRequestHeader("Content-Type", "application/json")
    iole_http.setRequestHeader("Accept", "application/json")
    iole_http.setRequestHeader("Authorization", ls_auth)
    iole_http.setTimeouts(ii_timeout * 1000, ii_timeout * 1000, ii_timeout * 1000, ii_timeout * 1000)
    
    If as_method = "POST" And Len(as_body) > 0 Then
        iole_http.send(as_body)
    Else
        iole_http.send()
    End If
    
    ls_response = String(iole_http.responseText)
    
Catch (OLERuntimeError ex)
    ls_response = '{"error": "' + ex.Description + '"}'
    is_last_error = ex.Description
End Try

Return ls_response

end function

public subroutine of_set_credentials (string as_api_key, string as_api_secret);// Set API Credentials

is_api_key = as_api_key
is_api_secret = as_api_secret

end subroutine

public subroutine of_set_sender (string as_sender);// Set Default Sender Name

is_default_sender = as_sender

end subroutine

public function string of_get_last_error ();// Get Last Error

Return is_last_error

end function

on n_sms_gateway.create
call super::create
TriggerEvent( this, "constructor" )
end on

on n_sms_gateway.destroy
TriggerEvent( this, "destructor" )
If IsValid(iole_http) Then
    iole_http.DisconnectObject()
    DESTROY iole_http
End If
call super::destroy
end on
