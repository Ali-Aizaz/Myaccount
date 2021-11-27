using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;


namespace Myaccount
{
    class Database
    {
        public SQLiteConnection myConnection;

        public Database()
        {
            string path = System.IO.Path.GetTempPath() + "System.db";
            myConnection = new SQLiteConnection("Data Source=" + path);
            if (!File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
                myConnection.Open();

                using (SQLiteCommand dbaseCommand = new SQLiteCommand("", myConnection))
                {
                    string Query = "CREATE TABLE \"documents\" ( \"ID\" INTEGER,\"fileKey\" Text,\"filename\"  TEXT,PRIMARY KEY(\"ID\" AUTOINCREMENT))";
                    dbaseCommand.CommandText = Query;
                    dbaseCommand.ExecuteNonQuery();
                    Query = "CREATE TABLE \"myAccounts\" (    \"id\"    INTEGER,	\"Narration\" TEXT NOT NULL,	\"DateAdded\" TEXT NOT NULL,	\"Debit\" INTEGER NOT NULL,	\"Credit\"    INTEGER NOT NULL, \"NameID\"  INTERGER NOT NULL,	PRIMARY KEY(\"id\" AUTOINCREMENT))";
                    dbaseCommand.CommandText = Query;
                    dbaseCommand.ExecuteNonQuery();
                    Query = "CREATE TABLE \"Clients\" ( \"ID\"    INTEGER,\"Name\" Text,\"Description\"  TEXT,\"Balance\" INTERGER,PRIMARY KEY(\"ID\" AUTOINCREMENT))";
                    dbaseCommand.CommandText = Query;
                    dbaseCommand.ExecuteNonQuery();
                    Query = "CREATE TRIGGER insertEntry AFTER INSERT ON myAccounts BEGIN UPDATE Clients SET Balance =( Balance + NEW.Debit - New.Credit) WHERE ID = NEW.NameID; END;";
                    dbaseCommand.CommandText = Query;
                    dbaseCommand.ExecuteNonQuery();
                    Query = "CREATE TRIGGER UpdateEntry AFTER UPDATE ON myAccounts BEGIN UPDATE Clients SET Balance =( Balance - (OLD.Debit - OLD.Credit) + (NEW.Debit - NEW.Credit)) WHERE ID = NEW.NameID; END;";
                    dbaseCommand.CommandText = Query;
                    dbaseCommand.ExecuteNonQuery();
                    Query = "CREATE TRIGGER deleteEntry AFTER DELETE ON myAccounts BEGIN UPDATE Clients SET Balance =( Balance - (OLD.Debit - OLD.Credit)) WHERE ID = OLD.NameID; END;";
                    dbaseCommand.CommandText = Query;
                    dbaseCommand.ExecuteNonQuery();
                    dbaseCommand.Dispose();
                }
                myConnection.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
