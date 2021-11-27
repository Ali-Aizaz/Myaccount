using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;


namespace Myaccount
{
    public partial class Form4 : Form
    {

        public Form4()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        public void InsertClient()
        {
            string query = "INSERT INTO Clients (Name,Description,Balance) Values(@Name,@Description,0)";
            Database databaseObj = new Database();
            databaseObj.myConnection.Open();
            using (SQLiteCommand dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
            {
                dbaseCommand.Parameters.AddWithValue("@Name",txtNewClient.Text);
                dbaseCommand.Parameters.AddWithValue("@Description",txtNewDescription.Text);
                dbaseCommand.ExecuteNonQuery();
            }
            databaseObj.myConnection.Close();
            databaseObj.myConnection.Dispose();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtNewClient.Text !="" && txtNewClient.Text !="Client Name" && txtNewDescription.Text != "" && txtNewDescription.Text != "Description")
                {
                    InsertClient();
                    this.DialogResult = DialogResult.OK;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incomplete Text Field", "Client Adding Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Client Adding Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtNewClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(txtNewClient.Text == "Client Name")
            {
                txtNewClient.Text = null;
            }
        }

        private void bunifuMetroTextbox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtNewDescription.Text == "Description")
            {
                txtNewDescription.Text = null;
            }
        }

        private void txtNewDescription_Enter(object sender, EventArgs e)
        {
            if(txtNewDescription.Text == "Description")
            {
                txtNewDescription.Text = null;
            }
        }

        private void txtNewClient_Enter(object sender, EventArgs e)
        {
            if (txtNewClient.Text == "Client Name")
            {
                txtNewClient.Text = null;
            }
        }

        private void txtNewClient_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtNewClient.Text))
            {
                txtNewClient.Text = "Client Name";
            }
        }

        private void txtNewDescription_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtNewDescription.Text))
            {
                txtNewDescription.Text = "Description";
            }
        }
    }
}
