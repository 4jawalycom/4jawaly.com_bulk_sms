
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization
Imports API_Example_VB.API_Example.Models
Imports API_Example_VB.API_Example_VB.Models
Imports API_Example_VB.API_Example_VB.Models.Balance
Imports Newtonsoft.Json

Namespace API_Example_VB
    Module _4jawaly
        Private UserAgent As String = "XXXXXXXXX" 'put your useragent here

        Public Function Send4jawaly(App_key As String, App_secret As String, SenderName As String, Mobiles As String, message As String) As String
            If App_key Is Nothing Then
                Return "key required"
            End If

            If App_secret Is Nothing Then
                Return "secret required"
            End If

            If SenderName Is Nothing Then
                Return "Sender required"
            End If

            If Mobiles Is Nothing Then
                Return "Mobile number required"
            End If

            If message Is Nothing Then
                Return "Message required"
            End If
            Try
                Dim messages As New List(Of message)()
                Dim mobilesList As List(Of String) = Mobiles.Split(","c).ToList()
                messages.Add(New message() With {
                .text = message,
                .numbers = mobilesList
            })

                Dim senddata As New SendData()

                senddata.messages = messages
                senddata.globals = New Globals() With {
                .number_iso = "SA",
                .sender = SenderName
            }

                Dim url As String = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send"
                Dim credentials As String = App_key & ":" & App_secret

                Dim httpRequest As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
                httpRequest.Method = "POST"
                httpRequest.Accept = "application/json"
                httpRequest.ContentType = "application/json"
                httpRequest.Headers("Authorization") = "Basic " & Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials))

                Dim serializer As New JsonSerializer()
                Dim data As String = JsonConvert.SerializeObject(senddata)

                Using streamWriter As New StreamWriter(httpRequest.GetRequestStream())
                    streamWriter.Write(data)
                End Using

                Dim httpResponse As HttpWebResponse = DirectCast(httpRequest.GetResponse(), HttpWebResponse)
                Dim result As String = ""

                If CInt(httpResponse.StatusCode) = 200 Then
                    Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                        result = streamReader.ReadToEnd()
                    End Using

                    Dim myDeserializedClass As _4jawalyRoot = JsonConvert.DeserializeObject(Of _4jawalyRoot)(result)
                    If myDeserializedClass.messages.FirstOrDefault().err_text Is Nothing Then
                        Return "تم الإرسال بنجاح" & "  job_id: " & myDeserializedClass.job_id & "  Code: " & myDeserializedClass.code & "  Message: " & myDeserializedClass.message
                    Else
                        Return "لم يتم الارسال" & "  Message: " & myDeserializedClass.messages.First().err_text
                    End If
                ElseIf CInt(httpResponse.StatusCode) = 400 Then
                    Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                        result = streamReader.ReadToEnd()
                    End Using

                    Dim myDeserializedClass As _4jawalyRoot = JsonConvert.DeserializeObject(Of _4jawalyRoot)(result)

                    Return "Error 400 " & "  Message: " & myDeserializedClass.message
                ElseIf CInt(httpResponse.StatusCode) = 403 Then
                    Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                        result = streamReader.ReadToEnd()
                    End Using

                    Dim myDeserializedClass As _4jawalyRoot = JsonConvert.DeserializeObject(Of _4jawalyRoot)(result)
                    Return "Error 403 محضور Firewall" & "  Message: " & myDeserializedClass.message
                ElseIf httpResponse.StatusCode = HttpStatusCode.BadRequest OrElse httpResponse.StatusCode = HttpStatusCode.InternalServerError Then
                    Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                        result = streamReader.ReadToEnd()
                    End Using

                    Dim x As Response = JsonConvert.DeserializeObject(Of Response)(result)

                    Return "Code:" & x.code & ",Message:" & x.message
                Else
                    Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                        result = streamReader.ReadToEnd()
                    End Using

                    Dim x As _4jawalyRoot = JsonConvert.DeserializeObject(Of _4jawalyRoot)(result)
                    If x.code = 0 Then
                        Return "Error 0 " & "  Message: " & "No text in message"
                    ElseIf x.messages.FirstOrDefault().err_text Is Nothing Then
                        Return " Error Code: " & x.code & "  Message: " & x.message
                    Else
                        Return " HTTP_Code: " & CInt(httpResponse.StatusCode) & " Error Code: " & x.code & "  Message: " & x.message & "Messages: " & x.messages.First().err_text
                    End If
                End If
            Catch ex As WebException
                Dim errorResponse As HttpWebResponse = DirectCast(ex.Response, HttpWebResponse)
                Using reader As New StreamReader(errorResponse.GetResponseStream())
                    Dim errorText As String = reader.ReadToEnd()
                    ' Handle error message
                    Dim Response As _4jawalyRoot = JsonConvert.DeserializeObject(Of _4jawalyRoot)(errorText)
                    Return "Error code:" & Response.code & " Message: " & Response.message
                End Using
            End Try
        End Function

        Function GetSenders4jawaly(ByVal App_key As String, ByVal App_secret As String) As Sender
            Try
                Dim url = "https://api-sms.4jawaly.com/api/v1/account/area/senders"
                Dim credentials As String = App_key & ":" & App_secret
                Dim httpRequest = CType(WebRequest.Create(url), HttpWebRequest)
                httpRequest.Method = "GET"
                httpRequest.Accept = "application/json"
                httpRequest.UserAgent = UserAgent
                httpRequest.ContentType = "application/json"
                httpRequest.Headers("Authorization") = "Basic " & Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials))
                Dim httpResponse = CType(httpRequest.GetResponse(), HttpWebResponse)
                Dim result As String = ""

                If httpResponse.StatusCode = HttpStatusCode.OK Then

                    Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                        result = streamReader.ReadToEnd()
                    End Using
                End If

                Return JsonConvert.DeserializeObject(Of Sender)(result)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Function GetBalance(App_key As String, App_secret As String) As String
            If App_key Is Nothing Then
                Return "key required"
            End If

            If App_secret Is Nothing Then
                Return "secret required"
            End If

            Try
                Dim url As String = "https://api-sms.4jawaly.com/api/v1/account/area/me/packages?is_active=1&order_by=id&order_by_type=desc&page=1&page_size=10&return_collection=1"
                Dim credentials As String = App_key & ":" & App_secret
                Dim Username As New List(Of String)()

                Dim httpRequest As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
                httpRequest.Method = "GET"
                httpRequest.Accept = "application/json"
                'httpRequest.UserAgent = UserAgent;
                httpRequest.ContentType = "application/json"

                httpRequest.Headers("Authorization") = "Basic " & Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials))
                Dim httpResponse As HttpWebResponse = DirectCast(httpRequest.GetResponse(), HttpWebResponse)

                If CInt(httpResponse.StatusCode) = 200 Then
                    Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                        Dim responseData As String = streamReader.ReadToEnd()
                        Dim result = JsonConvert.DeserializeObject(Of Balance)(responseData)

                        Return "Balance: " & result.total_balance & " Code: " & result.code & "  Message: " & result.message
                    End Using
                ElseIf CInt(httpResponse.StatusCode) = 400 Then
                    Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                        Dim responseData As String = streamReader.ReadToEnd()
                        Dim result = JsonConvert.DeserializeObject(Of _4jawalyRoot)(responseData)

                        Return "Error 400 " & " Code: " & result.code & "  Message: " & result.message
                    End Using
                ElseIf CInt(httpResponse.StatusCode) = 403 Then
                    Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                        Dim responseData As String = streamReader.ReadToEnd()
                        Dim result = JsonConvert.DeserializeObject(Of _4jawalyRoot)(responseData)

                        Return "Error 400 " & " Code: " & result.code & "  Message: " & result.message
                    End Using
                Else
                    Return " Error Code: " & CInt(httpResponse.StatusCode) & "  Message: error in data"
                End If
            Catch ex As WebException
                If App_key Is Nothing Then
                    Return "key required"
                End If

                If App_secret Is Nothing Then
                    Return "secret required"
                End If
                'else
                '    return "Try again";

                Dim errorResponse As HttpWebResponse = DirectCast(ex.Response, HttpWebResponse)
                Using reader As New StreamReader(errorResponse.GetResponseStream())
                    Dim errorText As String = reader.ReadToEnd()
                    ' Handle error message
                    Dim Response As _4jawalyRoot = JsonConvert.DeserializeObject(Of _4jawalyRoot)(errorText)
                    Return "Error code:" & Response.code & " Message: " & Response.message
                End Using
            End Try
        End Function
    End Module
End Namespace
