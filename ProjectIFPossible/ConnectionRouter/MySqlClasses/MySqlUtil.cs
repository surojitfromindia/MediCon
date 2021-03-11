using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace ProjectIFPossible.ConnectionRouter.MySqlClasses
{

    class MySqlUtil : mySqlConnection
    {
        public static string max_price_med_name = "";
        public static string max_price;
        public static string min_price_med_name = "";
        public static string min_price;


        public static string max_stocked_med_name = "";
        public static string max_stocked;
        public static string min_stocked_med_name = "";
        public static string min_stocked;

        public static string max_batch_med_name = "";
        public static string max_batch;
        public static string min_batch_med_name = "";
        public static string min_batch;

        public static string totalMedicineWorth = "";

        public static int MediCineCount()
        {
            int Value = 0;
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
            while (rd.Read())
            {
                namelist.Add(rd.GetString(0));
            }
            rd.Close();
            return namelist;
        }

        public static void GEN_OVERVIEW_INFO()
        {
            MySqlCommand cmd = new MySqlCommand("select * from overviewview;", globalCon);
            MySqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                max_price_med_name = rd.GetString(0);
                max_price = rd.GetUInt16(1).ToString();
                min_price_med_name = rd.GetString(2);
                min_price = rd.GetUInt16(3).ToString();


                max_stocked_med_name = rd.GetString(4);
                max_stocked = rd.GetUInt16(5).ToString();
                min_stocked_med_name = rd.GetString(6).ToString();
                min_stocked = rd.GetUInt16(7).ToString();

                max_batch_med_name = rd.GetString(8);
                max_batch = rd.GetUInt16(9).ToString();
                min_batch_med_name = rd.GetString(10);
                min_batch = rd.GetUInt16(11).ToString();
            }
            rd.Close();
        }

        public static void TOTAL_MED_WORTH()
        {
            var cmd = new MySqlCommand("select sum(tprice) from InventoryPriceView;", globalCon);
            var oot = Convert.ToDouble(cmd.ExecuteScalar());
            totalMedicineWorth = oot.ToString();
        }


        public static int BatchCount()
        {
            int Value = 0;
            var cmd = new MySqlCommand("select Total_Batch from MedicineStockStatusView2;", globalCon);
            var oot = cmd.ExecuteScalar();
            if (oot != DBNull.Value)
            {
                Value = Convert.ToInt16(cmd.ExecuteScalar());
            }
            return Value;
        }
        public static int ExpBatchCount()
        {
            int Value = 0;
            var cmd = new MySqlCommand("select Batch_Expired from MedicineStockStatusView2;", globalCon);
            var oot = cmd.ExecuteScalar();
            if (oot != DBNull.Value)
            {
                Value = Convert.ToInt16(cmd.ExecuteScalar());
            }
            return Value;
        }
    }
}

