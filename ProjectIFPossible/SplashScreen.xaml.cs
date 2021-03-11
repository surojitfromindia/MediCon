using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        LoginWindow lgw;
        FirstTimeLaunch ftLw;
        string ON_ENTRY_UID ;
        string ON_ENTRY_PASSWORD;
        string SERVER ;
        DispatcherTimer dt;
        public SplashScreen()
        {
            InitializeComponent();
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(1);
            dt.Tick += timer_Tick;
            dt.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
          
            pbarFake.Value += 1;
            if (pbarFake.Value > 99) {
            L1: try
                {
                    UANDP();
                    Console.WriteLine(ON_ENTRY_UID + ON_ENTRY_PASSWORD );
                    string cons = $"uid = {ON_ENTRY_UID }; password = {ON_ENTRY_PASSWORD}; server ={SERVER}; database ='MyDb'; ";
                    string cons1 = "uid=surojit; password=surojit; server=192.168.0.5; database=MyDb";
                    MySqlConnection k = new MySqlConnection(cons);
                    k.Open();
                    var l = k.Ping();
                    if (l)
                    {
                        dt.Stop();
                        lgw = new LoginWindow();
                        lgw.Show();
                        Close();
                    }
                }
                catch (MySqlException)
                {
                    ftLw = new FirstTimeLaunch();                  
                    ftLw.ShowDialog();
                    goto L1;
                }
            }

        }

        
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
            ON_ENTRY_UID = u;
            ON_ENTRY_PASSWORD = pas;
            SERVER = po;
        }
    }
}
