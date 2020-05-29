using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses
{
    class MedicineDetails : mySqlConnection
    {
        private string medicineName = "";
        private string manufactureName="";
        private int currntonHold = 0;
        private int currentBactc = 0;
        private int ValidPeriod = 0;

        private readonly string NO_OF_CURRENT_BATCH_QUERY =
            "select max(batchNumber) from medicinetable where medname = @med;";
        private readonly string NO_OF_MEDICINE_ON_SAME_NAME_AND_STATUS_VALID =
            "select sum(numOf) from medicinetable where medName = @med and status = 'OK';";
        private readonly string EXP_MONTH_DUIRATION_QUERY = 
            "select expiryDuration from mednametable where medName =@med";
        private readonly string NAME_OF_MANUFAC =
           "select manfName from mednametable where medName =@med";



        public MedicineDetails(string medi)
        {
            medicineName = medi;
        }

        public int MED_CUR_BATCH()
        {
            MySqlCommand cmd = new MySqlCommand(NO_OF_CURRENT_BATCH_QUERY, globalCon);
            cmd.Parameters.AddWithValue("@med", medicineName);
            var obj = cmd.ExecuteScalar();
            if (obj != DBNull.Value)
            {
                int j = Convert.ToInt16(obj);
                currentBactc = j + 1;
            }
            return currentBactc;
        }
        public int MED_CUR_HOLD()
        {
            MySqlCommand cmd = new MySqlCommand(NO_OF_MEDICINE_ON_SAME_NAME_AND_STATUS_VALID, globalCon);
            cmd.Parameters.AddWithValue("@med", medicineName);
            var obj = cmd.ExecuteScalar();
            if (obj != DBNull.Value)
            {
                int j = Convert.ToInt16(obj);
                currntonHold = j;
            }
            return currntonHold;
        }

        public int MED_EXP_MONTH_DURATION()
        {
            MySqlCommand cmd = new MySqlCommand(EXP_MONTH_DUIRATION_QUERY, globalCon);
            cmd.Parameters.AddWithValue("@med", medicineName);
            var obj = cmd.ExecuteScalar();
            if (obj != DBNull.Value)
            {
                int j = Convert.ToInt16(obj);
                ValidPeriod = j;
            }
            return ValidPeriod;
        }

        public string MED_MANUF_NAME()
        {
            MySqlCommand cmd = new MySqlCommand(NAME_OF_MANUFAC, globalCon);
            cmd.Parameters.AddWithValue("@med", medicineName);
            var obj = cmd.ExecuteScalar();
            if (obj != null)
            {
                string j = obj.ToString();
                manufactureName = j;
            }
            return manufactureName;
        }


    }
}
