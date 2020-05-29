using MySql.Data.MySqlClient;
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
            GenerateList();
        }

        void GenerateList()
        {

            MySqlCommand cmd = new MySqlCommand("select * from FullMedicineStatusView", globalCon);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int stocked = reader.GetInt16(1);
                int stockedExpB = reader.GetInt16(2);
                int batchs = reader.GetInt16(3);
                Medicine medicine = BindData(name, stocked, stockedExpB, batchs);
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

        Medicine BindData(string name, int stk, int sktEB, int btc)
        {
            Medicine medicine;
            ThisMedicineInfos thisMedicineInfos;
            thisMedicineInfos = new ThisMedicineInfos(btc, stk, sktEB, 1700);
            medicine = new Medicine(name, tags, thisMedicineInfos);
            return medicine;
        }

        public HashSet<Medicine> GetMedicines() => medicines;

        public HashSet<Medicine> GetFillteredMedicine() => filtterdMedicine;

        public HashSet<Medicine> FiltterMedicine(Filtter stock, Filtter batch, string medicinename)
        {
            string qPrice;
            if (stock.IsActive)
                qPrice = $"stocked between {stock.LowValue} and {stock.HighValue}";
            else qPrice = $"stocked between {0} and {int.MaxValue}";

            string qBatch;
            if (batch.IsActive)
                qBatch = $"Batch between {batch.LowValue} and {batch.HighValue}";
            else qBatch = $"stocked between {0} and {int.MaxValue}";

            string cQuery = $"select * from FullMedicineStatusView where {qPrice} and {qBatch} and medName like'{medicinename}%' ";
            MySqlCommand cmd = new MySqlCommand(cQuery, globalCon);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(0);
                int stocked = reader.GetInt16(1);
                int stockedExpB = reader.GetInt16(2);
                int batchs = reader.GetInt16(3);
                Medicine medicine = BindData(name, stocked, stockedExpB, batchs);
                if (!filtterdMedicine.Contains(medicine)) filtterdMedicine.Add(medicine);
            }
            reader.Close();
            return filtterdMedicine;
        }


    }

    class Medicine
    {
        public string name;
        public Dictionary<string, Color> tags;
        public ThisMedicineInfos medicineInfos;

        public Medicine(string name, Dictionary<string, Color> tags, ThisMedicineInfos medicineInfos)
        {
            this.name = name;
            this.tags = tags;
            this.medicineInfos = medicineInfos;

        }

    }


    /*
     * Purpose of this class
     * get infos about new medicine 
     * as well as their past prices and current price (that is last bacth prices)
     */
    class ThisMedicineInfos
    {
        public int batchs;
        public int currentNumber;
        public int expiredB;
        public int currentPrice;
        
        List<int> pricesList = new List<int>(5);
        public ThisMedicineInfos(int batchs, int currentNumber, int expiredB, int currentPrice /*,List<int> pricesList*/)
        {
            this.batchs = batchs;
            this.currentNumber = currentNumber;
            this.expiredB = expiredB;
            this.currentPrice = currentPrice;
            //this.pricesList = pricesList;
        }


    }

    class Filtter
    {
        public Filtter(int fillterLow, int filterHigh, bool isActive)
        {
          
            if(filterHigh < fillterLow)
            {
                int dummy = fillterLow;
                fillterLow = filterHigh;
                filterHigh = dummy;
            }
            this.LowValue = fillterLow;
            this.HighValue = filterHigh;
            this.IsActive = isActive;
        }

        public bool IsActive { get; }
        public int LowValue { get; }
        public int HighValue { get; }
    }
    

}
