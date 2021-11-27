using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Myaccount
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private bool _status;
        public bool Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        static protected string Check;
        private string Dncrypt()
        {
            string fileName = @"bunifu.bat";
            var fileText = System.IO.File.ReadAllText(fileName);
            string texttodecode = fileText;
            string decoded = "";
            var bytes = Convert.FromBase64String(texttodecode);
            decoded = Encoding.UTF8.GetString(bytes);
            return decoded;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {

            Check = Dncrypt();
            if(Password.Text == Check)
            {
                Status = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblWrongPassword.Visible = true;
            }
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuSwitch1_Click(object sender, EventArgs e)
        {
            if (bunifuSwitch1.Value == true)
            {
                Password.PasswordChar = '\0';

            }
            else
            {
                Password.PasswordChar = '*';

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bunifuButton1_Click(this, new EventArgs());
            }
        }

        
    }
}
