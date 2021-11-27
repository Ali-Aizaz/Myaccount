using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Tulpep.NotificationWindow;
using System.Data.SQLite;
using System.Threading;

namespace Myaccount
{

    public partial class Accounts : Form
    {
        public bool Result { get; set; }
        string query = "";
        SQLiteCommand dbaseCommand;
        SQLiteDataReader reader;
        Database databaseObj = new Database();
        int Index = -1;
        int ClientSelected = -1;
        string idx = "-1";
        int DeletingClient = -1;
        long PrevTotalBalance = 0;
        int BalanceFetchCount = 0;
        int FilesFetchCount = 0;
        bool inUse = false;
        bool isUptoDate = false;
        bool isUptoDatePrivate = false;
        static protected string Check;
        private string Dncrypt()
        {
            string fileName = @"Bunifu.bat";
            var fileText = System.IO.File.ReadAllText(fileName);
            string texttodecode = fileText;
            var bytes = Convert.FromBase64String(texttodecode);
            string decoded = Encoding.UTF8.GetString(bytes);
            return decoded;
        }

        private string Dnucrypt()
        {
            string fileName = @"Config.bat";
            var fileText = System.IO.File.ReadAllText(fileName);
            string texttodecode = fileText;
            var bytes = Convert.FromBase64String(texttodecode);
            string decoded = Encoding.UTF8.GetString(bytes);
            return decoded;
        }

        public Accounts()
        {
            try
            {
                InitializeComponent();
                AddClientList();
                notification("Greetings", "Welcome Back User");


                foreach (DataGridViewColumn column in dgvData.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Dependence Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                databaseObj.myConnection.Close();
                Result = true;
                this.Close();
            }
        }

        private void AddClientList()
        {
            try
            {
                databaseObj.myConnection.Open();
                query = "SELECT ID, Description, Name FROM Clients";
                using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                {
                    reader = dbaseCommand.ExecuteReader();
                    int i = 0;
                    dgvDataClients.Rows.Clear();
                    dgvData.Rows.Clear();
                    lblDataBalance.Text = "0";
                    lblDataTotal.Text = "00";
                    lblclientDesc.Text = "--";
                    lblledger.Text = "Ledger";
                    while (reader.Read())
                    {
                        dgvDataClients.Rows.Add();
                        dgvDataClients.Rows[i].Cells[0].Value = reader["Name"];
                        dgvDataClients.Rows[i].Cells[1].Value = reader["Description"];
                        dgvDataClients.Rows[i].Cells[2].Value = reader["ID"];
                        i++;
                    }

                    reader.Close();
                }
                query = "";
                databaseObj.myConnection.Close();
            }
            catch (Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Client Adding Error");
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            SETTING.SetPage(3);
        }

        private void UpdateForm(bool isAdmin)
        {
            try
            {
                databaseObj.myConnection.Open();
                using (dbaseCommand = new SQLiteCommand("", databaseObj.myConnection))
                {
                    dgvData.Rows.Clear();
                    isUptoDate = false;
                    inUse = false;
                    query = "SELECT Balance FROM Clients WHERE ID=@ID";
                    dbaseCommand.CommandText = query;
                    dbaseCommand.Parameters.AddWithValue("@ID", dgvDataClients.Rows[ClientSelected].Cells[2].Value);
                    reader = dbaseCommand.ExecuteReader();
                    reader.Read();
                    lblDataTotal.Text = reader["Balance"].ToString();
                    reader.Close();
                    PrevTotalBalance = long.Parse(lblDataTotal.Text);
                    if(isAdmin)
                    {
                        query = "SELECT * FROM (SELECT * FROM myAccounts WHERE NameID = @ID ORDER BY datetime(DateAdded) ,ID DESC LIMIT 100) ORDER BY datetime(DateAdded) DESC, ID";
                    }
                    else
                    {
                        query = "SELECT * FROM (SELECT * FROM myAccounts WHERE NameID = @ID ORDER BY datetime(DateAdded) ,ID DESC LIMIT 10) ORDER BY datetime(DateAdded) DESC, ID";
                    }
                    dbaseCommand.CommandText = query;
                    dbaseCommand.Parameters.AddWithValue("@ID", dgvDataClients.Rows[ClientSelected].Cells[2].Value.ToString());
                    reader = dbaseCommand.ExecuteReader();                 
                    int i = 0;
                    while (reader.Read())
                    {
                        dgvData.Rows.Add();
                        dgvData.Rows[i].Cells[1].Value = reader["ID"];
                        dgvData.Rows[i].Cells[2].Value = reader["DateAdded"];
                        dgvData.Rows[i].Cells[3].Value = reader["Narration"];
                        dgvData.Rows[i].Cells[4].Value = reader["Debit"];
                        dgvData.Rows[i].Cells[5].Value = reader["Credit"];
                        dgvData.Rows[i].Cells[6].Value = reader["NameID"];
                        i++;
                    }
                    reader.Close();
                    int j = 1;
                    for( ; i>0 ; )
                    {
                        i--;
                        dgvData.Rows[i].Cells[0].Value = j;
                        dgvData.Rows[i].Cells[7].Value = PrevTotalBalance;
                        PrevTotalBalance -= (long.Parse(dgvData.Rows[i].Cells[4].Value.ToString()) - long.Parse(dgvData.Rows[i].Cells[5].Value.ToString()));
                        j++;
                    }
                    BalanceFetchCount = 1;
                    query = "";
                    foreach (DataGridViewColumn column in dgvData.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
                databaseObj.myConnection.Close();
                if(dgvData.RowCount > 1)
                { 
                    dgvData.FirstDisplayedScrollingRowIndex = dgvData.RowCount - 1;
                }
            }
            catch (Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Initial Data Adding Error");
            }
        }

        private void LoadMoreEntries()
        {
            try
            {
                inUse = true;
                query = "SELECT * FROM (SELECT * FROM myAccounts WHERE NameID = @ID ORDER BY datetime(DateAdded) ,ID DESC LIMIT 100 OFFSET @OFFSET) ORDER BY datetime(DateAdded) DESC, ID";
                databaseObj.myConnection.Open();
                int x = 99;
                using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                {
                    dbaseCommand.Parameters.AddWithValue("@OFFSET", BalanceFetchCount * 100);
                    dbaseCommand.Parameters.AddWithValue("@ID", dgvDataClients.Rows[ClientSelected].Cells[2].Value);
                    reader = dbaseCommand.ExecuteReader();
                    if(reader.HasRows)
                    {
                        int j = dgvData.RowCount;
                        int i = j;
                        int k = j+100;
                        while (reader.Read())
                        {
                            dgvData.Rows.Add();
                            dgvData.Rows[j].Cells[0].Value = k;
                            dgvData.Rows[j].Cells[1].Value = reader["ID"];
                            dgvData.Rows[j].Cells[2].Value = reader["DateAdded"];
                            dgvData.Rows[j].Cells[3].Value = reader["Narration"];
                            dgvData.Rows[j].Cells[4].Value = reader["Debit"];
                            dgvData.Rows[j].Cells[5].Value = reader["Credit"];
                            dgvData.Rows[j].Cells[6].Value = reader["NameID"];
                            k--;
                            j++;
                        }
                        reader.Close(); 
                        int n = j % 100;
                        x = n;
                        int m = j-n;
                        for (; j > i;)
                        {
                            j--;
                            dgvData.Rows[j].Cells[7].Value = PrevTotalBalance;
                            PrevTotalBalance -= (long.Parse(dgvData.Rows[j].Cells[4].Value.ToString()) - long.Parse(dgvData.Rows[j].Cells[5].Value.ToString()));
                        }
                        if (n != 0)
                        {
                            int l = m;
                            for (; n > 0;)
                            {
                                n--;
                                dgvData.Rows[l].Cells[0].Value = m+n+1;
                                l++;
                            }
                            isUptoDate = true;
                        }
                    }
                    else
                    {
                        notification("All Entries Loaded","All of your entries have been loaded to the table") ;
                    }
                }
                BalanceFetchCount++;
                databaseObj.myConnection.Close();
                dgvData.FirstDisplayedScrollingRowIndex = x;
                dgvData.Refresh();
                dgvData.Sort(dgvData.Columns["DataID"], ListSortDirection.Descending);
                inUse = false;
            }
            catch(Exception ex)
            {
                dbaseCommand.Dispose();
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Loading Entries Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void UpdateFiles()
        {
            try
            {
                dgvPrivateFiles.Rows.Clear();
                databaseObj.myConnection.Open();
                query = "SELECT id, filename FROM documents LIMIT 100";
                isUptoDatePrivate = false;
                using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                {
                    reader = dbaseCommand.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {
                        dgvPrivateFiles.Rows.Add();
                        dgvPrivateFiles.Rows[i].Cells[0].Value = i + 1;
                        dgvPrivateFiles.Rows[i].Cells[1].Value = reader["filename"].ToString();
                        dgvPrivateFiles.Rows[i].Cells[2].Value = reader["ID"].ToString();
                        i++;
                    }
                    reader.Close();
                    if (i < 100)
                    {
                        isUptoDatePrivate = true;
                    }
                    FilesFetchCount = 1;
                }
                databaseObj.myConnection.Close();
                query = "";
            }
            catch (Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Form Load Error");
            }
        }

        private void LoadMoreFiles()
        {
            try
            {
                databaseObj.myConnection.Open();
                query = "SELECT id, filename FROM documents LIMIT 100 OFFSET @OFFSET";
                using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                {
                    dbaseCommand.Parameters.AddWithValue("@OFFSET", FilesFetchCount*100);
                    reader = dbaseCommand.ExecuteReader();
                    int i = dgvPrivateFiles.RowCount;
                    int j = i;
                    while (reader.Read())
                    {
                        dgvPrivateFiles.Rows.Add();
                        dgvPrivateFiles.Rows[i].Cells[0].Value = i + 1;
                        dgvPrivateFiles.Rows[i].Cells[1].Value = reader["filename"].ToString();
                        dgvPrivateFiles.Rows[i].Cells[2].Value = reader["ID"].ToString();
                        i++;
                    }
                    reader.Close();

                    if (i == j)
                    {
                        isUptoDatePrivate = true;
                    }
                }
                FilesFetchCount++;
                databaseObj.myConnection.Close();
                query = "";
            }
            catch (Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Form Load Error");
            }
        }

        private void onFileInsert(string name)
        {
            try
            {
                int i = dgvPrivateFiles.RowCount;
                dgvPrivateFiles.Rows.Add();
                query = "SELECT ID FROM documents ORDER BY ID DESC LIMIT 1";
                dgvPrivateFiles.Rows[i].Cells[0].Value = int.Parse(dgvPrivateFiles.Rows[i-1].Cells[0].Value.ToString()) + 1;
                dgvPrivateFiles.Rows[i].Cells[1].Value = name;
                databaseObj.myConnection.Open();
                using(dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                {
                    reader = dbaseCommand.ExecuteReader();
                    reader.Read();
                    dgvPrivateFiles.Rows[i].Cells[2].Value = reader["ID"];
                    reader.Close();
                }
                databaseObj.myConnection.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Uploading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UploadFiles(string filename)
        {
            try
            {
                databaseObj.myConnection.Open();
                FileStream fstream = File.OpenRead(filename);
                byte[] contents = new byte[fstream.Length];
                fstream.Read(contents, 0, (int)fstream.Length);
                fstream.Close();
                string[] temp = filename.Split(new string[] { "\\" }, StringSplitOptions.None);
                using (dbaseCommand = new SQLiteCommand("INSERT INTO documents (files,filename) VALUES (@files,@filename)", databaseObj.myConnection))
                {
                    dbaseCommand.Parameters.AddWithValue("@files",contents);
                    dbaseCommand.Parameters.AddWithValue("@filename", temp[temp.Length-1]);
                    dbaseCommand.ExecuteNonQuery();
                }
                databaseObj.myConnection.Close();
                onFileInsert(temp[temp.Length - 1]);
            }
            catch(Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Uploading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bunifuPages1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            SETTING.SetPage(0);
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            SETTING.SetPage(1);
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            SETTING.SetPage(3);
        }

        private void notification(string title,string text)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.Size = new Size(210, 100);
            popup.Image = Properties.Resources.ios;
            popup.TitleText = title;
            popup.ContentText = text;
            popup.Popup();
        }
   
        public void Preview ()
        {
            try
            {
                bool em = false;
                databaseObj.myConnection.Open();
                using (dbaseCommand = new SQLiteCommand("SELECT files FROM documents WHERE id=@ID;", databaseObj.myConnection))
                {
                    dbaseCommand.Parameters.AddWithValue("@ID", dgvPrivateFiles.Rows[Index].Cells[2].Value);
                    using (reader = dbaseCommand.ExecuteReader(CommandBehavior.Default))
                    {
                        if (reader.Read())
                        {
                            em = true;
                            byte[] fileData = (byte[])reader.GetValue(0);
                            Picbox.Image = (Bitmap)((new ImageConverter()).ConvertFrom(fileData));
                        }
                        if (em == false)
                        {
                            MessageBox.Show("NO Data", "ERROR");
                        }
                        reader.Close();
                    }
                }
                databaseObj.myConnection.Close();
            }
            catch (Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton9_Click(object sender, EventArgs e)
        {
            try
            {
                string filename ="";

                var thread = new Thread(new ParameterizedThreadStart(param => {

                    using (OpenFileDialog dlg = new OpenFileDialog())
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            filename = dlg.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }
                }));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                while(thread.IsAlive)
                {

                }
                if (filename!="")
                {
                    UploadFiles(filename);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Uploading Error");
            }
        }

        public void DownloadFiles(string filename)
        {
            try
            {
                bool em = false;
                databaseObj.myConnection.Open();
                using (dbaseCommand = new SQLiteCommand("SELECT files FROM documents WHERE id=@ID;", databaseObj.myConnection))
                {
                    dbaseCommand.Parameters.AddWithValue("@ID", dgvPrivateFiles.Rows[Index].Cells[2].Value);
                    using(reader = dbaseCommand.ExecuteReader(CommandBehavior.Default))
                    {
                        if (reader.Read())
                        {
                            em = true;
                            byte[] fileData = (byte[])reader.GetValue(0);
                            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
                            {
                                using (BinaryWriter bw = new BinaryWriter(fs))
                                {
                                    bw.Write(fileData);
                                    bw.Close();
                                }
                            }

                        }
                        if(em == false)
                        {
                            MessageBox.Show("NO Data", "ERROR");
                        }
                        reader.Close();
                    }
                }
                databaseObj.myConnection.Close();
            }
            catch(Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton10_Click(object sender, EventArgs e)
        {
            try
            {
                if (Index != -1)
                {    
                    string temp = dgvPrivateFiles.Rows[Index].Cells[1].Value.ToString();
                    string path = Directory.GetCurrentDirectory() + "\\PrivateFiles\\" + temp;
                    DownloadFiles(path);

                    notification("Success", "File Downloaded");
                    Index = -1;
                }
                else
                {
                    MessageBox.Show("You have not selected any file", "Info",MessageBoxButtons.OK ,MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Downloading ERROR");
            }
        }

        private void dgvPrivateFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Index = e.RowIndex;
            string temp = dgvPrivateFiles.Rows[Index].Cells[1].Value.ToString();
            if (temp.Contains(".jpeg") || temp.Contains(".png") || temp.Contains(".jpg"))
            {
                Preview();
            }
            else
            {
                Picbox.Image = null;
            }
        }

    

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            if (bunifuButton8.Text == "Lock")
            {
                notification("Security Notification", "Admin Logged Out Succesfully");
                bunifuButton8.Text = "Unlock";
                this.tableLayoutPanel12.Visible = false;
            }
                
            else if (bunifuButton8.Text == "Unlock")
            {
                Form2 frm = new Form2();
                frm.ShowDialog();
                bool status = frm.Status;
                if (status == true)
                {
                    notification("Security Notification", "Admin Logged In Succesfully");
                    bunifuButton8.Text = "Lock";
                    UpdateFiles();
                    this.tableLayoutPanel12.Visible = true;
                }   
            }
        }

        private void bunifuButton11_Click(object sender, EventArgs e)
        {
            
        }

        private void onInsert()
        {
            try
            {
                int C = dgvData.RowCount;
                dgvData.Rows.Add();
                if(C < 0)
                {
                    C = 0;
                }
                dgvData.Rows[C].Cells[0].Value = 0;
                dgvData.Rows[C].Cells[2].Value = dpData.Value;
                dgvData.Rows[C].Cells[3].Value = txtDataNarration.Text;
                dgvData.Rows[C].Cells[4].Value = txtDataDebit.Text;
                dgvData.Rows[C].Cells[5].Value = txtDataCredit.Text;
                dgvData.Rows[C].Cells[6].Value = dgvDataClients.Rows[ClientSelected].Cells[2].Value;
                databaseObj.myConnection.Open();
                query = "SELECT ID FROM myAccounts ORDER BY ID DESC LIMIT 1;";
                using(dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                {
                    reader = dbaseCommand.ExecuteReader();
                    reader.Read();
                    dgvData.Rows[C].Cells[1].Value = reader["ID"];
                    reader.Close();
                }
                databaseObj.myConnection.Close();
                query = "";
                if(C < 1)
                {
                    dgvData.Rows[C].Cells[7].Value = lblDataBalance.Text;
                }
                else
                {
                    dgvData.Rows[C].Cells[7].Value = long.Parse(lblDataBalance.Text) + long.Parse(dgvData.Rows[C - 1].Cells[7].Value.ToString());
                }

                lblDataTotal.Text = dgvData.Rows[C].Cells[7].Value.ToString();
                for (int i = 0; i < dgvData.RowCount; i++)
                {
                    dgvData.Rows[i].Cells[0].Value = int.Parse(dgvData.Rows[i].Cells[0].Value.ToString()) + 1;
                }
                if(bunifuButton1.Text == "View all" && dgvData.RowCount > 10)
                {
                    dgvData.Rows.RemoveAt(0);
                }
            }
            catch(Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Insert Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bunifuButton12_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDataNarration.Text != "" && ClientSelected !=-1)
                {
                    if(txtDataCredit.Text.Length <= 12 && txtDataDebit.Text.Length <= 12)
                    {
                        databaseObj.myConnection.Open();
                        if (txtDataCredit.Text == "")
                        {
                            txtDataCredit.Text = "0";
                        }
                        if (txtDataDebit.Text == "")
                        {
                            txtDataDebit.Text = "0";
                        }
                        query = "insert into myAccounts(DateAdded, Narration, Credit, Debit, NameID) values (@DateAdded, @Narration, @Credit, @Debit, @name);";
                        using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                        {
                            dbaseCommand.Parameters.AddWithValue("@DateAdded", dpData.Value.ToString());
                            dbaseCommand.Parameters.AddWithValue("@Narration", txtDataNarration.Text);
                            dbaseCommand.Parameters.AddWithValue("@Credit", txtDataCredit.Text);
                            dbaseCommand.Parameters.AddWithValue("@Debit", txtDataDebit.Text);
                            dbaseCommand.Parameters.AddWithValue("@name", dgvDataClients.Rows[ClientSelected].Cells[2].Value.ToString());

                            dbaseCommand.ExecuteNonQuery();
                            databaseObj.myConnection.Close();
                            dbaseCommand.CommandText = "";
                            query = "";
                            btnupdate.Visible = false;
                            btndel.Visible = false;
                            onInsert();
                            clearmain();
                        }
                    }
                    else
                    {
                        notification("Memory Error", "The Credit or Debit Value is to large" );
                    }
                }
                else
                {
                    notification("Insert Data Error", "Narration is empty or Client is not selected");
                }
            }
            catch (Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Insert Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     

        private void txtDataCredit_TextChange(object sender, EventArgs e)
        {
            try
            {
                if(txtDataCredit.Text == "" && txtDataDebit.Text=="")
                {
                    lblDataBalance.Text = "";
                }
                else if(txtDataDebit.Text =="")
                {
                    lblDataBalance.Text = (0 - long.Parse(txtDataCredit.Text)).ToString();
                }
                else if(txtDataCredit.Text == "")
                {
                    lblDataBalance.Text = txtDataDebit.Text;
                }
                else
                {
                    lblDataBalance.Text = (long.Parse(txtDataDebit.Text) - long.Parse(txtDataCredit.Text)).ToString();
                }
            }
            catch
            {
            }
        }

        private void Accounts_FormClosed(object sender, FormClosedEventArgs e)
        {
            databaseObj.myConnection.Close();
            databaseObj.myConnection.Dispose();
            dbaseCommand.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            this.DialogResult = DialogResult.OK;
        }

        private void passchange_Click(object sender, EventArgs e)
        {
            Check = Dncrypt();
            string newpassword = "";
            if (CPW.Text == Check)
            {
                if(NPW.Text == NP2.Text )
                {
                    newpassword = NPW.Text;
                    string fileName = @"Bunifu.bat";
                    byte[] stringbytes = Encoding.UTF8.GetBytes(newpassword);
                    string result = System.Convert.ToBase64String(stringbytes);
                    using (StreamWriter writers = new StreamWriter(fileName))
                    {
                        writers.WriteLine(result);
                    }
                    notification("Password Security", "Admin Password Changed Succesfully");
                    Clearpswd();
                }
                else
                {
                    MessageBox.Show("Please Enter Same Password in both fields");
                    Clearpswd2();
                }
            }
            else
            {
                MessageBox.Show("Incorect Passwords");
                Clearpswd();
            }
        }

        private void Accounts_Load(object sender, EventArgs e)
        {

        }

        private void onFileDelete(int idx)
        {
            dgvPrivateFiles.Rows.RemoveAt(idx);
            for(int i = idx; i< dgvPrivateFiles.RowCount; i++)
            {
                dgvPrivateFiles.Rows[i].Cells[0].Value = int.Parse(dgvPrivateFiles.Rows[i].Cells[0].Value.ToString()) - 1;
            }
        }

        private void bunifuButton4_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Index != -1)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete  " + dgvPrivateFiles.Rows[Index].Cells[1].Value.ToString() , "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        databaseObj.myConnection.Open();
                        query = "Delete FROM documents WHERE id=@ID;";
                        using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                        {
                            dbaseCommand.Parameters.AddWithValue("@ID", dgvPrivateFiles.Rows[Index].Cells[2].Value.ToString());
                            dbaseCommand.ExecuteNonQuery();
                            databaseObj.myConnection.Close();
                            this.Picbox.Image = null;
                            onFileDelete(Index);
                        }
                        Index = -1;
                    } 
                }
                else
                {
                    MessageBox.Show("You have not selected any file", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Deletion ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bunifuButton3_Click_1(object sender, EventArgs e)
        {
            SETTING.SetPage(1);
        }

      

        private void bunifuButton1_Click_1(object sender, EventArgs e)
        {
            SETTING.SetPage(3);
        }

        private void CPW_Enter(object sender, EventArgs e)
        {
            if (CPW.Text == "Enter Current Password")
            CPW.Text = String.Empty;
        }

        private void NPW_Enter(object sender, EventArgs e)
        {
            if (NPW.Text == "Enter New Password")
                NPW.Text = String.Empty;
        }

        private void NP2_Enter(object sender, EventArgs e)
        {

            if (NP2.Text == "Re-enter New Password")
                NP2.Text = String.Empty;
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            Clearpswd();
        }

        private void Clearpswd()
        {
            CPW.Text = String.Empty;
            NPW.Text = String.Empty;
            NP2.Text = String.Empty;
            NPW.isPassword = false;
            NP2.isPassword = false;
            CPW.isPassword = false;
            CPW.Text = "Enter Current Password";
            NPW.Text = "Enter New Password";
            NP2.Text = "Re-enter New Password";

        }

        private void Clearpswd2()
        {
           
            NPW.Text = String.Empty;
            NP2.Text = String.Empty;
            NPW.isPassword = false;
            NP2.isPassword = false;
            NPW.Text = "Enter New Password";
            NP2.Text = "Re-enter New Password";


        }

        private void CPW_OnValueChanged(object sender, EventArgs e)
        {
            if (CPW.Text != "Enter Current Password")
            {
                CPW.isPassword = true;
            }
        }

        private void NPW_OnValueChanged(object sender, EventArgs e)
        {
            if (NPW.Text != "Enter New Password")
            {
                NPW.isPassword = true;
            }
        }

        private void NP2_OnValueChanged(object sender, EventArgs e)
        {
            if (NP2.Text != "Re-enter New Password")
            {
                NP2.isPassword = true;
            }
        }

        private void bunifuToggleSwitch1_OnValuechange(object sender, EventArgs e)
        {
            if(bunifuToggleSwitch1.Value == true)
            {
                if (NP2.Text != "Re-enter New Password")
                {
                    NP2.isPassword = false;
                }
            }
            else
            {
                if (NP2.Text != "Re-enter New Password")
                {
                    NP2.isPassword = true;
                }
            }
        }

        private void bunifuToggleSwitch3_OnValuechange(object sender, EventArgs e)
        {
            if (bunifuToggleSwitch3.Value == true)
            {
                if (CPW.Text != "Enter Current Password")
                {
                    CPW.isPassword = false;
                }
            }
            else
            {
                if (CPW.Text != "Enter Current Password")
                {
                    CPW.isPassword = true;
                }
            }
        }

        private void bunifuToggleSwitch2_OnValuechange(object sender, EventArgs e)
        {
            if (bunifuToggleSwitch2.Value == true)
            {
                if (NPW.Text != "Enter New Password")
                {
                    NPW.isPassword = false;
                }
            }
            else
            {
                if (NPW.Text != "Enter New Password")
                {
                    NPW.isPassword = true;
                }
            }
        }

        private void txtDataNarration_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void dpDate_KeyUp(object sender, KeyEventArgs e)
       {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

    
        private void txtDataCredit_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void bunifuButton12_KeyUp(object sender, KeyEventArgs e)
        {
            
                if (e.KeyCode == Keys.Enter)
                {
        bunifuButton12_Click(this, new EventArgs());
             }
        }

        private void txtDataDebit_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void b1_Click(object sender, EventArgs e)
        {
            this.b1.BackColor = System.Drawing.SystemColors.Highlight;
            this.b2.BackColor = System.Drawing.Color.DodgerBlue;
            this.b3.BackColor = System.Drawing.Color.DodgerBlue;
            SETTING.SetPage(0);

        }

        private void b2_Click(object sender, EventArgs e)
        {
            this.b2.BackColor = System.Drawing.SystemColors.Highlight;
            this.b3.BackColor = System.Drawing.Color.DodgerBlue;
            this.b1.BackColor = System.Drawing.Color.DodgerBlue;
            SETTING.SetPage(2);
        }

        private void b3_Click(object sender, EventArgs e)
        {
            this.b3.BackColor = System.Drawing.SystemColors.Highlight;
            this.b2.BackColor = System.Drawing.Color.DodgerBlue;
            this.b1.BackColor = System.Drawing.Color.DodgerBlue;
            SETTING.SetPage(4);
        }

        private void dgvDataClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.RowIndex > -1 && ClientSelected != e.RowIndex)
                {
                    DeletingClient = -1;
                    btnclientdel.Enabled = false;
                    bunifuButton2.Enabled = false;
                    isUptoDate = false;
                    inUse = false;
                    ClientSelected = e.RowIndex;
                    lblclientDesc.Text = dgvDataClients.Rows[e.RowIndex].Cells[1].Value.ToString();
                    lblledger.Text = dgvDataClients.Rows[e.RowIndex].Cells[0].Value.ToString();
                    lblDataTotal.Text = "00";
                    if (bunifuButton1.Text == "View all")
                    {
                        UpdateForm(false);
                    }
                    else
                    {
                        UpdateForm(true);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Loading Client ERROR", MessageBoxButtons.OK ,MessageBoxIcon.Error);
            }
        }

        private void bunifuButton1_Click_2(object sender, EventArgs e)
        {
            if (bunifuButton1.Text == "View all")
            {
                Form2 frm = new Form2();
                frm.ShowDialog();
                bool status = frm.Status;
                if (status == true)
                {
                    notification("Security Notification", "Admin Logged In Succesfully");
                    try
                    {
                        if(ClientSelected != -1)
                        {
                            UpdateForm(true);
                        }
                        query = "";
                        bunifuButton1.Text = "Logout";
                    }
                    catch (Exception ex)
                    {
                        databaseObj.myConnection.Close();
                        MessageBox.Show(ex.Message, "Form Load Error");
                    }
                }
            }
            else if (bunifuButton1.Text == "Logout")
            {
                notification("Security Notification", "Admin Logged Out Succesfully");
                try
                {
                    if (ClientSelected != -1)
                    {
                        UpdateForm(false);
                    }
                    bunifuButton1.Text = "View all";
                }
                catch (Exception ex)
                {
                    databaseObj.myConnection.Close();
                    MessageBox.Show(ex.Message, "Form Load Error");
                }
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.RowIndex > -1)
                {
                    this.btnupdate.Visible = true;
                    this.btndel.Visible = true;
                    idx = dgvData.Rows[e.RowIndex].Cells[1].Value.ToString();
                    Index = e.RowIndex;
                    dpData.Value = DateTime.Parse(dgvData.Rows[e.RowIndex].Cells[2].Value.ToString());
                    txtDataNarration.Text = dgvData.Rows[e.RowIndex].Cells[3].Value.ToString();
                    txtDataDebit.Text = dgvData.Rows[e.RowIndex].Cells[4].Value.ToString();
                    txtDataCredit.Text = dgvData.Rows[e.RowIndex].Cells[5].Value.ToString();
                    lblDataBalance.Text = "00";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Data Grid Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

   

        private void bunifuMaterialTextbox1_OnValueChanged(object sender, EventArgs e)
        {
           

        }


        private void txtDataSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(txtDataSearch.Text == "Search For Client")
            {
                txtDataSearch.Text = null;
            }
        }




        private void Adminpass_Click(object sender, EventArgs e)
        {
            if(skt.Text == "93ex7fhc")
            {
                if(cpt.Text == Dnucrypt())
                {
                    if(npt.Text == cnpt.Text)
                    {
                        string fileName = @"Config.bat";
                        byte[] stringbytes = Encoding.UTF8.GetBytes(npt.Text);
                        string result = System.Convert.ToBase64String(stringbytes);
                        using (StreamWriter writers = new StreamWriter(fileName))
                        {
                            writers.WriteLine(result);
                        }
                        notification("Password Security", "User Password Changed Succesfully");
                        clearupass();
                    }
                    else
                    {
                        MessageBox.Show("New passwords should be same in both fields", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Current Password is Invalid", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Security Key is Invalid Kindly Enter Correct Key or Contact Developers.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clearupass();
            }
        }

   
        private void skt_Enter(object sender, EventArgs e)
        {
            if(skt.Text == "Enter Security Key")
            {
                skt.Text = null;
                skt.isPassword = true;
            }
        }

        private void skt_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(skt.Text))
            {
                skt.Text = "Enter Security Key";
                this.skt.isPassword = false;
            }
        }

        private void skt_OnValueChanged(object sender, EventArgs e)
        {
            if(skt.Text != "Enter Security Key" )
            {
                this.skt.isPassword = true;
            }
        }

        private void upassswitch_OnValueChange(object sender, EventArgs e)
        {
            if (upassswitch.Value == false)
            {
                if (skt.Text != "Enter Security Key"){this.skt.isPassword = true;}
                if(npt.Text != "Enter New Password"){this.npt.isPassword = true;}
                if(cpt.Text != "Enter Current Password"){this.cpt.isPassword = true;}
                if(cnpt.Text != "Re-enter New Password"){this.cnpt.isPassword = true;}
            }
            else
            {
                this.skt.isPassword = false;
                this.npt.isPassword = false;
                this.cpt.isPassword = false;
                this.cnpt.isPassword = false;
            }
            }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            clearupass();
        }

        private void clearupass()
        {
            skt.Text = "Enter Security Key";
            npt.Text = "Enter New Password";
            cpt.Text = "Enter Current Password";
            cnpt.Text = "Re-enter New Password";
            this.skt.isPassword = false;
            this.npt.isPassword = false;
            this.cpt.isPassword = false;
            this.cnpt.isPassword = false;
        }

        private void cpt_Enter(object sender, EventArgs e)
        {
            if (cpt.Text == "Enter Current Password")
            {
                cpt.Text = null;
                cpt.isPassword = true;
            }
        }

        private void npt_Enter(object sender, EventArgs e)
        {
            if (npt.Text == "Enter New Password")
            {
                npt.Text = null;
                npt.isPassword = true;
            }
        }

        private void cnpt_Enter(object sender, EventArgs e)
        {
            if (cnpt.Text == "Re-enter New Password")
            {
                cnpt.Text = null;
                cnpt.isPassword = true;
            }
        }

        private void cpt_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cpt.Text))
            {
                cpt.Text = "Enter Current Password";
                this.cpt.isPassword = false;
            }
        }

        private void npt_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(npt.Text))
            {
                npt.Text = "Enter New Password";
                this.npt.isPassword = false;
            }
        }

        private void cnpt_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cnpt.Text))
            {
                cnpt.Text = "Re-enter New Password";
                this.cnpt.isPassword = false;
            }
        }

        private void bunifuButton11_Click_1(object sender, EventArgs e)
        {
            clearmain();
        }

        private void clearmain()
        {
            txtDataNarration.Text = txtDataCredit.Text = txtDataDebit.Text  = null;
            lblDataBalance.Text = "--";
            btnupdate.Visible = false;
            btndel.Visible = false;
            Index = -1;
            idx = "-1";
        }

        public string RandomString()
        {
            var builder = new StringBuilder(6);
            Random random = new Random();
            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset =  'a';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < 6; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return builder.ToString();
        }

        private void txtDataSearch_OnValueChanged(object sender, EventArgs e)
        {
            try
            {
                databaseObj.myConnection.Open();
                dgvDataClients.Rows.Clear();
                query = "SELECT Name,Description, ID FROM Clients WHERE Name LIKE @name";
                using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                {
                    dbaseCommand.Parameters.AddWithValue("@name", "%" + txtDataSearch.Text + "%");
                    reader = dbaseCommand.ExecuteReader();
                    int i = 0;
                    while (reader.Read())
                    {
                        dgvDataClients.Rows.Add();
                        dgvDataClients.Rows[i].Cells[0].Value = reader["Name"];
                        dgvDataClients.Rows[i].Cells[1].Value = reader["Description"];
                        dgvDataClients.Rows[i].Cells[2].Value = reader["ID"];
                        i++;
                    }
                }

                reader.Close();
                databaseObj.myConnection.Close();
            }
            catch (Exception ex)
            {
                reader.Close();
                databaseObj.myConnection.Close();
                MessageBox.Show(ex.Message, "Searching ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void onDelete()
        {
            try
            {
                int i = Index;
                lblDataTotal.Text = (long.Parse(lblDataTotal.Text) - (long.Parse(dgvData.Rows[i].Cells[4].Value.ToString()) - long.Parse(dgvData.Rows[i].Cells[5].Value.ToString()))).ToString();
                if(i < 1)
                {
                    long last = 0;
                    for(int j = i+1; j<dgvData.RowCount; j++)
                    {
                        last += (long.Parse(dgvData.Rows[j].Cells[4].Value.ToString()) - long.Parse(dgvData.Rows[j].Cells[5].Value.ToString()));
                        dgvData.Rows[j].Cells[7].Value = last;
                    }
                }
                else if(i == dgvData.RowCount-1)
                {
                    for(int j = i-1; j>-1; j--)
                    {
                        dgvData.Rows[j].Cells[0].Value = int.Parse(dgvData.Rows[j].Cells[0].Value.ToString()) - 1;
                    }
                }
                else
                {
                    long last = long.Parse(dgvData.Rows[i - 1].Cells[7].Value.ToString());

                    for (int j = i + 1; j < dgvData.RowCount; j++)
                    {
                        last += (long.Parse(dgvData.Rows[j].Cells[4].Value.ToString()) - long.Parse(dgvData.Rows[j].Cells[5].Value.ToString()));
                        dgvData.Rows[j].Cells[7].Value =  last;
                    }
                    for (int j = i - 1; j > -1; j--)
                    {
                        dgvData.Rows[j].Cells[0].Value = int.Parse(dgvData.Rows[j].Cells[0].Value.ToString()) - 1;
                    }
                }
                dgvData.Rows.RemoveAt(i);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bunifuButton3_Click_2(object sender, EventArgs e)
        {
            try
            {
                if (Index != -1)
                {  
                    DialogResult result = MessageBox.Show("Are you sure you want to delete from Client: "+dgvDataClients.Rows[ClientSelected].Cells[0].Value+" Narration: "+dgvData.Rows[Index].Cells[3].Value, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(result == DialogResult.Yes)
                    {
                        databaseObj.myConnection.Open();
                        query = "Delete FROM myAccounts WHERE id=@ID;";
                        using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                        {
                            dbaseCommand.Parameters.AddWithValue("@ID", dgvData.Rows[Index].Cells[1].Value);
                            dbaseCommand.ExecuteNonQuery();
                        }

                        databaseObj.myConnection.Close();
                        if (dgvData.Rows.Count == 0)
                        {
                            AddClientList();
                            lblledger.Text = "Ledger";
                            lblDataTotal.Text = "-";
                        }
                        else
                        {
                            onDelete();
                        }

                        clearmain();
                        Index = -1;
                    }
                }
                else
                {
                    MessageBox.Show("You have not selected any file", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void onUpdate()
        {
            try
            {
                int i = Index;
                dgvData.Rows[Index].Cells[3].Value = txtDataNarration.Text;
                dgvData.Rows[Index].Cells[2].Value = dpData.Value;
                dgvData.Rows[Index].Cells[4].Value = txtDataDebit.Text;
                dgvData.Rows[Index].Cells[5].Value = txtDataCredit.Text;
                if(Index > 0)
                {
                    dgvData.Rows[Index].Cells[7].Value = long.Parse(dgvData.Rows[Index - 1].Cells[7].Value.ToString()) + long.Parse(lblDataBalance.Text);
                }
                else
                {
                    dgvData.Rows[Index].Cells[7].Value = long.Parse(lblDataBalance.Text);
                }
                for(; i<dgvData.RowCount; i++)
                {
                    dgvData.Rows[i].Cells[7].Value = long.Parse(dgvData.Rows[i - 1].Cells[7].Value.ToString()) + (long.Parse(dgvData.Rows[i].Cells[4].Value.ToString()) - long.Parse(dgvData.Rows[i].Cells[5].Value.ToString()));
                    lblDataTotal.Text = dgvData.Rows[i].Cells[7].Value.ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Updating Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bunifuButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (idx != "-1")
                {
                    databaseObj.myConnection.Open();
                    query = "UPDATE myAccounts SET Narration=@Narration, DateAdded=@DateAdded, Debit=@Debit, Credit=@Credit WHERE ID=@ID;";
                    using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                    {
                        dbaseCommand.Parameters.AddWithValue("@Narration", txtDataNarration.Text);
                        dbaseCommand.Parameters.AddWithValue("@DateAdded", dpData.Value.ToString());
                        dbaseCommand.Parameters.AddWithValue("@Debit", txtDataDebit.Text);
                        dbaseCommand.Parameters.AddWithValue("@Credit", txtDataCredit.Text);
                        dbaseCommand.Parameters.AddWithValue("@ID", idx);
                        dbaseCommand.ExecuteNonQuery();
                    }
                    onUpdate();
                    clearmain();
                    databaseObj.myConnection.Close();
                    idx = "-1";
                }
                else
                {
                    MessageBox.Show("Please select the row you want to update by double clicking", "Updating Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Updating Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addnew_Click(object sender, EventArgs e)
        {
            using (Form4 Poppup = new Form4())
            {
                if (Poppup.ShowDialog() == DialogResult.OK)
                {
                    AddClientList();
                }
            }
        }

  

        private void btnclientdel_Click(object sender, EventArgs e)
        {
            try
            {
                if(DeletingClient !=-1)
                {
                    DialogResult result;
                    using(Form2 popup = new Form2())
                    {
                        popup.ShowDialog();
                        result = popup.DialogResult;
                    }

                    if(result == DialogResult.OK)
                    {
                        databaseObj.myConnection.Open();
                        query = "DELETE FROM myAccounts WHERE NameID=@NameID;";
                        using (dbaseCommand = new SQLiteCommand(query, databaseObj.myConnection))
                        {
                            dbaseCommand.Parameters.AddWithValue("@NameID", DeletingClient);
                            dbaseCommand.ExecuteNonQuery();
                            dbaseCommand.CommandText = "DELETE FROM Clients WHERE ID=@ID;";
                            dbaseCommand.Parameters.AddWithValue("@ID", DeletingClient);
                            dbaseCommand.ExecuteNonQuery();
                            dbaseCommand.Dispose();
                            query = "";
                            lblclientDesc.Text = "-";
                            lblDataTotal.Text = "-";
                            lblDataBalance.Text = "-";
                            lblledger.Text = "Ledger";
                            dgvData.Rows.Clear();
                            DeletingClient = -1;
                            btnclientdel.Enabled = false;
                            bunifuButton2.Enabled = false;
                            databaseObj.myConnection.Close();
                            AddClientList();
                        }
                    }
                    
                }
                else
                {
                    MessageBox.Show("Client not selected", "Selecting Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Client Deleting Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDataClients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                DeletingClient = int.Parse(dgvDataClients.Rows[e.RowIndex].Cells[2].Value.ToString());
                btnclientdel.Enabled = true;
                bunifuButton2.Enabled = true;
            }
        }

        private void dpData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

   
 
        private void bunifuButton12_KeyUp_1(object sender, KeyEventArgs e)
        {
            
        }

        private void CPW_Leave(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(CPW.Text))
            {
                CPW.Text = "Enter Current Password";
                CPW.isPassword = false;
            }
        }

        private void NPW_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NPW.Text))
            {
                NPW.Text = "Enter New Password";
                NPW.isPassword = false;
            }
        }

        private void NP2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NP2.Text))
            {
                NP2.Text = "Re-enter New Password";
                NP2.isPassword = false;
            }
        }

        private void bunifuButton2_Click_2(object sender, EventArgs e)
        {
            try
            {
                using (Form6 Poppup = new Form6(dgvDataClients.Rows[ClientSelected].Cells[0].Value.ToString(), lblclientDesc.Text, dgvDataClients.Rows[ClientSelected].Cells[2].Value.ToString()))
                {
                    if (Poppup.ShowDialog() == DialogResult.OK)
                    {
                        AddClientList();
                    }
                }
                ClientSelected = -1;
                btnclientdel.Enabled = false;
                bunifuButton2.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Client Editing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_Scroll(object sender, ScrollEventArgs e)
        {
            if (bunifuButton1.Text == "Logout")
            {
                if (dgvData.VerticalScrollingOffset == 0 && inUse == false && isUptoDate == false)
                {
                    LoadMoreEntries();
                }
            }
        }

        private void dgvPrivateFiles_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {

                int Height = dgvPrivateFiles.RowCount * dgvPrivateFiles.RowTemplate.Height;
                if (dgvPrivateFiles.VerticalScrollingOffset > (Height - dgvPrivateFiles.Height) && isUptoDatePrivate == false)
                {
                    LoadMoreFiles();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "File Loading ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }

}

