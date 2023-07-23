<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tabControl1 = New System.Windows.Forms.TabControl()
        Me.tabPage1 = New System.Windows.Forms.TabPage()
        Me.cmbSenderNames = New System.Windows.Forms.ComboBox()
        Me.BtnConnect = New System.Windows.Forms.Button()
        Me.txtAPISecret = New System.Windows.Forms.TextBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.txtAPIkey = New System.Windows.Forms.TextBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.tabPage2 = New System.Windows.Forms.TabPage()
        Me.BtnSend = New System.Windows.Forms.Button()
        Me.txtMobiles = New System.Windows.Forms.TextBox()
        Me.txtMessage = New System.Windows.Forms.RichTextBox()
        Me.cmbSenderNames2 = New System.Windows.Forms.ComboBox()
        Me.label6 = New System.Windows.Forms.Label()
        Me.label8 = New System.Windows.Forms.Label()
        Me.label7 = New System.Windows.Forms.Label()
        Me.label5 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tabControl1.SuspendLayout()
        Me.tabPage1.SuspendLayout()
        Me.tabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabControl1
        '
        Me.tabControl1.Controls.Add(Me.tabPage1)
        Me.tabControl1.Controls.Add(Me.tabPage2)
        Me.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabControl1.Location = New System.Drawing.Point(0, 0)
        Me.tabControl1.Multiline = True
        Me.tabControl1.Name = "tabControl1"
        Me.tabControl1.RightToLeftLayout = True
        Me.tabControl1.SelectedIndex = 0
        Me.tabControl1.Size = New System.Drawing.Size(743, 450)
        Me.tabControl1.TabIndex = 1
        '
        'tabPage1
        '
        Me.tabPage1.Controls.Add(Me.cmbSenderNames)
        Me.tabPage1.Controls.Add(Me.BtnConnect)
        Me.tabPage1.Controls.Add(Me.txtAPISecret)
        Me.tabPage1.Controls.Add(Me.label3)
        Me.tabPage1.Controls.Add(Me.label2)
        Me.tabPage1.Controls.Add(Me.txtAPIkey)
        Me.tabPage1.Controls.Add(Me.label1)
        Me.tabPage1.Location = New System.Drawing.Point(4, 22)
        Me.tabPage1.Name = "tabPage1"
        Me.tabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPage1.Size = New System.Drawing.Size(735, 424)
        Me.tabPage1.TabIndex = 0
        Me.tabPage1.Text = "الاعدادات Settings"
        Me.tabPage1.UseVisualStyleBackColor = True
        '
        'cmbSenderNames
        '
        Me.cmbSenderNames.DisplayMember = "sender_name"
        Me.cmbSenderNames.FormattingEnabled = True
        Me.cmbSenderNames.Location = New System.Drawing.Point(247, 106)
        Me.cmbSenderNames.Name = "cmbSenderNames"
        Me.cmbSenderNames.Size = New System.Drawing.Size(239, 21)
        Me.cmbSenderNames.TabIndex = 10
        Me.cmbSenderNames.ValueMember = "sender_name"
        '
        'BtnConnect
        '
        Me.BtnConnect.Location = New System.Drawing.Point(135, 54)
        Me.BtnConnect.Name = "BtnConnect"
        Me.BtnConnect.Size = New System.Drawing.Size(106, 23)
        Me.BtnConnect.TabIndex = 9
        Me.BtnConnect.Text = "اتصال Connect"
        Me.BtnConnect.UseVisualStyleBackColor = True
        '
        'txtAPISecret
        '
        Me.txtAPISecret.Location = New System.Drawing.Point(247, 57)
        Me.txtAPISecret.Name = "txtAPISecret"
        Me.txtAPISecret.Size = New System.Drawing.Size(239, 20)
        Me.txtAPISecret.TabIndex = 7
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(494, 109)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(138, 13)
        Me.label3.TabIndex = 4
        Me.label3.Text = "أسماء الاسال SenderNames"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(505, 57)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(58, 13)
        Me.label2.TabIndex = 5
        Me.label2.Text = "API Secret"
        '
        'txtAPIkey
        '
        Me.txtAPIkey.Location = New System.Drawing.Point(247, 31)
        Me.txtAPIkey.Name = "txtAPIkey"
        Me.txtAPIkey.Size = New System.Drawing.Size(239, 20)
        Me.txtAPIkey.TabIndex = 8
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(519, 31)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(44, 13)
        Me.label1.TabIndex = 6
        Me.label1.Text = "API key"
        '
        'tabPage2
        '
        Me.tabPage2.Controls.Add(Me.Button1)
        Me.tabPage2.Controls.Add(Me.BtnSend)
        Me.tabPage2.Controls.Add(Me.txtMobiles)
        Me.tabPage2.Controls.Add(Me.txtMessage)
        Me.tabPage2.Controls.Add(Me.cmbSenderNames2)
        Me.tabPage2.Controls.Add(Me.label6)
        Me.tabPage2.Controls.Add(Me.label8)
        Me.tabPage2.Controls.Add(Me.label7)
        Me.tabPage2.Controls.Add(Me.label5)
        Me.tabPage2.Controls.Add(Me.label4)
        Me.tabPage2.Location = New System.Drawing.Point(4, 22)
        Me.tabPage2.Name = "tabPage2"
        Me.tabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPage2.Size = New System.Drawing.Size(735, 424)
        Me.tabPage2.TabIndex = 1
        Me.tabPage2.Text = "الارسال Send"
        Me.tabPage2.UseVisualStyleBackColor = True
        '
        'BtnSend
        '
        Me.BtnSend.Location = New System.Drawing.Point(253, 296)
        Me.BtnSend.Name = "BtnSend"
        Me.BtnSend.Size = New System.Drawing.Size(75, 23)
        Me.BtnSend.TabIndex = 15
        Me.BtnSend.Text = "ارسال Send"
        Me.BtnSend.UseVisualStyleBackColor = True
        '
        'txtMobiles
        '
        Me.txtMobiles.Location = New System.Drawing.Point(253, 106)
        Me.txtMobiles.Name = "txtMobiles"
        Me.txtMobiles.Size = New System.Drawing.Size(239, 20)
        Me.txtMobiles.TabIndex = 14
        '
        'txtMessage
        '
        Me.txtMessage.Location = New System.Drawing.Point(253, 178)
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(239, 96)
        Me.txtMessage.TabIndex = 13
        Me.txtMessage.Text = ""
        '
        'cmbSenderNames2
        '
        Me.cmbSenderNames2.DisplayMember = "sender_name"
        Me.cmbSenderNames2.FormattingEnabled = True
        Me.cmbSenderNames2.Location = New System.Drawing.Point(253, 36)
        Me.cmbSenderNames2.Name = "cmbSenderNames2"
        Me.cmbSenderNames2.Size = New System.Drawing.Size(239, 21)
        Me.cmbSenderNames2.TabIndex = 12
        Me.cmbSenderNames2.ValueMember = "sender_name"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(498, 178)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(103, 13)
        Me.label6.TabIndex = 11
        Me.label6.Text = "نص الرسالة Message"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(169, 144)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(312, 13)
        Me.label8.TabIndex = 11
        Me.label8.Text = "Mobiles seperated by coma ex 9665123456789,9665123456789"
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(169, 129)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(326, 13)
        Me.label7.TabIndex = 11
        Me.label7.Text = "أرقام الجوال مفصولة ب فاصلة مثال 9665123456789,9665123456789"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(498, 109)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(75, 13)
        Me.label5.TabIndex = 11
        Me.label5.Text = "الأرقام Mobiles"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(500, 39)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(138, 13)
        Me.label4.TabIndex = 11
        Me.label4.Text = "أسماء الاسال SenderNames"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(403, 296)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 16
        Me.Button1.Text = "الرصيد"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(743, 450)
        Me.Controls.Add(Me.tabControl1)
        Me.Name = "Form1"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RightToLeftLayout = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Send"
        Me.tabControl1.ResumeLayout(False)
        Me.tabPage1.ResumeLayout(False)
        Me.tabPage1.PerformLayout()
        Me.tabPage2.ResumeLayout(False)
        Me.tabPage2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents tabControl1 As TabControl
    Private WithEvents tabPage1 As TabPage
    Private WithEvents cmbSenderNames As ComboBox
    Private WithEvents BtnConnect As Button
    Private WithEvents txtAPISecret As TextBox
    Private WithEvents label3 As Label
    Private WithEvents label2 As Label
    Private WithEvents txtAPIkey As TextBox
    Private WithEvents label1 As Label
    Private WithEvents tabPage2 As TabPage
    Private WithEvents BtnSend As Button
    Private WithEvents txtMobiles As TextBox
    Private WithEvents txtMessage As RichTextBox
    Private WithEvents cmbSenderNames2 As ComboBox
    Private WithEvents label6 As Label
    Private WithEvents label8 As Label
    Private WithEvents label7 As Label
    Private WithEvents label5 As Label
    Private WithEvents label4 As Label
    Private WithEvents Button1 As Button
End Class
