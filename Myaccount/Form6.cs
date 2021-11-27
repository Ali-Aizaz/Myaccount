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
    public partial class Form6 : Form
    {
        string ogName = "";
        string ogDesc = "";
        string idx = "";
        public Form6(string OGName, string OGDesc, string ogID)
        {
            InitializeComponent();
            ogName = OGName;
            ogDesc = OGDesc;
            idx = ogID;
            txtNewClient.Text = OGName;
            txtNewDescription.Text = OGDesc;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if ((txtNewClient.Text != ogName && txtNewClient.Text != "" && txtNewDescription.Text != "") || (txtNewDescription.Text != ogDesc && txtNewClient.Text != "" && txtNewDescription.Text != ""))
            {
                Database dbSQL = new Database();
                dbSQL.myConnection.Open();
                try
                {
                    string query = "UPDATE Clients SET Name=@Name, Description=@Desc WHERE ID=@ID";
                    using (SQLiteCommand command = new SQLiteCommand(query, dbSQL.myConnection))
                    {
                        command.Parameters.AddWithValue("@ID", idx);
                        command.Parameters.AddWithValue("@Name", txtNewClient.Text);
                        command.Parameters.AddWithValue("@Desc", txtNewDescription.Text);
                        command.ExecuteNonQuery();
                        command.Dispose();
                    }
                    dbSQL.myConnection.Close();
                    dbSQL.myConnection.Dispose();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    dbSQL.myConnection.Close();
                    dbSQL.myConnection.Dispose();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show(ex.Message, "Client Editing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("The input field is empty or unchanged please provide input for all text fields", "Client Editing Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
