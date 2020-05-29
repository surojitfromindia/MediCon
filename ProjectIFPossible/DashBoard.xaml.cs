using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ProjectIFPossible.ConnectionRouter;
using ProjectIFPossible.ConnectionRouter.MySqlClasses;
using MySql.Data.MySqlClient;
using ProjectIFPossible.MiniWindows;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Window
    {
        MySqlConnection ct;
        public DashBoard()
        {
            ct = mySqlConnection.globalCon;
            InitializeComponent();
            DataContext = this;
            RefreshCounters();
            navBtnBar.Width = 0; //this will collaspe navbar at first run;
        }


        

        private void OneCard_UserControlClicked_1(object sender, RoutedEventArgs e)
        {
            if (ct.Ping())
            {
                MedicinEntryWindow MDW = new MedicinEntryWindow();
                MDW.onCloserUpdate = RefreshCounters; //a delegate method from MedicinEntryWindow
                MDW.Owner = this;
                MDW.Show();
            }

        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (navBtnBar.Width != 0) //if navbar is not collasped then collasped it
                navBtnBar.Width = 0;
            else
                navBtnBar.Width = double.NaN; //if already collapsed then expand it (auto)
        }

        

        private void btnOpenSetting_Click(object sender, RoutedEventArgs e)
        {
            //show setting dialoge window
            settingWindow st = new settingWindow();
            st.Owner = this;
            st.ShowDialog();
        }


        private void RefreshAllField(object ed, RoutedEventArgs egs)
        {
            //Refresh all the counter
            RefreshCounters();
        }


        private void RefreshCounters()
        {
            if (ct.Ping())
            //Refresh all the counter
            {
                txtLogedInUser.Text = mySqlConnection.globalUser;
                Sicon.Fill = new SolidColorBrush(Colors.RoyalBlue);
                onlineStatus.Text = "Online";
                UCMedi.text = MySqlMedicineEntry.MediCineCount().ToString();
                UCStock.text = MySqlUtil.MediCineCount().ToString();
              
            }
            else
            {
                MessageBox.Show("Not Connected!");
            }
        }

        private void CardsControl_detailBtnHadler(object sender, RoutedEventArgs e)
        {
            //show new Medicine Batch entry dialoge
            MedicineBatchEntry nMBE = new MedicineBatchEntry();
            nMBE.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            onlineStatus.Text = "Offline";
            txtLogedInUser.Text = "-----";
            Sicon.Fill = new SolidColorBrush(Colors.Red);
            mySqlConnection.DisConnect();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mySqlConnection mm = new mySqlConnection("soumya", "1234@kol");
            mm.Connect();
            ct = mySqlConnection.globalCon;

        }

        private void UCList_detailBtnHadler(object sender, RoutedEventArgs e)
        {
            MedicineListWindow mdlw = new MedicineListWindow();
            mdlw.Show();
        }
    }
}
