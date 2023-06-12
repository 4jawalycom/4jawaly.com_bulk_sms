
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization
Imports API_Example_VB.API_Example_VB.Models
Imports API_Example_VB.API_Example_VB.Models.Balance
Imports Newtonsoft.Json

Namespace API_Example_VB
    Module _4jawaly
        Private UserAgent As String = "XXXXXXXXX" 'put your useragent here

        Function Send4jawaly(ByVal App_key As String, ByVal App_secret As String, ByVal SenderName As String, ByVal messages As message) As SendResult
            Dim senddata = New SendData()
            senddata.messages = messages
            senddata.sender = SenderName

            Dim url = "https://api-sms.4jawaly.com/api/v1/account/area/sms/send"
            Dim credentials As String = App_key & ":" & App_secret
            Dim httpRequest = CType(WebRequest.Create(url), HttpWebRequest)
            httpRequest.Method = "POST"
            httpRequest.Accept = "application/json"
            httpRequest.ContentType = "application/json"
            httpRequest.Headers("Authorization") = "Basic " & Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials))
            Dim serializer = New JavaScriptSerializer()
            Dim data = serializer.Serialize(senddata)

            Try

                Using streamWriter = New StreamWriter(httpRequest.GetRequestStream())
                    streamWriter.Write(data)
                End Using

                Dim httpResponse = CType(httpRequest.GetResponse(), HttpWebResponse)
                Dim result As String = ""

                If httpResponse.StatusCode = HttpStatusCode.OK Then

                    Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                        result = streamReader.ReadToEnd()
                    End Using


                    Dim myDeserializedClass As _4jawalyRoot = JsonConvert.DeserializeObject(Of _4jawalyRoot)(result)
                    Return New SendResult() With {
                        .Sent = True,
                        .Message = result
                    }
                ElseIf httpResponse.StatusCode = HttpStatusCode.Ambiguous Or httpResponse.StatusCode = HttpStatusCode.BadRequest Then

                    Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                        result = streamReader.ReadToEnd()
                    End Using


                    Dim myDeserializedClass As _4jawalyRoot = JsonConvert.DeserializeObject(Of _4jawalyRoot)(result)
                    Return New SendResult() With {
                            .Sent = False,
                            .Message = result
                        }
                Else
                    Dim statusCode As Integer = httpResponse.StatusCode

                    Return New SendResult() With {
                           .Sent = False,
                           .Message = statusCode
                       }

                End If

            Catch ex As Exception
                Return New SendResult() With {
                    .Sent = False,
                    .Message = ex.Message
                }
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

        Function GetBalance4jawaly(ByVal App_key As String, ByVal App_secret As Integer) As Balance
            Try
                Dim url = "https://api-sms.4jawaly.com/api/v1/account/area/me/packages?is_active=1&order_by=id&order_by_type=desc&page=1&page_size=10&return_collection=1"
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

                Return JsonConvert.DeserializeObject(Of Balance)(result)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Function Check4jawaly(ByVal App_key As String, ByVal App_secret As String) As Boolean
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

                If httpResponse.StatusCode = HttpStatusCode.OK Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                Return False
            End Try
        End Function
    End Module
End Namespace
