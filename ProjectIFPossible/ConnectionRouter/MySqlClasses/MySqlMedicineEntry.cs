using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses
{
    class MySqlMedicineEntry : mySqlConnection
    {
        private readonly MySqlConnection msc = globalCon;
        private const string MEDCINE_TABLE_NAME = "mednametable";
        private const string MEDCINE_MANUFACTURE_TABLE = "manufacturedetailstable"; 
        public string MediName { get; }
        public string MediManfName { get; }
        public int price { get; }
        private string user=globalUser;
        private string date;
        public MySqlMedicineEntry(string medName, string medmafna, string date, int price)
        {
            MediName = medName;
            MediManfName = medmafna;
            user = globalUser;
            this.date = date;
            this.price = price;
        }

        private bool IsEmpty()
        {
            if ((MediName.Length==0) || (MediManfName.Length==0)) return true;
            return false;
        }

        public bool validateManufactName()
        {
            
            var cmd = new MySqlCommand("select count(*) from manufacturedetailstable where manfName = @manfName;", msc);
            cmd.Parameters.AddWithValue("@manfName", MediManfName);
            var Value = Convert.ToByte(cmd.ExecuteScalar());
            if (Value == 1) return true;
            return false;
        }

        public bool SaveAndCommit()
        {
          

            if(validateManufactName())
            {
                var cmd = new MySqlCommand("insert into "+MEDCINE_TABLE_NAME +" values(@mN, @mfN, @usr, @date, @price); commit;", msc);
                cmd.Parameters.AddWithValue("@mN", MediName);
                cmd.Parameters.AddWithValue("@mfN", MediManfName);
                cmd.Parameters.AddWithValue("@usr", user);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.ExecuteNonQuery();
                return true;
            }
            return false;
        }

        public static List<MySqlMedicineEntry> GetMedicineList()
        {
            List<MySqlMedicineEntry> mList = new List<MySqlMedicineEntry>(100);
            string qText = "select * from " + MEDCINE_TABLE_NAME + " order by medname;";
            MySqlCommand cmd = new MySqlCommand(qText, globalCon);
            MySqlDataReader mdR = cmd.ExecuteReader();
            while(mdR.Read())
            {
                string mN = mdR.GetString(0);
                string mfN = mdR.GetString(1);
                string date = mdR.GetString(2);
                string ur = mdR.GetString(3);
                var mPrice = mdR.GetInt16(4);
                MySqlMedicineEntry mddE = new MySqlMedicineEntry(mN, mfN, date, mPrice)
                {
                    user = ur
                };
                mList.Add(mddE);
            }
            mdR.Close();
            return mList;
        }

        public static int MediCineCount()
        {
            var cmd = new MySqlCommand("select count(*) from "+ MEDCINE_TABLE_NAME+";", globalCon);
            var Value = Convert.ToByte(cmd.ExecuteScalar());
            return Value;
        }
    }
}
