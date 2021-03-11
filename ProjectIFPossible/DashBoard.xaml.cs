using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ProjectIFPossible.ConnectionRouter;
using ProjectIFPossible.ConnectionRouter.MySqlClasses;
using MySql.Data.MySqlClient;
using ProjectIFPossible.MiniWindows;
using System.Windows.Shapes;
using ProjectIFPossible.ConnectionRouter.MySqlClasses.Models;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Window
    {

        //List All Windows here
        MedicinEntryWindow MRW = null;
        MedicineBatchEntry MBEW = null;
        EmptyMedicineLookUpWindow EMLU = null;
        MedicineDetailsInfoWindow MDEVWSEARCH = null;
        MedicineListWindow MDLW = null;
        ManfuacturerRegistraionWindow MFRW = null;
        ManufacturerDetailsSearchWindow MFDSW = null;
        Window[] childs;
        MySqlConnection ct;

        //
        Theme currntTheme;
        string themename = "green";
        public DashBoard()
        {
            ct = mySqlConnection.globalCon;
            childs = new Window[] { MRW, MBEW, EMLU, MDEVWSEARCH, MDLW, MFRW };
            InitializeComponent();
            Closing += DashBoard_Closing;
            Closed += DashBoard_Closed;
            DataContext = this;

            RefreshCounters();
            HideShowNavBar();
            SetTheme(Themes.Green);

        }

        private void DashBoard_Closed(object sender, EventArgs e)
        {
            mySqlConnection.DisConnect();
        }

        private void DashBoard_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Prompt you to close all Window Before closing the DashBoard.
            if (MDEVWSEARCH != null || MBEW != null || EMLU != null || MRW != null || MDLW != null || MFRW != null)
            {
                MessageBox.Show("Close All Form!");
                e.Cancel = true;
            }
        }










        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HideShowNavBar();

        }

        private void HideShowNavBar()
        {
            if (navBtnBar.Visibility == Visibility.Collapsed) //if navbar is not collasped then collasped it
                navBtnBar.Visibility = Visibility.Visible;
            else
                navBtnBar.Visibility = Visibility.Collapsed; ; //if already collapsed then expand it (auto)

            if (navBtnBar.Visibility == Visibility.Collapsed)
                ColorPanelCollapse(true);
            else
                ColorPanelCollapse(false);
        }

        private void ColorPanelCollapse(bool isCollapsed)
        {
            if (isCollapsed)
            {
                foreach (Ellipse e in colorPanel.Children)
                {
                    e.Visibility = Visibility.Collapsed;
                }

                switch (themename)
                {
                    case "purple": btnCPu.Visibility = Visibility.Visible; break;
                    case "green": btnCGr.Visibility = Visibility.Visible; break;
                    case "red": btnCRe.Visibility = Visibility.Visible; break;
                    case "dark": btnCDr.Visibility = Visibility.Visible; break;
                    case "night": btnCNi.Visibility = Visibility.Visible; break;
                    case "blue": btnCBlu.Visibility = Visibility.Visible; break;
                }

            }
            else
            {
                foreach (Ellipse e in colorPanel.Children)
                {
                    e.Visibility = Visibility.Visible;
                }
            }
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
                Sicon.Fill = new SolidColorBrush(Color.FromRgb(20, 255, 30));
                onlineStatus.Text = "Online";
                UCMedi.text = MySqlMedicineEntry.MediCineCount().ToString();
                UCStock.text = MySqlUtil.MediCineCount().ToString();
                UCCompany.text = ManufacturerInfo.GetAllManufacturerName().Count.ToString();

            }
            else
            {
                MessageBox.Show("Not Connected!");
            }
        }



        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            onlineStatus.Text = "Offline";
            txtLogedInUser.Text = "-----";
            Sicon.Fill = new SolidColorBrush(Colors.Red);
            mySqlConnection.DisConnect();
        }





        private void btnLoginAgain_Click(object sender, RoutedEventArgs e)
        {
            mySqlConnection mm = new mySqlConnection(txtUserfromSideBar.Text, txtPassfromSideBar.Password);
            mm.Connect();
            ct = mySqlConnection.globalCon;
            if (ct.Ping())
            {
                txtPassfromSideBar.Clear();
                txtUserfromSideBar.Clear();
            }
            RefreshCounters();
        }

        private void UCCompare_detailBtnHadler(object sender, RoutedEventArgs e)
        {
            if (ct.Ping())
            {
                MedicineDetailsInfoCompare mdlw = new MedicineDetailsInfoCompare();
                mdlw.ShowDialog();
            }
            else
                MessageBox.Show("Not Connected!");
        }
























        //Medicine Stock Update Group --- Card 1
        private void UCMedicineUpdateLeftClick(object sender, RoutedEventArgs e)
        {
            //show new Medicine Batch entry dialoge
            if (ct.Ping())
            {
                if (MBEW == null)
                {
                    MBEW = new MedicineBatchEntry();
                    MBEW.ControlTheme = currntTheme;
                    MBEW.Closing += MBEW_Closing;
                    OpendWindowElement opp3 = new OpendWindowElement("Stock/Batch Entry Window");
                    opp3.ControlTheme = currntTheme;
                    opp3.closebtnHandaler += Opp3_closebtnHandaler; ;
                    opp3.showbtnHandaler += Opp3_showbtnHandaler; ;
                    RegisterName("MBEW", opp3);
                    OpenWindowNameHolder.Children.Add(opp3);
                }
                MBEW.Show();
                MBEW.WindowState = WindowState.Normal;
                MBEW.Focus();
            }
            else
                MessageBox.Show("Not Connected!");
        }
        private void Opp3_showbtnHandaler(object sender, RoutedEventArgs e)
        {
            onShowClick(MBEW);

        }
        private void Opp3_closebtnHandaler(object sender, RoutedEventArgs e)
        {
            MBEW.Close();
        }
        private void MBEW_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MBEW != null)
            {
                CloseBatchStockUpdateForm();

            }
            MBEW = null;
        }
        void CloseBatchStockUpdateForm()
        {
            OpenWindowNameHolder.Children
                  .Remove(OpenWindowNameHolder
                  .FindName("MBEW") as OpendWindowElement);
            UnregisterName("MBEW");
        }

        //ListView Stock Update Group --- Card 1
        private void UCMedicineUpdateRightClick(object sender, RoutedEventArgs e)
        {
            if (ct.Ping())
            {
                if (MDLW == null)
                {
                    MDLW = new MedicineListWindow();
                    MDLW.Closing += MDLW_Closing;
                    MDLW.ControlTheme = currntTheme;
                    OpendWindowElement opp4 = new OpendWindowElement("Medicine List Window");
                    opp4.ControlTheme = currntTheme;
                    opp4.closebtnHandaler += Opp4_closebtnHandaler; ;
                    opp4.showbtnHandaler += Opp4_showbtnHandaler; ;
                    RegisterName("MDLW", opp4);
                    OpenWindowNameHolder.Children.Add(opp4);
                }
                MDLW.Show();
                MDLW.WindowState = WindowState.Normal;
                MDLW.Focus();
            }
            else
                MessageBox.Show("Not Connected!");
        }
        private void MDLW_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MDLW != null)
            {
                CloseListForm();

            }
            MDLW = null;
        }
        private void Opp4_showbtnHandaler(object sender, RoutedEventArgs e)
        {
            onShowClick(MDLW);
        }
        private void Opp4_closebtnHandaler(object sender, RoutedEventArgs e)
        {
            MDLW.Close();
        }
        void CloseListForm()
        {
            OpenWindowNameHolder.Children
                  .Remove(OpenWindowNameHolder
                  .FindName("MDLW") as OpendWindowElement);
            UnregisterName("MDLW");
        }


        //Medicine Register Group --- Card 2
        private void UCMedicineRegisterLeftClick(object sender, RoutedEventArgs e)
        {
            if (ct.Ping())
            {
                if (MRW == null)
                {
                    MRW = new MedicinEntryWindow();
                    MRW.onCloserUpdate = RefreshCounters; //a delegate method from MedicinEntryWindow
                    MRW.Closing += MRW_Closing; ;
                    MRW.ControlTheme = currntTheme;
                    OpendWindowElement opp2 = new OpendWindowElement("Medicine Register Window");
                    opp2.ControlTheme = currntTheme;
                    opp2.closebtnHandaler += Opp2_closebtnHandaler;
                    opp2.showbtnHandaler += Opp2_showbtnHandaler;
                    RegisterName("MRW", opp2);
                    OpenWindowNameHolder.Children.Add(opp2);
                }
                MRW.Show();
                MRW.WindowState = WindowState.Normal;
                MRW.Focus();
            }
            else
                MessageBox.Show("Not Connected!");

        }
        private void Opp2_showbtnHandaler(object sender, RoutedEventArgs e)
        {
            onShowClick(MRW);
        }
        private void Opp2_closebtnHandaler(object sender, RoutedEventArgs e)
        {
            MRW.Close();
        }
        private void MRW_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MRW != null)
            {
                CloseMedicineRegForm();

            }
            MRW = null;
        }
        void CloseMedicineRegForm()
        {
            OpenWindowNameHolder.Children
                   .Remove(OpenWindowNameHolder
                   .FindName("MRW") as OpendWindowElement);
            UnregisterName("MRW");
        }

        //Manufacturer Register Group --- Card 3
        private void UCCompanyLeftButtonClick(object sender, RoutedEventArgs e)
        {
            if (ct.Ping())
            {
                if (MFRW == null)
                {
                    MFRW = new ManfuacturerRegistraionWindow();
                    MFRW.ControlTheme = currntTheme;
                    MFRW.Closing += MFRW_Closing; ;
                    OpendWindowElement opp6 = new OpendWindowElement("Company Register Window");
                    opp6.ControlTheme = currntTheme;
                    opp6.closebtnHandaler += Opp6_closebtnHandaler; ; ;
                    opp6.showbtnHandaler += Opp6_showbtnHandaler; ; ;
                    RegisterName("MFRW", opp6);
                    OpenWindowNameHolder.Children.Add(opp6);
                }
                MFRW.Show();
                MFRW.WindowState = WindowState.Normal;
                MFRW.Focus();
            }

        }
        private void Opp6_showbtnHandaler(object sender, RoutedEventArgs e)
        {
            onShowClick(MFRW);
        }
        private void Opp6_closebtnHandaler(object sender, RoutedEventArgs e)
        {
            MFRW.Close();
        }
        private void MFRW_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MFRW != null)
            {
                CloseCompanyRegForm();

            }
            MFRW = null;
        }
        void CloseCompanyRegForm()
        {
            OpenWindowNameHolder.Children
                  .Remove(OpenWindowNameHolder
                  .FindName("MFRW") as OpendWindowElement);
            UnregisterName("MFRW");
        }
        //Manufacturer Register Group --- Card 3
        private void UCCompany_refreshBtnHadler(object sender, RoutedEventArgs e)
        {
            if(ct.Ping())
            {
                if(MFDSW==null)
                {
                    MFDSW = new ManufacturerDetailsSearchWindow();
                    MFDSW.ControlTheme = currntTheme;
                    MFDSW.Closing += MFDW_Closing;
                    OpendWindowElement opp7 = new OpendWindowElement("Search Medicine Window");
                    opp7.ControlTheme = currntTheme;
                    opp7.closebtnHandaler += Opp7_closebtnHandaler; ;
                    opp7.showbtnHandaler += Opp7_showbtnHandaler; ;
                    RegisterName("MFDW", opp7);
                    OpenWindowNameHolder.Children.Add(opp7);

                }
                MFDSW.Show();
                MFDSW.WindowState = WindowState.Normal;
                MFDSW.Focus();
            }
        }

        private void Opp7_showbtnHandaler(object sender, RoutedEventArgs e)
        {
            onShowClick(MFDSW);
        }
        private void Opp7_closebtnHandaler(object sender, RoutedEventArgs e)
        {
            MFDSW.Close();
        }
        private void MFDW_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MFDSW != null)
            {
                CloseManfactDetailsForm();
            }
            MFDSW = null;
        }
        private void CloseManfactDetailsForm()
        {
            OpenWindowNameHolder.Children
                   .Remove(OpenWindowNameHolder
                   .FindName("MFDW") as OpendWindowElement);
            UnregisterName("MFDW");
        }




        //Search Form Group -- Card 6
        private void UCExpandedMedicineDetailsLeftClick(object sender, RoutedEventArgs e)
        {
            if (ct.Ping())
            {
                if (MDEVWSEARCH == null)
                {
                    //Create One Time
                    MDEVWSEARCH = new MedicineDetailsInfoWindow();
                    MDEVWSEARCH.Closing += MDEVWSEARCH_Closing;
                    MDEVWSEARCH.ControlTheme = currntTheme;
                    OpendWindowElement opp = new OpendWindowElement("Search Medicine Window");
                    opp.ControlTheme = currntTheme;
                    opp.closebtnHandaler += Opp_closebtnHandaler;
                    opp.showbtnHandaler += Opp_showbtnHandaler;
                    RegisterName("MDEVWSEARCH", opp);
                    OpenWindowNameHolder.Children.Add(opp);
                }
                MDEVWSEARCH.Show();
                MDEVWSEARCH.WindowState = WindowState.Normal;
                MDEVWSEARCH.Focus();

            }
            else
                MessageBox.Show("Not Connected!");
        }
        private void Opp_showbtnHandaler(object sender, RoutedEventArgs e)
        {
            onShowClick(MDEVWSEARCH);
        }
        private void Opp_closebtnHandaler(object sender, RoutedEventArgs e)
        {
            if (MDEVWSEARCH != null)
            {
                MDEVWSEARCH.Close();
            }
        }
        private void MDEVWSEARCH_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MDEVWSEARCH != null)
            {
                CLoseSearchForm();
            }
            MDEVWSEARCH = null;
        }
        void CLoseSearchForm()
        {
            OpenWindowNameHolder.Children
                   .Remove(OpenWindowNameHolder
                   .FindName("MDEVWSEARCH") as OpendWindowElement);
            UnregisterName("MDEVWSEARCH");
        }


        //Zero/Empty Stock Medicine Group --- Card 7
        private void UCEmptyStockMedicine_detailBtnHadler(object sender, RoutedEventArgs e)
        {
            if (ct.Ping())
            {
                if (EMLU == null)
                {
                    EMLU = new EmptyMedicineLookUpWindow();
                    EMLU.Closing += EMLU_Closing; ;
                    OpendWindowElement opp5 = new OpendWindowElement("Empty Stock Entry Window");
                    opp5.ControlTheme = currntTheme;
                    opp5.closebtnHandaler += Opp5_closebtnHandaler; ; ;
                    opp5.showbtnHandaler += Opp5_showbtnHandaler; ; ;
                    RegisterName("EMLU", opp5);
                    OpenWindowNameHolder.Children.Add(opp5);
                }
                EMLU.Show();
                EMLU.WindowState = WindowState.Normal;
                EMLU.Focus();
            }
            else
                MessageBox.Show("Not Connected");
        }
        private void Opp5_showbtnHandaler(object sender, RoutedEventArgs e)
        {
            onShowClick(EMLU);
        }
        private void Opp5_closebtnHandaler(object sender, RoutedEventArgs e)
        {
            EMLU.Close();
        }
        private void EMLU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (EMLU != null)
            {
                EmptyStockFormClose();
            }
            EMLU = null;
        }
        void EmptyStockFormClose()
        {
            OpenWindowNameHolder.Children
                  .Remove(OpenWindowNameHolder
                  .FindName("EMLU") as OpendWindowElement);
            UnregisterName("EMLU");
        }
        //onShowClick
        void onShowClick(Window w)
        {
            w.Focus();
            w.WindowState = WindowState.Normal;
        }











        //Themes and Color Option
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(DashBoard));
        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }
        private void SetTheme(Theme value)
        {
            currntTheme = value;


            navBar.Background = value.MainBodyBackColor;
            btnLogOut.Background = value.ContainerBackColor;
            refTxt.Foreground = value.ContainerBackColor;
            btnRefresh.Background = value.InfoBackColor;


            foreach (CardsControl e in firstLevelPanel.Children)
            {
                e.ControlTheme = value;
            }
            foreach (CardsControl e in secondLevelPanel.Children)
            {
                e.ControlTheme = value;
            }
            foreach (CardsControl e in thirdLevelPanel.Children)
            {
                e.ControlTheme = value;
            }

            OpenWindowtabColor();
            OpenWindowsColor();
            CurrentThemeCLass.currentTheme = value;
        }
        private void btnCPu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.Purple);
            themename = "purple";
        }
        private void btnCGr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.Green);
            themename = "green";
        }
        private void btnCRe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.Red);
            themename = "red";
        }
        private void btnCDr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.dark);
            themename = "dark";
        }
        private void btnCNi_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.night);
            themename = "night";
        }
        private void btnCBlu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.blue);
            themename = "blue";
        }
        private void btnCDefal_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        void OpenWindowtabColor()
        {
            foreach (OpendWindowElement e in OpenWindowNameHolder.Children)
                e.ControlTheme = currntTheme;
        }
        void OpenWindowsColor()
        {

            if (MFRW != null)
                MFRW.ControlTheme = currntTheme;
            if (MDLW != null)
                MDLW.ControlTheme = currntTheme;

        }

        private void UCAbout_detailBtnHadler(object sender, RoutedEventArgs e)
        {
            new AboutScreen().ShowDialog();
        }


    }
}
