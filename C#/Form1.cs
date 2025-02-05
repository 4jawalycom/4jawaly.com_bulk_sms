using API_Example.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace API_Example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            var data = _4jawaly.GetSenders4jawaly(txtAPIkey.Text, txtAPISecret.Text);

            if (data != null && data.items != null && data.items.data != null)
            {
                var activeSender = data.items.data.Where(s => s.status == 1).ToList(); ;
                cmbSenderNames.DataSource = activeSender;
                cmbSenderNames2.DataSource = activeSender;
                MessageBox.Show("تم الاتصال بنجاح | Connected");
            }
            else
            {
                MessageBox.Show("لم ينم الاتصال | Can not connect");
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            //string[] mobiles = txtMobiles.Text.Split(',');

            //message messages = new message();
            //messages.text = txtMessage.Text;
            //messages.numbers=mobiles.ToList();

            //foreach (var mobile in mobiles)
            //{
            //    messages.Add(new message()
            //    {
            //        text = txtMessage.Text,
            //        numbers = new List<string>() { mobile }
            //    });
            //}

            var result = _4jawaly.Send4jawaly(txtAPIkey.Text, txtAPISecret.Text, cmbSenderNames2.Text, txtMobiles.Text, txtMessage.Text);

                MessageBox.Show(result);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = _4jawaly.GetBalance(txtAPIkey.Text, txtAPISecret.Text);

            MessageBox.Show(result);
        }
    }
}
