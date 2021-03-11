using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using ProjectIFPossible.ConnectionRouter;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for FirstTimeLaunch.xaml
    /// </summary>
    public partial class FirstTimeLaunch : Window
    {
        string  userName;
        string pass;
        string port;
        const string correct = "\x2714";
        const string incorrect = "\x2716";
        const string saved = "\x2b55";
        const string connectionOk = "Connection Establised";
        const string connectionError = "Connection Faild, review connection infos";
        const string dbFound = "Database found.";
        const string dbNotFound = "Database Not found!";
        
        
        
        public FirstTimeLaunch()
        {
            UANDP();
            InitializeComponent();
        }

        private void btnBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            onCloseBtnClikc();
        }

        private void btnConnectionTest_Click(object sender, RoutedEventArgs e)
        {
            userName = txtUserName.Text;
            pass = txtPass.Password;
            port = txtPort.Text;
            ChckDbandConnection();
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            ConnectionAndDatabaseValidator valic = new ConnectionAndDatabaseValidator(userName, pass, port);
            if (valic.WriteUserMasterLoginInfo())
            {
                lblvalid.Text = "saved";
                lblvalid.Foreground = new SolidColorBrush(Colors.Yellow);
            }

        }

        private void btnCreateDB_Click(object sender, RoutedEventArgs e)
        {
            
            ConnectionAndDatabaseValidator valic = new ConnectionAndDatabaseValidator(userName, pass, port);
            Thread k = new Thread(valic.CreateDatabaseIfNotExsist);
            k.Start(); //work on a new Thread
            while (k.ThreadState == ThreadState.Running)
            {
                lbldbfound.Text = "Creating Database, Please Wait";
            }
            Console.WriteLine("Done");
            lblvalid2.Text = correct;
            lbldbfound.Text = "Database created, Press back";
            btnCreateDB.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChckDbandConnection();
        }

       


        private bool ChckDbandConnection()
        {
            
            ConnectionAndDatabaseValidator valic = new ConnectionAndDatabaseValidator(userName, pass, port);
            bool cV = valic.CheckConnection();
            bool cD = valic.DBConnection();
            //if connection is valid
            if (cV)
            {
                lblvalid.Text = correct;
                lblCreditCorrect.Text = connectionOk;
                lblvalid.Foreground = new SolidColorBrush(Colors.Yellow);

            }
            else
            {
                lblvalid.Text = incorrect;
                lblCreditCorrect.Text = connectionError;
                lblvalid.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            //if db is valid
            if (cD)
            {
                lblvalid2.Text = correct;
                lbldbfound.Text = dbFound;
                lblvalid2.Foreground = new SolidColorBrush(Colors.Yellow);
                btnCreateDB.Visibility = Visibility.Hidden;
            }
            else
            {
                lblvalid2.Text = incorrect;
                lbldbfound.Text = dbNotFound;
                lblvalid2.Foreground = new SolidColorBrush(Colors.Yellow);
                btnCreateDB.Visibility = Visibility.Visible;
            }

            return (cV && cD);
        }

        //Behave differently
        //if correct Userinfo and Database found. it will close this from
        //and the main splash will again try to load everythig again.
        //else
        //It will Close the whole App (Exit);
        void onCloseBtnClikc()
        {
            //Call UANDP() to read current saved UserInfo
            UANDP();
            if (ChckDbandConnection())
                Close();
            else
                Environment.Exit(0);
        }

        //Currently saved user infos.
        private void UANDP()
        {
            string u = "";
            string pas = "";
            string po = "";
            var col = new List<string>(2);
            using (var rd = new StreamReader("settings\\xco.csv"))
            {
                while (!rd.EndOfStream)
                {
                    var lines = rd.ReadLine().Split(':');
                    col.Add(lines[1]);
                }
                u = col[0];
                pas = col[1];
                po = col[2];
            }
            userName = u;
            pass = pas;
            port = po;
        }

    } 
}
