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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectIFPossible.ConnectionRouter.MySqlClasses.Models;

namespace ProjectIFPossible
{
    public partial class MediInfoWindowCard2 : UserControl
    {
        //state for five buttons
        int[] buttonStates = { 1, 0, 0, 1, 0 };
        public Visibility isColorChangeButtonVisible { get; set; } = Visibility.Visible;
        private string medicinename = "Bozin";
        private List<int> allBatchNumbers = new List<int>(10);

        // return false if the medicine name is not found in cataloge and 
        // this Window Will Show The Default Template;
        //See BindData medthod for more.
        public bool dataRecived = false;

        Medicine medicine;
        Medicine medicine2;
        public MediInfoWindowCard2()
        {
            InitializeComponent();
            DataContext = this;
            SetTheme(CurrentThemeCLass.currentTheme);
        }

        public MediInfoWindowCard2(string medicinename)
        {
            InitializeComponent();
            DataContext = this;
            this.medicinename = medicinename;
            BindData();
            SetTheme(CurrentThemeCLass.currentTheme);
        }


        void BindData()
        {
            medicine = new Medicine(medicinename).MedicineLongInfo();
            medicine2 = new Medicine(medicinename).MediShortInfo();

            //Medicine name heading
            lblMedName.Text = medicinename;

            if (medicine != null && medicine2 != null)
            {
                storeBatchNumbers();
                //Medicine Overall details block
                lblTotalBatch.Text = medicine2.medicineOverAllInfo.batchs.ToString();
                lblTotalStock.Text = medicine2.medicineOverAllInfo.stocked.ToString();
                lblTotalSchedule.Text = medicine2.medicineScheduleInfo.noOfSch.ToString();
                lblLastScheduleDate.Text = " (" + medicine.medicineScheduleInfo.scheduleCreateDate.ToString("dd-MM-yy") + ")";
                lblLastBatch.Text = allBatchNumbers.Max().ToString();
                lblExpireBatch.Text = medicine2.medicineOverAllInfo.expiredB.ToString();

                //Medicine Registration Details, and Current Price, and Rating
                lblRegisterBy.Text = medicine.registeredBy;
                lblLatestPrice.Text = medicine2.medicineOverAllInfo.currentPrice.ToString();

                //Medicine Manufacturer Details
                lblCompayName.Text = medicine.manufacturerInfo.manufacturName;
                lblCompanyContact.Text = medicine.manufacturerInfo.contact;
                lblCompanyWebsite.Text = medicine.manufacturerInfo.website;
                lblCompayEmail.Text = medicine.manufacturerInfo.email;
                lblCompanyLoc.Text = medicine.manufacturerInfo.address;
                GeneratedCard();
                dataRecived = true;
            }
        }

        void storeBatchNumbers()
        {
            foreach (var e in medicine.perBatchInfoList)
            {
                allBatchNumbers.Add(e.batchs);
            }
        }
        void GeneratedCard()
        {
            foreach (MedicineGroup mg in medicine.perBatchInfoList)
            {
                bool tp = mg.status == "OK" ? true : false;
                BatchCard bbc = new BatchCard(mg.batchs, mg.stocked, mg.entryDate, mg.expDate, mg.currentPrice, tp);
                batchInfoHolder.Children.Add(bbc);
            }
        }




        /*Coloring Fuction*/
        void SetTheme(Theme theme)
        {
            lblMedName.Foreground = theme.FontColor;
            stkContainerBack.Background = theme.ContainerBackColor;
            stkInfoback.Background = theme.InfoBackColor;
            stkMainBack.Background = theme.MainBodyBackColor;
            foreach (BatchCard e in batchInfoHolder.Children)
            {
                e.ControlTheme = theme;
            }
            ColorButton(theme);
        }

        void ColorButton(Theme theme)
        {
            if (buttonStates[0] == 0)
                btnByBatch.Background = theme.ContainerBackColor;
            else
                btnByBatch.Background = theme.InfoBackColor;

            if (buttonStates[1] == 0)
                btnByEntry.Background = theme.ContainerBackColor;
            else
                btnByEntry.Background = theme.InfoBackColor;

            if (buttonStates[2] == 0)
                btnByExp.Background = theme.ContainerBackColor;
            else
                btnByExp.Background = theme.InfoBackColor;

            if (buttonStates[3] == 0)
                btnBySto.Background = theme.ContainerBackColor;
            else
                btnBySto.Background = theme.InfoBackColor;

            if (buttonStates[4] == 0)
                btnByStatus.Background = theme.ContainerBackColor;
            else
                btnByStatus.Background = theme.InfoBackColor;
            btnSearch.Background = theme.InfoBackColor;
        }

        private void btnCPu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.Purple);
        }

        private void btnCGr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.Green);
        }

        private void btnCRe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.Red);
        }

        private void btnCDr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.dark);
        }

        private void btnCNi_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.night);
        }

        private void btnCBlu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.blue);
        }

        void cc(Theme t)
        {
            this.ControlTheme = t;

        }

        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(MediInfoWindowCard2));

        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }
    }


    public class Theme
    {
        public SolidColorBrush FontColor { get; }
        public SolidColorBrush ContainerBackColor { get; }
        public SolidColorBrush InfoBackColor { get; }
        public SolidColorBrush MainBodyBackColor { get; }
        public Theme(string fc, string Cb, string Ib, string Mb)
        {
            FontColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(fc));
            ContainerBackColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Cb));
            InfoBackColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Ib));
            MainBodyBackColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Mb));
        }
 

    }
    public class Themes
    {
        public static Theme Purple => new Theme("#FFCBC7F3", "#FF7063F0", "#FF6253F7", "#FF6253F7");
        public static Theme Green => new Theme("#FFC8FFD4", "#FF087821", "#FF1C6834", "#FF1C6834");
        public static Theme Red => new Theme("#FFFFC8CC", "#FF9C0F1B", "#FF80131D", "#FF80131D");
        public static Theme dark => new Theme("#FFCDCDCD", "#FF4F4F4F", "#FF3A3939", "#FF3A3939");
        public static Theme night => new Theme("#FFFFFFFF", "#FF191616", "#FF2B2B2B", "#FF2B2B2B");
        public static Theme blue => new Theme("#FFC6E8FF", "#FF1F8AD3", "#FF095D95", "#FF006EB8");
        public static Theme DashCardDefault => new Theme("#FFFFFFFF","#FF0FDC76","#00000000", "#FF979797");
    }
}
