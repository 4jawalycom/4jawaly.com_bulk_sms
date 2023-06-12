Imports API_Example_VB.API_Example_VB
Imports API_Example_VB.API_Example_VB.Models

Public Class Form1
    Private Sub BtnConnect_Click(sender As Object, e As EventArgs) Handles BtnConnect.Click

        Dim data = _4jawaly.GetSenders4jawaly(txtAPIkey.Text, txtAPISecret.Text)

        If data IsNot Nothing AndAlso data.items IsNot Nothing AndAlso data.items.data IsNot Nothing Then
            Dim activeSender = data.items.data.Where(Function(s) s.status = 1).ToList()
            cmbSenderNames.DataSource = activeSender
            cmbSenderNames2.DataSource = activeSender
            MessageBox.Show("تم الاتصال بنجاح | Connected")
        Else
            MessageBox.Show("لم ينم الاتصال | Can not connect")
        End If
    End Sub


    Private Sub BtnSend_Click(sender As Object, e As EventArgs) Handles BtnSend.Click
        Dim mobiles As String() = txtMobiles.Text.Split(",")

        Dim messages As message = New message()
        messages.text = txtMessage.Text
        messages.numbers = mobiles.ToList()
        '    For Each mobile In mobiles
        '        messages.Add(New Message() With {
        '.text = txtMessage.Text,
        '.numbers = New List(Of String)() From {
        'mobile
        '}
        '})
        '    Next

        Dim result = _4jawaly.Send4jawaly(txtAPIkey.Text, txtAPISecret.Text, cmbSenderNames2.Text, messages)
        If result.Sent Then
            MessageBox.Show("Message sent تم الارسال")
        Else
            MessageBox.Show("Message not sent لم يتم الارسال" & Environment.NewLine & result.Message)
        End If
    End Sub
End Class
