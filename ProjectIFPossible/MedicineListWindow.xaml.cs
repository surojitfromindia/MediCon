using ProjectIFPossible.ConnectionRouter.MySqlClasses;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using PillControlClass;

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

        bool isSamllCard = false;
        bool isBigCard = true;
      
        public MedicineListWindow()
        {
          
            InitializeComponent();
            FillWithData(AllMedicine,false);
            UpdateResultCountChips(AllMedicine);
            pillStock.xCloseClick = CloseStockFillter;
            pillBatch.xCloseClick = CloseSBatchFillter;
            pillRating.xCloseClick = CloseRatingFillter;
            pillPrice.xCloseClick = ClosePriceFillter;

            noRecordWarningText.FontSize = 30;
            noRecordWarningText.Text = "Sorry! No Record Found";

            
        }

        private string Nameing(string Name)
        {
            string name=Name;
            if(Name.Contains(" "))
            {
                name = Name.Replace(" ", string.Empty);
            }
            return name;
        }

        private void btnDebug_Click(object sender, RoutedEventArgs e)
        {
            if (filterPanel.Visibility == Visibility.Collapsed)
                filterPanel.Visibility = Visibility.Visible;
            else if(filterPanel.Visibility == Visibility.Visible)
                filterPanel.Visibility = Visibility.Collapsed;
        }


        //Filtter and sorting Purpose Only
        private void btnApplyFiltter_Click(object sender, RoutedEventArgs e)
        {
            if(isBigCard)
                Fillter();
            if (isSamllCard)
                Fillter();
            UpdateResultCountChips(FilttedMedicineSet);
        }

        private void btnCleanFiltter_Click(object sender, RoutedEventArgs e)
        {
            FillWithData(AllMedicine,isSamllCard);
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

        void FillWithData(HashSet<Medicine> AllMedicine, bool isSmallCard)
        {
            ListPanel.Children.Clear();

            if(!isSamllCard)
                foreach (Medicine medicine in AllMedicine)
                {
                    MediListCardControl medInfoCard = new MediListCardControl(medicine.name,
                        medicine.medicineInfos.currentNumber, medicine.medicineInfos.batchs,
                        medicine.medicineInfos.expiredB, medicine.tags);
                    ListPanel.Children.Add(medInfoCard);
                }

            if(isSamllCard)
                foreach (Medicine medicine in AllMedicine)
                {
                    MediListCardControlSmall medInfoCard = new MediListCardControlSmall(medicine.name,
                        medicine.medicineInfos.currentNumber, medicine.medicineInfos.batchs,
                        medicine.medicineInfos.expiredB);
                    ListPanel.Children.Add(medInfoCard);
                }


            if (ListPanel.Children.Count == 0)
                ListPanel.Children.Add(noRecordWarningText);
        }

        void Fillter()
        {
            FilttedMedicineSet.Clear();
            bool bStock = (bool)chkStocked.IsChecked;
            bool bBatch = (bool)chkBatch.IsChecked;
            Filtter fillterStock = new Filtter(int.Parse(txtChkStockedLow.Text), int.Parse(txtChkStockdeHigh.Text), bStock);
            Filtter fillterBatch = new Filtter(int.Parse(txtChkBatchLow.Text), int.Parse(txtChkBatchHigh.Text), bBatch);
            FilttedMedicineSet = new MySqlMedicineListing().FiltterMedicine(fillterStock, fillterBatch, txtMedicineSearchBox.Text);
            FillWithData(FilttedMedicineSet, isSamllCard);
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
            Chips chip = new Chips(Colors.DarkCyan,ts.Count.ToString());
            chip.Width = 40;
            panel2.Children.Add(chip);
            
        }




        //Two type of Cards exchange
        private void btnBigCard_Click(object sender, RoutedEventArgs e)
        {
            isSamllCard = false;
            isBigCard = true;
            ListPanel.Children.Clear();
            foreach (Medicine medicine in AllMedicine)
            {
                MediListCardControl medInfoCard = new MediListCardControl(medicine.name,
                    medicine.medicineInfos.currentNumber, medicine.medicineInfos.batchs,
                    medicine.medicineInfos.expiredB,medicine.tags);
                ListPanel.Children.Add(medInfoCard);
            }
            if (ListPanel.Children.Count == 0)
                ListPanel.Children.Add(noRecordWarningText);
        }

        private void btnSmallCard_Click(object sender, RoutedEventArgs e)
        {
            isSamllCard = true;
            isBigCard = false;
            ListPanel.Children.Clear();
            foreach (Medicine medicine in AllMedicine)
            {
                MediListCardControlSmall medInfoCard = new MediListCardControlSmall(medicine.name,
                    medicine.medicineInfos.currentNumber, medicine.medicineInfos.batchs,
                    medicine.medicineInfos.expiredB);
                ListPanel.Children.Add(medInfoCard);
            }
            if (ListPanel.Children.Count == 0)
                ListPanel.Children.Add(noRecordWarningText);
        }



        //Name Search
        private void btnNameSearch_Click(object sender, RoutedEventArgs e)
        {
            Fillter();
        }

        private void txtMedicineSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtMedicineSearchBox.Text != String.Empty)
                Fillter();
            else
                FillWithData(AllMedicine, isSamllCard);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            FillWithData(AllMedicine, isSamllCard);
        }
    }
}
