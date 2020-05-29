using MySql.Data.MySqlClient;
using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;

namespace ProjectIFPossible.ConnectionRouter
{

    /*
     * Author : Surojit  Paul
     * Date created : 12. 4 . 2020
     * Last modified : 20 . 4 . 2020
     */
    class mySqlConnection : DataBaseConnectionBase
    {
        private static string ON_ENTRY_UID;
        private static string ON_ENTRY_PASSWORD;
        private static string PORT;
        private const string SERVER_NAME = "localhost"; // definately don't change this.
        private const string DATABASE_NAME = "MyDb"; //don't change this

        private readonly string csS = 
            "server =" + SERVER_NAME + ";" +
            "port = "+PORT+"; "+
            "database = " + DATABASE_NAME + ";" +
            "uid = " + ON_ENTRY_UID + ";" +
            "password = " + ON_ENTRY_PASSWORD + ";";

        private readonly string userIds;
        private readonly string userPass;
        public static string globalUser;
        private static  MySqlConnection rootOnlyConnection;
        public static MySqlConnection globalCon;
        
        
        public mySqlConnection()
        {
            LOGIN();
        }





        public mySqlConnection(string userIds, string userPass)
        {
            this.userIds = userIds;
            this.userPass = userPass;
        }





        public override void PrinLn()
        {
            Console.WriteLine("This is My Sql Class");
        }






        private bool LOGIN()
        {
            UANDP();
            string connectionString =
                "server =" + SERVER_NAME + ";"+
                "port= " + PORT + ";"+ 
                "database = " + DATABASE_NAME + ";" +
                "uid = " + ON_ENTRY_UID + ";" +
                "password = " + ON_ENTRY_PASSWORD + ";";
            rootOnlyConnection = new MySqlConnection(connectionString);
            rootOnlyConnection.Open();
            bool i = rootOnlyConnection.Ping();
            return i;
        }





        //TODO: Write a function inside the schema and remove this;
        private bool ValidateUserNameOnLogin()
        {
            var queryText = "select count(*) from userTable where username = @id";
            var getUserNameCommand = new MySqlCommand(queryText, rootOnlyConnection);
            getUserNameCommand.Parameters.AddWithValue("@id", userIds);
            object usFromDb = getUserNameCommand.ExecuteScalar();
            if (usFromDb != null)
            {
                int Ufromdb = Convert.ToInt16(usFromDb);
                if (Ufromdb==1)
                    return true;
            }
            return false;
        }






        //TODO: Write a function inside the schema and remove this;
        private bool ValidatePasswordOnLogin()
        {
            var queryText ="select password from userTable where username = @id ;";
			var getUserPasswordCommand = new MySqlCommand(queryText, rootOnlyConnection);
			getUserPasswordCommand.Parameters.AddWithValue("@id", userIds);               
			object passFromDb = getUserPasswordCommand.ExecuteScalar();
			if (passFromDb != null)
			{
                string Ufromdb = Convert.ToString(passFromDb);
				if (Ufromdb == userPass)
                    return true; 	
			}
			return false;
        }







        public bool ValidateConnection()
        {
            if (ValidatePasswordOnLogin() && ValidateUserNameOnLogin())
                return true;
            return false;
        }






        public void Connect()
        {
            if (ValidateConnection()) 
            {
                globalCon = new MySqlConnection(csS);
                globalCon.Open();
                globalUser = userIds;
            }
        }

        public static void DisConnect()
        {
            if (globalCon.Ping())
            { 
                globalCon.Close(); 
                globalUser = ""; 
            }
        }

       





        //This Function Call at first
        private void UANDP()
        {
            string u = "";
            string pas = "";
            string po = "";
            var col = new List<string>(2);
            using (var rd = new StreamReader("xco.csv"))
            {
                while(!rd.EndOfStream)
                {
                    var lines =rd.ReadLine().Split(':');
                    col.Add(lines[1]);
                }
                u = col[0];
                pas = col[1];
                po = col[2];
            }

            ON_ENTRY_UID = u;
            ON_ENTRY_PASSWORD = pas;
            PORT = po;
        }






    }
}
