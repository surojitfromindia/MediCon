using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;

namespace TestProject.NewFolder1
{
    public class mySqlConnection 
    {
        private static string ON_ENTRY_UID = "root";
        private static string ON_ENTRY_PASSWORD = "password";
        private const string SERVER_NAME = "localhost";
        private const string DATABASE_NAME = "MyDb";

        private readonly string csS = "server =" + SERVER_NAME + ";" +
                "database = " + DATABASE_NAME + ";" +
                "uid = " + ON_ENTRY_UID + ";" +
                "password = " + ON_ENTRY_PASSWORD + ";";

        private readonly string userIds;
        private readonly string userPass;
        public static string globalUser;
        private static  MySqlConnection rootOnlyConnection;
        public static MySqlConnection gloabalCon;
        public mySqlConnection()
        {
            LOGIN();
            //Connect();
        }

        public mySqlConnection(string userIds, string userPass)
        {
            this.userIds = userIds;
            this.userPass = userPass;
        }


        private bool LOGIN()
        {
            UANDP();
            string connectionString =
                "server =" + SERVER_NAME + ";" +
                "database = " + DATABASE_NAME + ";" +
                "uid = " + ON_ENTRY_UID + ";" +
                "password = " + ON_ENTRY_PASSWORD + ";";
            rootOnlyConnection = new MySqlConnection(connectionString);
            rootOnlyConnection.Open();
            bool i = rootOnlyConnection.Ping();
            
            return i;
        }

        private bool ValidateUserNameOnLogin()
        {
            var queryText = "select username from userTable";
            var getUserNameCommand = new MySqlCommand(queryText, rootOnlyConnection);
            object usFromDb = getUserNameCommand.ExecuteScalar();
            if (usFromDb != null)
            {
                string Ufromdb = Convert.ToString(usFromDb);
                if (Ufromdb.Equals(userIds))
                    return true;
            }
            return false;
        }


        private bool ValidatePasswordOnLogin()
        {
            if (userIds != "true")
            {
                var queryText ="select password from userTable where username =@userIds";
                var getUserPasswordCommand = new MySqlCommand(queryText, rootOnlyConnection);
                getUserPasswordCommand.Parameters.AddWithValue("@userIds", userIds);               
                object passFromDb = getUserPasswordCommand.ExecuteScalar();
                if (passFromDb != null)
                {
                    string Ufromdb = Convert.ToString(passFromDb);
                    if (Ufromdb == userPass)
                        return true;                    
                }
            }
            return false;
        }


        public bool ValidateConnection()
        {
            if (ValidateUserNameOnLogin() && ValidatePasswordOnLogin())
            {
                return true; 
            }
            return false;
        }

        public void Connect()
        {
            if (ValidateConnection()) 
            {
                gloabalCon = new MySqlConnection(csS);
                gloabalCon.Open();
                globalUser = userIds;
            }
        }

        public void DisConnect()
        {
            if(gloabalCon.Ping())
                gloabalCon.Close();
        }

        private void UANDP()
        {
            string u = "";
            string p = "";
            var col = new List<string>(2);
            using (var rd = new StreamReader("xco.csv"))
            {
                while(!rd.EndOfStream)
                {
                    var lines =rd.ReadLine().Split(':');
                    col.Add(lines[1]);

                }
                u = col[0];
                p = col[1];
            }

            ON_ENTRY_UID = u;
            ON_ENTRY_PASSWORD = p;
        }

        public static List<string> GetMedicineList()
        {
            List<string> mList = new List<string>(100);
            string qText = "select medname from mednametable order by medname;";
            MySqlCommand cmd = new MySqlCommand(qText, gloabalCon);
            MySqlDataReader mdR = cmd.ExecuteReader();
            while (mdR.Read())
            {
                string mN = mdR.GetString(0);
                mList.Add(mN);
            }
            mdR.Close();
            return mList;
        }


    }
}
