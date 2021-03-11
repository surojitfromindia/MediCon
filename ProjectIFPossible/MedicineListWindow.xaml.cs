using ProjectIFPossible.ConnectionRouter.MySqlClasses;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using PillControlClass;
using ProjectIFPossible.ConnectionRouter.MySqlClasses.Models;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for MedicineListWindow.xaml
    /// </summary>
    public partial class MedicineListWindow : Window
    {

        HashSet<Medicine> AllMedicine = new MySqlMedicineListing().GetMedicines();
        HashSet<Medicine> FilttedMedicineSet = new HashSet<Medicine>(20);
        TextBlock noRecordWarningText = new TextBlock();
        TextBlock noRecordWarningTextSmall = new TextBlock();

        bool isSamllCard = true;
        bool isBigCard = false;
        private Theme currentTheme;

        public MedicineListWindow()
        {

            InitializeComponent();
            //
            FillWithData(AllMedicine);
            UpdateResultCountChips(AllMedicine);
            UpdateOverView();
            pillStock.xCloseClick = CloseStockFillter;
            pillBatch.xCloseClick = CloseSBatchFillter;
            pillRating.xCloseClick = CloseRatingFillter;
            pillPrice.xCloseClick = ClosePriceFillter;

            noRecordWarningText.FontSize = 30;
            noRecordWarningText.Text = "Sorry! No Record Found";
            noRecordWarningTextSmall.FontSize = 20;
            noRecordWarningTextSmall.Text = "Sorry! No Record Found";



        }

        private string Nameing(string Name)
        {
            string name = Name;
            if (Name.Contains(" "))
            {
                name = Name.Replace(" ", string.Empty);
            }
            return name;
        }

        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            if (filterPanel.Visibility == Visibility.Collapsed)
                filterPanel.Visibility = Visibility.Visible;
            else if (filterPanel.Visibility == Visibility.Visible)
                filterPanel.Visibility = Visibility.Collapsed;
        }


        //Filtter and sorting Purpose Only
        private void btnApplyFiltter_Click(object sender, RoutedEventArgs e)
        {
            if (isBigCard)
                Fillter();
            if (isSamllCard)
                Fillter();
            UpdateResultCountChips(FilttedMedicineSet);
        }

        private void btnCleanFiltter_Click(object sender, RoutedEventArgs e)
        {
            FillWithData(AllMedicine);
            UpdateResultCountChips(AllMedicine);
            chkBatch.IsChecked = false;
            chkPrice.IsChecked = false;
            chkRating.IsChecked = false;
            chkStocked.IsChecked = false;
            pillBatch.Visibility = Visibility.Collapsed;
            pillStock.Visibility = Visibility.Collapsed;
            pillRating.Visibility = Visibility.Collapsed;
            pillPrice.Visibility = Visibility.Collapsed;
            panelBatch.IsEnabled = false;
            panelPrice.IsEnabled = false;
            panelStock.IsEnabled = false;
            panelRating.IsEnabled = false;

        }

        void FillWithData(HashSet<Medicine> AllMedicine)
        {
            //clear each panel before filling up
            ListPanel.Children.Clear();
            ListPanel2.Children.Clear();

            if (!isSamllCard)
                foreach (Medicine medicine in AllMedicine)
                {
                    MediListCardControl medInfoCard = new MediListCardControl(medicine.name,
                        medicine.medicineOverAllInfo.stocked, medicine.medicineOverAllInfo.batchs,
                        medicine.medicineOverAllInfo.expiredB, medicine.medicineOverAllInfo.currentPrice, medicine.tags);
                    ListPanel.Children.Add(medInfoCard);
                }

            if (isSamllCard)
                foreach (Medicine medicine in AllMedicine)
                {
                    MediListCardControlSmall medInfoCard = new MediListCardControlSmall(medicine.name);
                    medInfoCard.StockedP = medicine.medicineOverAllInfo.stocked;
                    medInfoCard.PriceP = medicine.medicineOverAllInfo.currentPrice;
                    medInfoCard.BatchP = medicine.medicineOverAllInfo.batchs;
                    medInfoCard.BatchEP = medicine.medicineOverAllInfo.expiredB;
                    medInfoCard.Schedule = medicine.medicineScheduleInfo.noOfSch;
                    ListPanel2.Children.Add(medInfoCard);
                }


            if (ListPanel.Children.Count == 0)
            {
                ListPanel.Children.Clear();
                ListPanel.Children.Add(noRecordWarningText);
            }
            if (ListPanel2.Children.Count == 0)
            {
                ListPanel2.Children.Clear();
                ListPanel2.Children.Add(noRecordWarningTextSmall);
            }
        }

        void Fillter()
        {
            FilttedMedicineSet.Clear();
            bool bStock = (bool)chkStocked.IsChecked;
            bool bBatch = (bool)chkBatch.IsChecked;
            bool bPrice = (bool)chkPrice.IsChecked;
            Fillter fillterStock = new Fillter(int.Parse(txtChkStockedLow.Text), int.Parse(txtChkStockdeHigh.Text), bStock);
            Fillter fillterBatch = new Fillter(int.Parse(txtChkBatchLow.Text), int.Parse(txtChkBatchHigh.Text), bBatch);
            Fillter fillterPrice = new Fillter(int.Parse(txtChkPriceLow.Text), int.Parse(txtChkPriceHigh.Text), bPrice);

            FilttedMedicineSet = new MySqlMedicineListing().FiltterMedicine(fillterStock, fillterBatch, fillterPrice, txtMedicineSearchBox.Text);
            FillWithData(FilttedMedicineSet);
        }

        void CloseStockFillter(string pubName)
        {
            PillControl pp = (PillControl)FindName(pubName);
            pp.Visibility = Visibility.Collapsed;
            chkStocked.IsChecked = false;
            panelStock.IsEnabled = false;
            Fillter();
            UpdateResultCountChips(FilttedMedicineSet);
        }

        void CloseSBatchFillter(string pubName)
        {
            PillControl pp = (PillControl)FindName(pubName);
            pp.Visibility = Visibility.Collapsed;
            chkBatch.IsChecked = false;
            panelBatch.IsEnabled = false;
            Fillter();
            UpdateResultCountChips(FilttedMedicineSet);
        }

        void CloseRatingFillter(string pubName)
        {
            PillControl pp = (PillControl)FindName(pubName);
            pp.Visibility = Visibility.Collapsed;
            chkRating.IsChecked = false;
            panelRating.IsEnabled = false;
            Fillter();
            UpdateResultCountChips(FilttedMedicineSet);
        }

        void ClosePriceFillter(string pubName)
        {
            PillControl pp = (PillControl)FindName(pubName);
            pp.Visibility = Visibility.Collapsed;
            chkPrice.IsChecked = false;
            panelPrice.IsEnabled = false;
            Fillter();
            UpdateResultCountChips(FilttedMedicineSet);
        }


        private void chkStocked_Click(object sender, RoutedEventArgs e)
        {
            if (chkStocked.IsChecked == true)
            {
                pillStock.Visibility = Visibility.Visible;
                panelStock.IsEnabled = true;

            }
            else if (chkStocked.IsChecked == false)
            {
                pillStock.Visibility = Visibility.Collapsed;
                panelStock.IsEnabled = false;

            }
        }

        private void chkPrice_Click(object sender, RoutedEventArgs e)
        {
            if (chkPrice.IsChecked == true)
            {
                pillPrice.Visibility = Visibility.Visible;
                panelPrice.IsEnabled = true;

            }
            else if (chkPrice.IsChecked == false)
            {
                pillPrice.Visibility = Visibility.Collapsed;
                panelPrice.IsEnabled = false;

            }
        }

        private void chkBatch_Click(object sender, RoutedEventArgs e)
        {
            if (chkBatch.IsChecked == true)
            {
                pillBatch.Visibility = Visibility.Visible;
                panelBatch.IsEnabled = true;
            }
            else if (chkBatch.IsChecked == false)
            {
                pillBatch.Visibility = Visibility.Collapsed;
                panelBatch.IsEnabled = false;
            }
        }

        private void chkRating_Click(object sender, RoutedEventArgs e)
        {
            if (chkRating.IsChecked == true)
            {
                pillRating.Visibility = Visibility.Visible;
                panelRating.IsEnabled = true;
            }
            else if (chkRating.IsChecked == false)
            {
                pillRating.Visibility = Visibility.Collapsed;
                panelRating.IsEnabled = false;
            }
        }


        void UpdateResultCountChips(HashSet<Medicine> ts)
        {
            panel2.Children.Clear();
            Chips chip = new Chips((Color)ColorConverter.ConvertFromString("#FFFFEB21"), ts.Count.ToString());
            chip.Width = 40;
            panel2.Children.Add(chip);

        }




        //Two type of Cards exchange
        //look next defination for details
        private void btnBigCard_Click(object sender, RoutedEventArgs e)
        {
            isSamllCard = false;
            isBigCard = true;
            ListPanel2.Visibility = Visibility.Collapsed;
            ListPanel.Visibility = Visibility.Visible;
            ListPanel.Children.Clear();
            foreach (Medicine medicine in AllMedicine)
            {
                MediListCardControl medInfoCard = new MediListCardControl(medicine.name,
                    medicine.medicineOverAllInfo.stocked, medicine.medicineOverAllInfo.batchs,
                    medicine.medicineOverAllInfo.expiredB, medicine.medicineOverAllInfo.currentPrice, medicine.tags);
                ListPanel.Children.Add(medInfoCard);
            }
            if (ListPanel.Children.Count == 0)
            {
                ListPanel.Children.Clear();
                ListPanel.Children.Add(noRecordWarningText);
            }
        }
        private void btnSmallCard_Click(object sender, RoutedEventArgs e)
        {
            AllMedicine = new MySqlMedicineListing().GetMedicines();
            //small card is in current use = true
            isSamllCard = true;
            isBigCard = false;
            //first colllaspe ListPanel which hold long Card
            ListPanel.Visibility = Visibility.Collapsed;
            //Then visiable ListPanel2 which hold small card.
            ListPanel2.Visibility = Visibility.Visible;
            //clear child if any.
            ListPanel2.Children.Clear();
            //Get all medicine info from AllMedicine haset
            //rig the data to visual small card component
            //** small card does't have a 'tag' field.
            foreach (Medicine medicine in AllMedicine)
            {
                MediListCardControlSmall medInfoCard = new MediListCardControlSmall(medicine.name);
                medInfoCard.StockedP = medicine.medicineOverAllInfo.stocked;
                medInfoCard.PriceP = medicine.medicineOverAllInfo.currentPrice;
                medInfoCard.BatchP = medicine.medicineOverAllInfo.batchs;
                medInfoCard.BatchEP = medicine.medicineOverAllInfo.expiredB;
                medInfoCard.Schedule = medicine.medicineScheduleInfo.noOfSch;
                medInfoCard.ControlTheme = currentTheme;//OK
                //after riging the small card with value
                //add it to listpanel2 as children
                //name registration is'nt required cause we aren't 
                //chaging or accessing value at runtime
                //(Filltering will build the whole listpanel2 from scracth)
                ListPanel2.Children.Add(medInfoCard);
            }

            //if no small card is added (that is the fillter result return zero)
            //then add a "No Value Found" textblock for user information.
            //(don't forget to clear previouse message).
            if (ListPanel2.Children.Count == 0)
            {
                ListPanel2.Children.Clear();
                ListPanel2.Children.Add(noRecordWarningTextSmall);
            }
        }



        //Name Search
        private void btnNameSearch_Click(object sender, RoutedEventArgs e)
        {
            Fillter();
        }
        private void txtMedicineSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtMedicineSearchBox.Text != string.Empty)
                Fillter();
            //when textbox is clear return to original dataset without fillter
            else
                FillWithData(AllMedicine);
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //return to original dataset without fillter
            FillWithData(AllMedicine);
        }

        private void btnUpdateOverView_Click(object sender, RoutedEventArgs e)
        {
            UpdateOverView();
        }


        void UpdateOverView()
        {
            MySqlUtil.GEN_OVERVIEW_INFO();
            lblHPri.Text = MySqlUtil.max_price_med_name;
            lblHPVal.Text = "\u20B9" + MySqlUtil.max_price;
            lblLowPric.Text = MySqlUtil.min_price_med_name;
            lblLPVa.Text = "\u20B9" + MySqlUtil.min_price;

            lblHighStock.Text = MySqlUtil.max_stocked_med_name;
            lblHSVa.Text = MySqlUtil.max_stocked;
            lblLowStock.Text = MySqlUtil.min_stocked_med_name;
            lblLSVa.Text = MySqlUtil.min_stocked;

            lblHighBatch.Text = MySqlUtil.max_batch_med_name;
            lblHBVa.Text = "#" + MySqlUtil.max_batch;
            lblLowBatch.Text = MySqlUtil.min_batch_med_name;
            lblLBVa.Text = "#" + MySqlUtil.min_batch;


            MySqlUtil.TOTAL_MED_WORTH();
            lblTStockPrice.Text = MySqlUtil.totalMedicineWorth;
            var t1 = MySqlUtil.MediCineCount();
            lblInStorage.Text = $"{t1}({(t1 * 100 / 15000)}%)";

            lblTbatch.Text = MySqlUtil.BatchCount().ToString();
            lblTBExp.Text = MySqlUtil.ExpBatchCount().ToString();
        }



        private void btnBack_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        void SetTheme(Theme t)
        {
            currentTheme = t;
            refOverView.Background = t.MainBodyBackColor;
            BigHeading.Foreground = t.MainBodyBackColor;
            OverViewHeader.Background = t.ContainerBackColor;
            fillterBackDrop.Background = t.ContainerBackColor;
            txtMedicineSearchBox.BorderBrush = t.MainBodyBackColor;
            foreach (MediListCardControlSmall smc in ListPanel2.Children)
                if(smc!=null)
                    smc.ControlTheme = t;
        }

        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(MedicineListWindow));

        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }


    }
}

