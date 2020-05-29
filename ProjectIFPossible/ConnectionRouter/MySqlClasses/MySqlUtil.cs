using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace ProjectIFPossible.ConnectionRouter.MySqlClasses
{
   
    class MySqlUtil: mySqlConnection
    {


        public static int MediCineCount()
        {
            int Value=0;
            var cmd = new MySqlCommand("select Total_Medicine_In_Stock from MedicineStockStatusView2;", globalCon);
            var oot = cmd.ExecuteScalar();
            if (oot != DBNull.Value)
            {
                 Value = Convert.ToInt16(cmd.ExecuteScalar());
            }
            return Value;
        }


        public static List<string> MEDICINE_NAMES_LIST()
        {
            List<string> namelist = new List<string>(100);
            MySqlCommand cmd = new MySqlCommand("select medName from mednametable order by medName;", globalCon);
            MySqlDataReader rd = cmd.ExecuteReader();
            while(rd.Read())
            {
                namelist.Add(rd.GetString(0));
            }
            rd.Close();
            return namelist;
        }
    }
}

