using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Tulpep.NotificationWindow;

namespace Myaccount
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            
        }
        private void notification(string title, string text)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Size = new Size(210, 100);
            popup.Image = Properties.Resources.ios;
            popup.TitleText = title;
            popup.ContentText = text;
            popup.Popup();
        }

        private void Createnewpassword(String newpassword)
        {
            
            string fileName = @"Config.bat";
            byte[] stringbytes = Encoding.UTF8.GetBytes(newpassword);
            string result = System.Convert.ToBase64String(stringbytes);
            using (StreamWriter writers = new StreamWriter(fileName))
            {
                writers.WriteLine(result);
            }
            notification("Password Security", "Admin Password Changed Succesfully");
        }

        public void clearp()
        {
            skt.Text = npt.Text = cnpt.Text = null;
        }

        protected static string SK = "93ex7fhc";
        private void Newpass_Click(object sender, EventArgs e)
        {
            if(skt.Text == SK)
            {
                if(npt.Text == cnpt.Text )
                {
                    Createnewpassword(npt.Text);
                    clearp();
                    using(Form3 frm3 = new Form3())
                    {
                        this.Hide();
                        frm3.ShowDialog();
                    }
                 
                }
                else
                {
                    MessageBox.Show("New passwords should be same in both fields", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clearp();
                }
            }
            else
            {
                MessageBox.Show("Security Key is Invalid Kindly Enter Correct Key or Contact Developers.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clearp();            
            }
            
        }
    }
}
