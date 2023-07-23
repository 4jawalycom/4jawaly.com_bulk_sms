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


        Dim result = _4jawaly.Send4jawaly(txtAPIkey.Text, txtAPISecret.Text, cmbSenderNames2.Text, txtMobiles.Text, txtMessage.Text)

        MessageBox.Show(result)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim result = _4jawaly.GetBalance(txtAPIkey.Text, txtAPISecret.Text)

        MessageBox.Show(result)
    End Sub
End Class
