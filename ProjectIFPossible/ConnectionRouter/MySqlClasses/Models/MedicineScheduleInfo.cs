using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses.Models
{
    class MedicineScheduleInfo : mySqlConnection
    {
        public string medName { get; }
        public int noOfSch { get; }
        public DateTime scheduleCreateDate { get; }


        public MedicineScheduleInfo(string medName)
        {
            this.medName = medName;
        }


        public MedicineScheduleInfo(string medName, int noOfSch)
        {
            this.medName = medName;
            this.noOfSch = noOfSch;
        }
        public MedicineScheduleInfo(string medName, int noOfSch, DateTime creationDate)
        {
            this.medName = medName;
            this.noOfSch = noOfSch;
            scheduleCreateDate = creationDate;
        }

        public bool MakeSchedule(int Amount)
        {
            //use the stored Procedure
            string procedureName = "MakeASchedule";
            MySqlCommand cmd = new MySqlCommand(procedureName, globalCon)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("Medicine_Name", medName);
            cmd.Parameters.AddWithValue("Medicine_Quan", Amount);
            int i = cmd.ExecuteNonQuery();
            if (i == 1)
                return true;
            return false;
        }

        public MedicineScheduleInfo GetScheduleInfo()
        {
            MedicineScheduleInfo md = new MedicineScheduleInfo("", 0);
            string query = "select * from scheduletable where medicinename = @medname";
            MySqlCommand cmd = new MySqlCommand(query, globalCon);
            cmd.Parameters.AddWithValue("@medname", medName);
            MySqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow);
            while (reader.Read())
            {
                string medName = reader.GetString(0);
                DateTime scDate = reader.GetDateTime(1);
                int nofS = reader.GetInt16(2);
                md = new MedicineScheduleInfo(medName, nofS, scDate);
            }
            reader.Close();
            return md;
        }

        public bool Remove()
        {
            string procedureName = "RemoveSchedule";
            MySqlCommand cmd = new MySqlCommand(procedureName, globalCon)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("MedName", medName);
            int i = cmd.ExecuteNonQuery();
            if (i == 1)
                return true;
            return false;
        }
    }
}
