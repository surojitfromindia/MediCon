using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using ProjectIFPossible.ConnectionRouter.MySqlClasses;

/**
 * Author: Surojit Paul
 * 2019-2020
 */

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses
{
    class MySqlMedicineBatchEntry : mySqlConnection
    {
        private readonly string MedicineName;
        private readonly int NumbOfMed;
        private readonly int Price;
        public MySqlMedicineBatchEntry(string M, int N, int p)
        {
            MedicineName = M;
            NumbOfMed = N;
            Price = p;
        }


        /* this can throw NoRecordException*/
        public bool SaveAndCommit()
        {
            if (ValidateMedicineNameBeforeSaveing() && NumbOfMed > 0 && Price > 0)
            {
                string procedureName = "medicineBatchInsert";
                MySqlCommand cmd = new MySqlCommand(procedureName, globalCon)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("med_name", MedicineName);
                cmd.Parameters.AddWithValue("no_Of_Item_To_Add", NumbOfMed);
                cmd.Parameters.AddWithValue("med_price", Price);
                int i = cmd.ExecuteNonQuery();
                if (i == 1)
                    return true;
            }
            else
            {
                throw new NonRegisterNameException();
            }
            return false;
        }



        private bool ValidateMedicineNameBeforeSaveing()
        {
            bool Tr; //mean false
            MySqlCommand cmd = new MySqlCommand("select ValidDateMedicineName( @mediCineName )", globalCon);
            cmd.Parameters.AddWithValue("@mediCineName", MedicineName);
            Tr = (bool)cmd.ExecuteScalar();
            Console.WriteLine(Tr);
            return Tr;
        }


        public override int GetHashCode()
        {
            int hash = 0;
            for(int i =0; i<MedicineName.Length;i++)
            {
                hash += MedicineName[i] * (i + 1);
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
           if(obj!=null)
            {
                MySqlMedicineBatchEntry md = (MySqlMedicineBatchEntry)obj;
                if (MedicineName == md.MedicineName)
                    return true;
            }
           return false;
        }

    }
    

   
}

