using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectIFPossible
{
    public class UserCreadential
    {
        private string pass1;
        private string pass2;

        public UserCreadential(string ids, string password1, string password2)
        {
            this.UID = ids;
            pass1 = password1;
            pass2 = password2;
        }

        public string UID { get; }



        public bool validate()
        {
            if (UID == "Admin" && pass2 == "")
            {
                MessageBox.Show("Enter Second Password");
                return false;
            }
            else if (UID == "" || pass1 == "")
            {
                MessageBox.Show("Field Can't be Empty");
                return false;
            }

            return true;
        }

        public void Login()
        {
            if (UID == "1") throw new WrongPassWordException();
            
        }

       
    }

    public class WrongPassWordException: Exception
    {
        public override string Message
        {
            get
            {
                return "Password is not matched";
            }
        }
    }
}
