using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses.Models
{
    class ManufacturerInfo : mySqlConnection
    {
        public string manufacturName;
        public string address;
        public string contact;
        public string email;
        public string website;


        //For New Company Registration
        public ManufacturerInfo(string manufaname, string address, string contact, string email, string website)
        {
            manufacturName = manufaname;
            this.address = address;
            this.contact = contact;
            this.email = email;
            this.website = website;
        }

        //For Value Value Fetch.
        public ManufacturerInfo(string manufaname)
        {
            manufacturName = manufaname;
        }


        //Method To Find Details by Manufacturer Name
        //which returns a ManufacturerInfo object
        public static ManufacturerInfo GetShortInfoOf(string manufacturName)
        {
            string address = "";
            string contact = "";
            string email = "";
            string web = "";
            string query = "select * from manufacturedetailstable where manfName=@mfname";
            MySqlCommand getDetailsCommand = new MySqlCommand(query, globalCon);
            getDetailsCommand.Parameters.AddWithValue("@mfname", manufacturName);
            MySqlDataReader reader = getDetailsCommand.ExecuteReader();
            while (reader.Read())
            {
                address = reader.GetString(1);
                contact = reader.GetString(2);
                email = reader.GetString(3);
                web = reader.GetString(4);
            }
            reader.Close();
            return new ManufacturerInfo(manufacturName, address, contact, email, web);
        }

        //Method To Register New Company
        public bool Register()
        {
            string query = "insert into  manufacturedetailstable values(@c1, @c2, @c3, @c4, @c5);";
            MySqlCommand insertCommand = new MySqlCommand(query, globalCon);
            insertCommand.Parameters.AddWithValue("@c1", manufacturName);
            insertCommand.Parameters.AddWithValue("@c2", address);
            insertCommand.Parameters.AddWithValue("@c3", contact);
            insertCommand.Parameters.AddWithValue("@c4", email);
            insertCommand.Parameters.AddWithValue("@c5", website);
            int i = insertCommand.ExecuteNonQuery();
            return i > 0 ? true : false;
        }

        public bool RecordContactsAndEmails(List<string> contacts, List<string> emalis)
        {
            int i = 0;
            int i2 = 0;

            //Insert All Contact Fields
            foreach (string contact in contacts)
            {
                string query = $"insert into onlycontacttable values('{manufacturName}', '{contact}')";
                MySqlCommand insertContactCommand = new MySqlCommand(query, globalCon);
                i = insertContactCommand.ExecuteNonQuery();
            }
            //Insert All Email Fields
            foreach (string email in emalis)
            {
                string query2 = $"insert into  onlyemailtable values('{manufacturName}', '{email}')";
                MySqlCommand insertEmailCommand = new MySqlCommand(query2, globalCon);
                i2 = insertEmailCommand.ExecuteNonQuery();
            }
            return (i > 0 && i2 > 0) ? true : false;
        }


        public static ManufacturerInfo GetInfoByName(string manufacturer)
        {
            string query = "select * from  manufacturedetailstable;";
            MySqlCommand fetchCommand = new MySqlCommand(query, globalCon);
            int count = 0;
            string add = "", cont = "", email = "", website = "";
            MySqlDataReader reader = fetchCommand.ExecuteReader();
            while (reader.Read())
            {
                add = reader.GetString(1);
                cont = reader.GetString(2);
                email = reader.GetString(3);
                website = reader.GetString(4);
                count += 1;
            }
            reader.Close();
            if (count == 1)
                return new ManufacturerInfo(manufacturer, add, cont, email, website);
            return new ManufacturerInfo(manufacturer, add, cont, email, website);
        }


        public static List<string> GetAllManufacturerName()
        {
            List<string> names = new List<string>(40);
            string query = "select manfName from  manufacturedetailstable;";
            MySqlCommand fetchCommand = new MySqlCommand(query, globalCon);

            MySqlDataReader reader = fetchCommand.ExecuteReader();
            while (reader.Read())
            {
                names.Add(reader.GetString(0));
            }
            reader.Close();
            return names;
        }
        public static List<string> GetAllContactOf(string manfacturer)
        {
            List<string> contacts = new List<string>(40);
            string fetch = $"select contact from  onlycontacttable where manufac = '{manfacturer}';";
            MySqlCommand fetchCommand = new MySqlCommand(fetch, globalCon);
            MySqlDataReader reader = fetchCommand.ExecuteReader();
            while (reader.Read())
            {
                contacts.Add(reader.GetString(0));
            }
            reader.Close();
            return contacts;
        }
        public static List<string> GetAllEmailOf(string manfacturer)
        {
            List<string> emails = new List<string>(40);
            string fetch = $"select email from onlyemailtable where manufac = '{manfacturer}';";
            MySqlCommand fetchCommand = new MySqlCommand(fetch, globalCon);
            MySqlDataReader reader = fetchCommand.ExecuteReader();
            while (reader.Read())
            {
                emails.Add(reader.GetString(0));
            }
            reader.Close();
            return emails;
        }
        public static List<string> GetMedicineNameOf(string manfacturer)
        {
            List<string> names = new List<string>(20);

            string query = $"select medname from mednametable where manfName ='{manfacturer}'";
            MySqlCommand fetch = new MySqlCommand(query, globalCon);
            MySqlDataReader reader = fetch.ExecuteReader();
            while (reader.Read())
            {
                names.Add(reader.GetString(0));
            }
            reader.Close();

            return names;
        }
    }
}
