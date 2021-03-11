using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses.Models
{

    class EmptyStockMedicine : mySqlConnection
    {
        public EmptyStockMedicine()
        {
            FillUp();
            
        }

        private List<Medicine> listOfEmptyStockMedicine = new List<Medicine>(20);
        private List<string> mednames = new List<string>(20);

        private void FillUp()
        {
            string query = "select medName from inventorypriceview where tprice = 0;";
            MySqlCommand cmd = new MySqlCommand(query, globalCon);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string medicinename = reader.GetString(0);
                mednames.Add(medicinename);

            }
            reader.Close();

            foreach(string e in mednames)
            {
                Medicine md = new Medicine(e).MedicineLongInfo();
                listOfEmptyStockMedicine.Add(md);
            }
        }

        public List<Medicine> getEmptyStockedMedicineNames => listOfEmptyStockMedicine;

       
            
            
        


    }
}
