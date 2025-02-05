VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   5385
   ClientLeft      =   120
   ClientTop       =   465
   ClientWidth     =   8325
   LinkTopic       =   "Form1"
   ScaleHeight     =   5385
   ScaleWidth      =   8325
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command4 
      Caption         =   "Get Balance"
      Height          =   615
      Left            =   5520
      TabIndex        =   11
      Top             =   1320
      Width           =   1815
   End
   Begin VB.CommandButton Command3 
      Caption         =   "Check connection"
      Height          =   735
      Left            =   5520
      TabIndex        =   10
      Top             =   120
      Width           =   1815
   End
   Begin VB.CommandButton Command2 
      Caption         =   "Send SMS"
      Height          =   495
      Left            =   6720
      TabIndex        =   9
      Top             =   4440
      Width           =   1335
   End
   Begin VB.TextBox txtMessage 
      Height          =   1095
      Left            =   360
      TabIndex        =   8
      Text            =   "SMS Message"
      Top             =   4080
      Width           =   5655
   End
   Begin VB.TextBox txtMobiles 
      Height          =   495
      Left            =   360
      TabIndex        =   7
      Text            =   "Mobile Numbers with , ex 01111111111111,02222222222"
      Top             =   3360
      Width           =   5655
   End
   Begin VB.ComboBox cmbSenderNames 
      Height          =   315
      Left            =   360
      TabIndex        =   4
      Top             =   2520
      Width           =   2775
   End
   Begin VB.TextBox txtAPISecret 
      Height          =   495
      Left            =   360
      TabIndex        =   3
      Top             =   1440
      Width           =   2655
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Get Senders"
      Height          =   615
      Left            =   5520
      TabIndex        =   1
      Top             =   2400
      Width           =   1815
   End
   Begin VB.TextBox txtAPIkey 
      Height          =   495
      Left            =   360
      TabIndex        =   0
      Top             =   480
      Width           =   2655
   End
   Begin VB.Label Label3 
      Caption         =   "Sender Name"
      Height          =   255
      Left            =   3360
      TabIndex        =   6
      Top             =   2520
      Width           =   1575
   End
   Begin VB.Label Label2 
      Caption         =   "API Secret"
      Height          =   495
      Left            =   3360
      TabIndex        =   5
      Top             =   1440
      Width           =   1575
   End
   Begin VB.Label Label1 
      Caption         =   "API Key"
      Height          =   375
      Left            =   3360
      TabIndex        =   2
      Top             =   600
      Width           =   1695
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Function Base64Encode(ByVal sText As String) As String
    Dim arrData() As Byte
    arrData = StrConv(sText, vbFromUnicode)
    Base64Encode = StrConv(Base64EncodeByteArray(arrData), vbUnicode)
End Function

Private Function Base64EncodeByteArray(ByRef arrData() As Byte) As String
    Dim objXML As Object
    Set objXML = CreateObject("MSXML2.DOMDocument")
    Dim objNode As Object
    Set objNode = objXML.createElement("b64")
    objNode.DataType = "bin.base64"
    objNode.nodeTypedValue = arrData
    Base64EncodeByteArray = objNode.Text
    Set objNode = Nothing
    Set objXML = Nothing
End Function

Private Sub Command1_Click()
Dim mysms As New jawalySms.SmsGo
   Dim senderList() As String
senderList = mysms.GetSenders(txtAPIkey.Text, txtAPISecret.Text)
  For i = 0 To UBound(senderList)
   cmbSenderNames.AddItem senderList(i)
Next i

End Sub

Private Sub Text1_Change()

End Sub

Private Sub Command2_Click()
Dim mysms As New jawalySms.SmsGo

Dim result As String
result = mysms.SendSMS(txtAPIkey.Text, txtAPISecret.Text, cmbSenderNames.Text, txtMobiles.Text, txtMessage.Text)
MsgBox result, vbOKOnly, "result"
End Sub

Private Sub Command3_Click()
Dim mysms As New jawalySms.SmsGo
   MsgBox mysms.get_Hello, vbOKOnly, "Connected"
End Sub

Private Sub Command4_Click()
Dim mysms As New jawalySms.SmsGo
   Dim x As String
   x = mysms.GetBalance(txtAPIkey.Text, txtAPISecret.Text)
   MsgBox x, vbOKOnly, "Connected"
End Sub
