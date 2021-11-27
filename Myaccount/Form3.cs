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
using System.Security.Cryptography;

namespace Myaccount
{
    public partial class Form3 : Form
    {
        private void DatabaseDecrypt(string Old, string New, string key)
        {
            FileStream inStream, outString;
            CryptoStream CryStream;
            TripleDESCryptoServiceProvider TDC = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

            byte[] byteHash, byteText;

            inStream = new FileStream(Old, FileMode.Open, FileAccess.Read);
            outString = new FileStream(New, FileMode.OpenOrCreate, FileAccess.Write);

            byteHash = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
            byteText = File.ReadAllBytes(Old);

            MD5.Clear();

            TDC.Key = byteHash;
            TDC.Mode = CipherMode.ECB;

            CryStream = new CryptoStream(outString, TDC.CreateDecryptor(), CryptoStreamMode.Write);

            int bytesRead;
            long length, position = 0;
            length = inStream.Length;

            while (position < length)
            {
                bytesRead = inStream.Read(byteText, 0, byteText.Length);
                position += bytesRead;

                CryStream.Write(byteText, 0, bytesRead);
            }
            CryStream.Close();
            inStream.Close();
            outString.Close();
        }

        public void DatabaseEncrypt(string Old, string New, string key)
        {
            FileStream inStream, outString;
            CryptoStream CryStream;
            TripleDESCryptoServiceProvider TDC = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

            byte[] byteHash, byteText;

            inStream = new FileStream(Old, FileMode.Open, FileAccess.Read);
            outString = new FileStream(New, FileMode.OpenOrCreate, FileAccess.Write);

            byteHash = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
            byteText = File.ReadAllBytes(Old);

            MD5.Clear();

            TDC.Key = byteHash;
            TDC.Mode = CipherMode.ECB;

            CryStream = new CryptoStream(outString, TDC.CreateEncryptor(), CryptoStreamMode.Write);

            int bytesRead;
            long length, position = 0;
            length = inStream.Length;

            while (position < length)
            {
                bytesRead = inStream.Read(byteText, 0, byteText.Length);
                position += bytesRead;

                CryStream.Write(byteText, 0, bytesRead);
            }
            CryStream.Close();
            inStream.Close();
            outString.Close();
        }


        static protected string Check;
        private string Dnucrypt()
        {
                string fileName = @"Config.bat";
                var fileText = System.IO.File.ReadAllText(fileName);
                string texttodecode = fileText;
                var bytes = Convert.FromBase64String(texttodecode);
                string decoded = Encoding.UTF8.GetString(bytes);
                return decoded;
        }

        public Form3()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            string path = System.IO.Path.GetTempPath() + "System.db";
            try
            {
                string key = Dnucrypt();
               
                if (!File.Exists("database"))
                {
                    this.Hide();
                    using (Accounts frm1 = new Accounts())
                    {
                        frm1.ShowDialog();
                        if (frm1.DialogResult == DialogResult.OK)
                        {
                            frm1.Close();
                            DatabaseEncrypt(path, "database", key);
                            File.Delete(path);
                            Application.Exit();
                        }
                    }
                }
                else if (Password.Text == key)
                {
                    DatabaseDecrypt("database", path, key);
                    this.Hide();
                    using (Accounts frm1 = new Accounts())
                    {
                        frm1.ShowDialog();

                        if (frm1.DialogResult == DialogResult.OK)
                        {
                            DatabaseEncrypt(path, "database", key);
                            File.Delete(path);
                            frm1.Close();
                            Application.Exit();
                        }
                    }
                }
                else
                {
                    this.lblinvalid.Visible = true;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }



        private void bunifuSwitch1_Click(object sender, EventArgs e)
        {
            if(bunifuSwitch1.Value == true)
            {
                Password.PasswordChar = '\0';
             
            }
            else
            {
                Password.PasswordChar = '*';
          
            }
        }

        private void Password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bunifuButton1_Click(this, new EventArgs());
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

      

    }
}
