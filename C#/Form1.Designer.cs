
namespace API_Example
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cmbSenderNames = new System.Windows.Forms.ComboBox();
            this.BtnConnect = new System.Windows.Forms.Button();
            this.txtAPISecret = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAPIkey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.BtnSend = new System.Windows.Forms.Button();
            this.txtMobiles = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.cmbSenderNames2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(669, 358);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cmbSenderNames);
            this.tabPage1.Controls.Add(this.BtnConnect);
            this.tabPage1.Controls.Add(this.txtAPISecret);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtAPIkey);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(661, 332);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "الاعدادات Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cmbSenderNames
            // 
            this.cmbSenderNames.DisplayMember = "sender_name";
            this.cmbSenderNames.FormattingEnabled = true;
            this.cmbSenderNames.Location = new System.Drawing.Point(247, 106);
            this.cmbSenderNames.Name = "cmbSenderNames";
            this.cmbSenderNames.Size = new System.Drawing.Size(239, 21);
            this.cmbSenderNames.TabIndex = 10;
            this.cmbSenderNames.ValueMember = "sender_name";
            // 
            // BtnConnect
            // 
            this.BtnConnect.Location = new System.Drawing.Point(135, 54);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(106, 23);
            this.BtnConnect.TabIndex = 9;
            this.BtnConnect.Text = "اتصال Connect";
            this.BtnConnect.UseVisualStyleBackColor = true;
            this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // txtAPISecret
            // 
            this.txtAPISecret.Location = new System.Drawing.Point(247, 57);
            this.txtAPISecret.Name = "txtAPISecret";
            this.txtAPISecret.Size = new System.Drawing.Size(239, 20);
            this.txtAPISecret.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(494, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "أسماء الاسال SenderNames";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(505, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "API Secret";
            // 
            // txtAPIkey
            // 
            this.txtAPIkey.Location = new System.Drawing.Point(247, 31);
            this.txtAPIkey.Name = "txtAPIkey";
            this.txtAPIkey.Size = new System.Drawing.Size(239, 20);
            this.txtAPIkey.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(519, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "API key";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.BtnSend);
            this.tabPage2.Controls.Add(this.txtMobiles);
            this.tabPage2.Controls.Add(this.txtMessage);
            this.tabPage2.Controls.Add(this.cmbSenderNames2);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(661, 332);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "الارسال Send";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // BtnSend
            // 
            this.BtnSend.Location = new System.Drawing.Point(253, 296);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(75, 23);
            this.BtnSend.TabIndex = 15;
            this.BtnSend.Text = "ارسال Send";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // txtMobiles
            // 
            this.txtMobiles.Location = new System.Drawing.Point(253, 106);
            this.txtMobiles.Name = "txtMobiles";
            this.txtMobiles.Size = new System.Drawing.Size(239, 20);
            this.txtMobiles.TabIndex = 14;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(253, 178);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(239, 96);
            this.txtMessage.TabIndex = 13;
            this.txtMessage.Text = "";
            // 
            // cmbSenderNames2
            // 
            this.cmbSenderNames2.DisplayMember = "sender_name";
            this.cmbSenderNames2.FormattingEnabled = true;
            this.cmbSenderNames2.Location = new System.Drawing.Point(253, 36);
            this.cmbSenderNames2.Name = "cmbSenderNames2";
            this.cmbSenderNames2.Size = new System.Drawing.Size(239, 21);
            this.cmbSenderNames2.TabIndex = 12;
            this.cmbSenderNames2.ValueMember = "sender_name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(498, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "نص الرسالة Message";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(169, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(312, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Mobiles seperated by coma ex 9665123456789,9665123456789";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(169, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(326, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "أرقام الجوال مفصولة ب فاصلة مثال 9665123456789,9665123456789";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(498, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "الأرقام Mobiles";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(500, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "أسماء الاسال SenderNames";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(373, 296);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "الرصيد";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 358);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "send";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ComboBox cmbSenderNames;
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.TextBox txtAPISecret;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAPIkey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSenderNames2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMobiles;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
    }
}

