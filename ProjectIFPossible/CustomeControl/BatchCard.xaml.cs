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

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for BatchCard.xaml
    /// </summary>
    public partial class BatchCard : UserControl
    {
        private int batchNumber { get; set; }
        private int batchStorage { get; set; } = 0;
        private DateTime entryDate { get; set; }
        private DateTime expDate { get; set; }
        private double price { get; set; } = 0;
        private bool status;
        public BatchCard()
        {
            InitializeComponent();
          
        }

        public BatchCard(int batchNumber, int batchStorage, DateTime entryDate, DateTime expDate, double price, bool st )
        {
            InitializeComponent();
            this.batchNumber = batchNumber;
            this.batchStorage = batchStorage;
            this.entryDate = entryDate;
            this.expDate = expDate;
            this.price = price;
            status = st;
            FillField();
        }



        void FillField()
        {
            lblBatchNumber.Text = batchNumber.ToString();
            lblBatchSotrage.Text = batchStorage.ToString();
            lblEnDate.Text = entryDate.ToString("dd-MM-yy");
            lblExpDate.Text = expDate.ToString("dd-MM-yy");
            lblBatchPrice.Text = price.ToString();
            lblBatchValue.Text = (price * batchStorage).ToString();
        }


        void SetTheme(Theme theme)
        {
            if(status==false)
                cWindow.Background = Themes.Red.InfoBackColor;
            else
                cWindow.Background = theme.ContainerBackColor;
        }
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(BatchCard));
        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }
        public static readonly DependencyProperty expireIndicationProperty = DependencyProperty.Register("ExpColor", typeof(Theme), typeof(BatchCard));
        public Theme ExpColor
        {
            set { SetValue(expireIndicationProperty, value); SetTheme(value); }
        }


    }
}
