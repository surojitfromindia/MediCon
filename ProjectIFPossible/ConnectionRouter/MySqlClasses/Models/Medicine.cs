using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses.Models
{

    class Medicine : mySqlConnection
    {
        public string name;
        public string registeredBy;
        public Dictionary<string, Color> tags;
        public MedicineGroup medicineOverAllInfo;
        public List<MedicineGroup> perBatchInfoList;
        public MedicineScheduleInfo medicineScheduleInfo;
        public ManufacturerInfo manufacturerInfo;

        public Medicine(string name)
        {
            this.name = name;
        }

        public Medicine(
            string name,
            Dictionary<string, Color> tags,
            MedicineGroup medicineInfos,
            MedicineScheduleInfo medicineScheduleInfo)
        {
            this.name = name;
            this.tags = tags;
            this.medicineOverAllInfo = medicineInfos;
            this.medicineScheduleInfo = medicineScheduleInfo;
        }

        public Medicine(
            string name,
            string registeredBy,
            List<MedicineGroup> medicineLongInfos,
            MedicineScheduleInfo medicineScheduleInfo,
            ManufacturerInfo manufacturerInfo)
        {
            this.name = name;
            this.registeredBy = registeredBy;
            this.perBatchInfoList = medicineLongInfos;
            this.medicineScheduleInfo = medicineScheduleInfo;
            this.manufacturerInfo = manufacturerInfo;

        }


        public Medicine MediShortInfo()
        {
            Medicine medicine = null;
            MySqlCommand cmd = new MySqlCommand("select * from FullMedicineStatusViewAdvance where medname=@mdn ", globalCon);
            cmd.Parameters.AddWithValue("@mdn", name);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int stocked = reader.GetInt16(1);
                int stockedExpB = reader.GetInt16(2);
                int batchs = reader.GetInt16(3);
                int price = reader.GetInt16(4);
                int onSchedule = reader.GetInt16(5);
                medicine = new Medicine(name, tags,
                    new MedicineGroup(batchs, stocked, stockedExpB, price),
                    new MedicineScheduleInfo(name, onSchedule));
            }
            reader.Close();
            return medicine;
        }
        public Medicine MedicineLongInfo()
        {
            string companyName = "";
            string query = "select manfName,username from mednametable where medName=@mname";
            MySqlCommand getDetailsCommand = new MySqlCommand(query, globalCon);
            getDetailsCommand.Parameters.AddWithValue("@mname", name);
            MySqlDataReader reader = getDetailsCommand.ExecuteReader();
            while (reader.Read())
            {
                registeredBy = reader.GetString(1);
                companyName = reader.GetString(0);
            }
            reader.Close();
            //Get List of Manufacturer Infos.
            manufacturerInfo = ManufacturerInfo.GetShortInfoOf(companyName);
            //Get List of Batch Infos.
            perBatchInfoList = MedicineGroup.SeparatedBatchInfo(name);
            //Get Full Schedule Info.
            medicineScheduleInfo = new MedicineScheduleInfo(name).GetScheduleInfo();
            //Return the Medicine.
            return new Medicine(name, registeredBy, perBatchInfoList, medicineScheduleInfo, manufacturerInfo);
        }

    }

    class MedicineGroup : mySqlConnection
    {
        public int batchs;
        public int stocked;
        public int expiredB;
        public int currentPrice;
        public string status;
        public DateTime entryDate;
        public DateTime expDate;

        public MedicineGroup(int batchno, int stocked, int expiredB, int currentPrice)
        {
            this.batchs = batchno;
            this.stocked = stocked;
            this.expiredB = expiredB;
            this.currentPrice = currentPrice;
        }
        public MedicineGroup(int batchNo, int thisNumber, string thisStatus, DateTime thisEntryD, DateTime thisExpD, int thisPrice)
        {
            batchs = batchNo;
            stocked = thisNumber;
            status = thisStatus;
            entryDate = thisEntryD;
            expDate = thisExpD;
            currentPrice = thisPrice;
        }



        public static List<MedicineGroup> SeparatedBatchInfo(string name)
        {
            List<MedicineGroup> medicineLongInfos = new List<MedicineGroup>(10);
            //Get the Full Batch Info.
            MySqlCommand cmd = new MySqlCommand("select * from medicinetable where medname=@mdn ", globalCon);
            cmd.Parameters.AddWithValue("@mdn", name);
            MySqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read())
            {
                DateTime entryDate = reader2.GetDateTime(1);
                DateTime expDate = reader2.GetDateTime(2);
                int BatchNumber = reader2.GetInt16(3);
                int BatchStock = reader2.GetInt16(4);
                string status = reader2.GetString(5);
                int price = reader2.GetInt16(6);
                var tempInfo = new MedicineGroup(BatchNumber, BatchStock, status, entryDate, expDate, price);
                medicineLongInfos.Add(tempInfo);
            }
            reader2.Close();
            return medicineLongInfos;
        }

    }

   enum MEDCINESTATUS
    {
        OK, EXP
    }
}
