using ProjectIFPossible.ConnectionRouter.MySqlClasses;
using ProjectIFPossible.ConnectionRouter.MySqlClasses.Models;
using ProjectIFPossible.CustomeControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for MediListCardControlSmall.xaml
    /// </summary>
    public partial class MediListCardControlSmall : UserControl
    {
        private string medName = "";
        private int stocked = 0;
        private int batch = 0;
        private int batchExp = 0;
        private int price = 0;
        private int sch = 0;


        //Currently in User
        public MediListCardControlSmall(string medName)
        {
            this.medName = medName;
            InitializeComponent();
            VisualDataOnControl();
            SetTheme(CurrentThemeCLass.currentTheme);
        }

        public MediListCardControlSmall()
        {
            InitializeComponent();
            VisualDataOnControl();
            SetTheme(CurrentThemeCLass.currentTheme);
        }

        //start a immutable on time-card
        public MediListCardControlSmall(string medName, int stocked, int batch, int batchExp, int price)
        {
            this.medName = medName;
            this.stocked = stocked;
            this.batch = batch;
            this.batchExp = batchExp;
            this.price = price;
            InitializeComponent();
            VisualDataOnControl();
            SetTheme(CurrentThemeCLass.currentTheme);
            
        }

        void VisualDataOnControl()
        {
            lblMedicine.Text = medName;
            lblStocked.Text = stocked.ToString();
            lblB.Text = batch.ToString();
            lblBEXp.Text = batchExp.ToString();
            lblPrice.Text = price.ToString();
            lblSchdelue.Text = sch.ToString();
        }
        
        public static readonly DependencyProperty stockedPropetry = DependencyProperty.Register("StockedP", typeof(int), typeof(MediListCardControlSmall));
        public static readonly DependencyProperty pricePropetry = DependencyProperty.Register("PriceP", typeof(int), typeof(MediListCardControlSmall));
        public static readonly DependencyProperty batchPropetry = DependencyProperty.Register("BatchP", typeof(int), typeof(MediListCardControlSmall));
        public static readonly DependencyProperty bacthEPropetry = DependencyProperty.Register("BatchEP", typeof(int), typeof(MediListCardControlSmall));
        public static readonly DependencyProperty scheduleEPropetry = DependencyProperty.Register("Schedule", typeof(int), typeof(MediListCardControlSmall));


        public int StockedP
        {
            get { return (int)GetValue(stockedPropetry); }
            set { SetValue(stockedPropetry, value); lblStocked.Text = value.ToString(); }
        }
        public int PriceP
        {
            get { return (int)GetValue(stockedPropetry); }
            set { SetValue(stockedPropetry, value); lblPrice.Text = value.ToString(); }
        }
        public int BatchP
        {
            get { return (int)GetValue(stockedPropetry); }
            set { SetValue(stockedPropetry, value); lblB.Text = value.ToString(); }
        }
        public int BatchEP
        {
            get { return (int)GetValue(stockedPropetry); }
            set { SetValue(stockedPropetry, value); lblBEXp.Text = value.ToString(); }
        }

        public int Schedule
        {
            get { return (int)GetValue(scheduleEPropetry); }
            set { SetValue(scheduleEPropetry, value); lblSchdelue.Text = value.ToString(); }
        }


        private void btnSchdelue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MedicineScheduleInfo medicineSchedule = new MedicineScheduleInfo(medName);
            ScheduleQuickWindow frm = new ScheduleQuickWindow(medName);
            frm.ShowDialog();
            Schedule = medicineSchedule.GetScheduleInfo().noOfSch;
            HighLigthSchWithColor();

        }

        private void lblSchdelue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HighLigthSchWithColor();            
        }


        void HighLigthSchWithColor()
        {

            if (Schedule > 120)
            {
                lblSchdelue.Foreground = new SolidColorBrush(Colors.Yellow);
                btnSchdelue.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else if (Schedule > 60) { 
                lblSchdelue.Foreground = new SolidColorBrush(Colors.Orange);
                btnSchdelue.Foreground = new SolidColorBrush(Colors.Orange);
            }
              
            else if (Schedule > 0)
            {
                lblSchdelue.Foreground = new SolidColorBrush(Colors.White);
                
                btnSchdelue.Foreground = new SolidColorBrush(Colors.White);
            }
            else if (Schedule == 0)
            {
                lblSchdelue.Foreground = new SolidColorBrush(Colors.Gray);
                btnSchdelue.Foreground = new SolidColorBrush(Colors.Gray);
            }
          
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           HighLigthSchWithColor();
        }


        void LoadAgain()
        {
            MedicineScheduleInfo ss = new MedicineScheduleInfo(lblMedicine.Text);
            Schedule = ss.GetScheduleInfo().noOfSch;
            Medicine thisMed = new Medicine(lblMedicine.Text).MediShortInfo();
            StockedP = thisMed.medicineOverAllInfo.stocked;
            PriceP = thisMed.medicineOverAllInfo.currentPrice;
            BatchP = thisMed.medicineOverAllInfo.batchs;
            BatchEP = thisMed.medicineOverAllInfo.expiredB;
            Schedule = thisMed.medicineScheduleInfo.noOfSch;
        }

        private void lblRef_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadAgain();
        }

        private void lblMedicine_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MedicineDetailsInfoWindow medicineDetailsInfoWind = new MedicineDetailsInfoWindow(medName);
            medicineDetailsInfoWind.ShowDialog();
        }


        //Theme
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(MediListCardControlSmall));

        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }

        private void SetTheme(Theme value)
        {
            refBack.Background = CurrentThemeCLass.currentTheme.MainBodyBackColor;

        }
    }
}
