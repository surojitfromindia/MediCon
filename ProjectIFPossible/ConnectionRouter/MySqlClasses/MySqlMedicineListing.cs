using MySql.Data.MySqlClient;
using ProjectIFPossible.ConnectionRouter.MySqlClasses.Models;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses
{
    class MySqlMedicineListing : mySqlConnection
    {
        private HashSet<Medicine> medicines = new HashSet<Medicine>(10);
        private HashSet<Medicine> filtterdMedicine = new HashSet<Medicine>(10);// set of medicines and their infos
        Color[] tagColors =
            {
            Color.FromRgb(120,98,160),
            Color.FromRgb(110, 98, 111),
            Color.FromRgb(185, 81, 120),
        };
        //mockup tag list
        Dictionary<string, Color> tags = new Dictionary<string, Color>(50);



        public MySqlMedicineListing()
        {
            GenerateTagMap();
            GenerateShortDetailsList();
        }

        //Only Short Details
        void GenerateShortDetailsList()
        {

            MySqlCommand cmd = new MySqlCommand("select * from FullMedicineStatusViewAdvance", globalCon);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int stocked = reader.GetInt16(1);
                int stockedExpB = reader.GetInt16(2);
                int batchs = reader.GetInt16(3);
                int price = reader.GetInt16(4);
                int onSchedule = reader.GetInt16(5);
                Medicine medicine = BindData(name, stocked, stockedExpB, batchs, price, onSchedule);
                if (!medicines.Contains(medicine)) medicines.Add(medicine);
            }
            reader.Close();

        }

        //will list all medicine tags from database
        void GenerateTagMap()
        {
            tags.Add("Cold", tagColors[0]);
            tags.Add("Headache", tagColors[1]);
            tags.Add("Flu", tagColors[2]);
        }



        public HashSet<Medicine> GetMedicines() => medicines;

        public HashSet<Medicine> GetFillteredMedicine() => filtterdMedicine;

        public HashSet<Medicine> FiltterMedicine(Fillter stock, Fillter batch, Fillter Price, string medicinename)
        {
            string qStocked;
            if (stock.IsActive)
                qStocked = $"stocked between {stock.LowValue} and {stock.HighValue}";
            else qStocked = $"stocked between {0} and {int.MaxValue}";

            string qBatch;
            if (batch.IsActive)
                qBatch = $"Batch between {batch.LowValue} and {batch.HighValue}";
            else qBatch = $"Batch between {0} and {int.MaxValue}";

            string qPrice;
            if (Price.IsActive)
                qPrice = $"price between {Price.LowValue} and {Price.HighValue}";
            else qPrice = $"price between {0} and  {int.MaxValue}";

            string cQuery = $"select * from FullMedicineStatusViewAdvance " +
                $"where {qStocked} and {qBatch} and {qPrice} and medName like'{medicinename}%' ";
            MySqlCommand cmd = new MySqlCommand(cQuery, globalCon);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int stocked = reader.GetInt16(1);
                int stockedExpB = reader.GetInt16(2);
                int batchs = reader.GetInt16(3);
                int medPrice = reader.GetInt16(4);
                int onSchedule = reader.GetInt16(5);
                Medicine medicine = BindData(name, stocked, stockedExpB, batchs, medPrice, onSchedule);
                if (!filtterdMedicine.Contains(medicine)) filtterdMedicine.Add(medicine);
            }
            reader.Close();
            return filtterdMedicine;
        }

        Medicine BindData(string name, int stk, int sktEB, int btc, int pri, int noOfSc)
        {
            Medicine medicine;
            MedicineGroup thisMedicineInfos;
            MedicineScheduleInfo medicineScheduleInfo;
            thisMedicineInfos = new MedicineGroup(btc, stk, sktEB, pri);
            medicineScheduleInfo = new MedicineScheduleInfo(name, noOfSc);
            medicine = new Medicine(name, tags, thisMedicineInfos, medicineScheduleInfo);
            return medicine;
        }


    }

}
