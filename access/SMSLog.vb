Option Compare Database

' تعريف المتغيرات العامة
Private Const BASE_URL As String = "https://api-sms.4jawaly.com/api/v1/"
Private m_AppID As String
Private m_AppSecret As String

' دالة تهيئة الخدمة
Public Sub InitializeService(AppID As String, AppSecret As String)
    m_AppID = AppID
    m_AppSecret = AppSecret
End Sub

' دالة إرسال الرسائل
Public Function SendSMS(Message As String, Numbers As String, Sender As String) As String
    On Error GoTo ErrorHandler
    
    Dim objHTTP As Object
    Set objHTTP = CreateObject("MSXML2.ServerXMLHTTP")
    
    ' تجهيز عنوان URL
    Dim URL As String
    URL = BASE_URL & "account/area/sms/send"
    
    ' تجهيز البيانات
    Dim JsonData As String
    JsonData = "{""messages"":[{""text"":""" & Message & """,""numbers"":[""" & Replace(Numbers, ",", """,""") & """],""sender"":""" & Sender & """}]}"
    
    ' إرسال الطلب
    With objHTTP
        .Open "POST", URL, False
        .setRequestHeader "Content-Type", "application/json"
        .setRequestHeader "Accept", "application/json"
        .setRequestHeader "Authorization", "Basic " & Base64Encode(m_AppID & ":" & m_AppSecret)
        .send JsonData
        
        ' تحليل الاستجابة
        If .Status = 200 Then
            SendSMS = .responseText
            ' تسجيل في قاعدة البيانات
            LogSMSToDatabase Message, Numbers, Sender, .responseText
        Else
            SendSMS = "Error: " & .Status & " - " & .responseText
        End If
    End With
    
    Exit Function
    
ErrorHandler:
    SendSMS = "Error: " & Err.Description
End Function

' دالة التحقق من الرصيد
Public Function CheckBalance() As String
    On Error GoTo ErrorHandler
    
    Dim objHTTP As Object
    Set objHTTP = CreateObject("MSXML2.ServerXMLHTTP")
    
    ' تجهيز عنوان URL
    Dim URL As String
    URL = BASE_URL & "account/area/me/packages"
    
    ' إرسال الطلب
    With objHTTP
        .Open "GET", URL, False
        .setRequestHeader "Accept", "application/json"
        .setRequestHeader "Content-Type", "application/json"
        .setRequestHeader "Authorization", "Basic " & Base64Encode(m_AppID & ":" & m_AppSecret)
        .send
        
        If .Status = 200 Then
            CheckBalance = .responseText
        Else
            CheckBalance = "Error: " & .Status & " - " & .responseText
        End If
    End With
    
    Exit Function
    
ErrorHandler:
    CheckBalance = "Error: " & Err.Description
End Function

' دالة جلب المرسلين
Public Function GetSenders() As String
    On Error GoTo ErrorHandler
    
    Dim objHTTP As Object
    Set objHTTP = CreateObject("MSXML2.ServerXMLHTTP")
    
    ' تجهيز عنوان URL
    Dim URL As String
    URL = BASE_URL & "account/area/senders"
    
    ' إرسال الطلب
    With objHTTP
        .Open "GET", URL, False
        .setRequestHeader "Accept", "application/json"
        .setRequestHeader "Content-Type", "application/json"
        .setRequestHeader "Authorization", "Basic " & Base64Encode(m_AppID & ":" & m_AppSecret)
        .send
        
        If .Status = 200 Then
            GetSenders = .responseText
        Else
            GetSenders = "Error: " & .Status & " - " & .responseText
        End If
    End With
    
    Exit Function
    
ErrorHandler:
    GetSenders = "Error: " & Err.Description
End Function

' دالة مساعدة لتشفير Base64
Private Function Base64Encode(ByVal Text As String) As String
    Dim arrData() As Byte
    arrData = StrConv(Text, vbFromUnicode)
    
    Dim objXML As Object
    Dim objNode As Object
    
    Set objXML = CreateObject("MSXML2.DOMDocument")
    Set objNode = objXML.createElement("b64")
    
    objNode.DataType = "bin.base64"
    objNode.nodeTypedValue = arrData
    Base64Encode = objNode.Text
    
    Set objNode = Nothing
    Set objXML = Nothing
End Function

' دالة تسجيل الرسائل في قاعدة البيانات
Private Sub LogSMSToDatabase(Message As String, Numbers As String, Sender As String, Response As String)
    CurrentDb.Execute "INSERT INTO tblSMSLog (Message, Numbers, Sender, Response, SendDate) " & _
                     "VALUES ('" & Replace(Message, "'", "''") & "', '" & _
                              Replace(Numbers, "'", "''") & "', '" & _
                              Replace(Sender, "'", "''") & "', '" & _
                              Replace(Response, "'", "''") & "', #" & _
                              Now() & "#)"
End Sub
