using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProjectIFPossible.ConnectionRouter
{
    class ConnectionAndDatabaseValidator
    {

        //This will just some static class and  static variabls;
        private  string dbname = "MyDb";
        private string UserRootID = "";
        private string UserRootPass = "";
        private string ServerPort = "";
        private string connectioValidityString = "";
        private string dbValidityString = "";

        public ConnectionAndDatabaseValidator(string u, string p, string port)
        {
            UserRootID = u;
            UserRootPass = p;
            ServerPort = port;
            connectioValidityString = $"uid = {UserRootID}; password = {UserRootPass}; port ={ServerPort};";
            dbValidityString = $"uid = {UserRootID}; password = {UserRootPass}; port ={ServerPort}; database={dbname}";
        }

        public bool CheckConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectioValidityString);
            try
            {
                connection.Open();
            }
            catch(MySqlException e)
            {
                Console.WriteLine(e.ErrorCode);
                connection.Close();
            }
            return connection.Ping();
        }

        public bool DBConnection()
        {
            //Always Check Db Connection From already valid/succesfully establish connection
            //thats why use UANDP(); which load connection data from xco.csv file which is valid
            UANDP();
            MySqlConnection connection = new MySqlConnection(dbValidityString);
            try
            {
                connection.Open();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ErrorCode);
                connection.Close();
            }
            return connection.Ping();
        }

        public bool WriteUserMasterLoginInfo()
        {
            //Overwrite currently store data only if the 
            //data taken from textboxs are valid.
            if(CheckConnection())
            {
                string j = $"username:{UserRootID}\r\npassword:{UserRootPass}\r\nport:{ServerPort}";
                File.Delete("settings\\xco.csv");
                File.WriteAllText("settings\\xco.csv", j);
                return true;
            }
            return false;
        }

        public void  CreateDatabaseIfNotExsist()
        {
            UANDP();
            var j = File.ReadAllText($"settings\\DataBaseQu.sql");
            MySqlConnection con = new MySqlConnection(connectioValidityString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand(j, con);
            var rowEffected = cmd.ExecuteNonQuery();
            con.Close();
            
        }

        private void UANDP()
        {
            string u = "";
            string pas = "";
            string po = "";
            var col = new List<string>(2);
            using (var rd = new StreamReader("settings\\xco.csv"))
            {
                while (!rd.EndOfStream)
                {
                    var lines = rd.ReadLine().Split(':');
                    col.Add(lines[1]);
                }
                u = col[0];
                pas = col[1];
                po = col[2];
            }
            UserRootID = u;
            UserRootPass = pas;
            ServerPort = po;
        }
    }


    
       
}
