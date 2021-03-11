using System;
using System.Collections;
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
    /// Interaction logic for MedicineBatchUpdateControl.xaml
    /// </summary>
    public partial class MedicineBatchUpdateControl : UserControl
    {
        public static int kp = 0;
        public int Id = 0;

        /* Action Delegates*/
        public delegate void editItemDelegate(string name);
        public editItemDelegate thisControlRemoveEvent;
        public editItemDelegate thisControlSaveButtonEvent;
        public editItemDelegate thisComboBoxItemChangeEvent;
        public editItemDelegate onNowHoldTextChangeChangeEvent;


        //This values can only set by it self at first Time Launch
        //Then you can't change it's value programetically after that.
        //But you can get the value, can't set it.
        public string medName { get; set; } = string.Empty;
        public string nowHold { get; set; } = "0";
        public string entryDate { get; set; }
        public bool IsBtnDeleteEnable { get; set; } = true; //by default enable.
        public bool bSave { get; set; } = true;
       
        public MedicineBatchUpdateControl()
        {
            InitializeComponent();
            DataContext = this;
            Id = kp; //kp will not reset untill you lauch a new form.
            kp++;
            this.Name = "p" + Id;
        }


        /*Dependencies*/
        
        public static readonly DependencyProperty itemSourceProperty =
            DependencyProperty.Register("ItemSoureceForComboBox", typeof(IEnumerable), typeof(MedicineBatchUpdateControl) );

        public IEnumerable ItemSoureceForComboBox
        {
            get { return (IEnumerable)GetValue(itemSourceProperty); }
            set { SetValue(itemSourceProperty, value); comMed.ItemsSource = value; }
        }


        public static readonly DependencyProperty manfNameProperty =
            DependencyProperty.Register("manfName", typeof(string), typeof(MedicineBatchUpdateControl));
        public string manfName
        {
            set { SetValue(manfNameProperty, value); comMau.Text = value; }
            get { return (string)GetValue(manfNameProperty);  }
        }



        public static readonly DependencyProperty ButtonStateProperty =
            DependencyProperty.Register("IsSaveButtonEanble", typeof(bool), typeof(MedicineBatchUpdateControl));
        public bool IsSaveButtonEanble
        {
            get { return (bool)GetValue(ButtonStateProperty); }
            set { SetValue(ButtonStateProperty, value); btnSave.IsEnabled = value; }
        }
        


        public static readonly DependencyProperty validColorProperty =
            DependencyProperty.Register("validColor", typeof(SolidColorBrush), typeof(MedicineBatchUpdateControl));
        public SolidColorBrush validColor
        {
            set { SetValue(validColorProperty, value); tickValid.Fill = value; }
        }



        public static readonly DependencyProperty batchNumberProperty =
            DependencyProperty.Register("cuurtBatc", typeof(int), typeof(MedicineBatchUpdateControl));
        public int cuurtBatc
        {
            set { SetValue(batchNumberProperty, value); }
            get { return (int)GetValue(batchNumberProperty); }
        }


        public static readonly DependencyProperty OnHoldProperty =
            DependencyProperty.Register("onHold", typeof(int), typeof(MedicineBatchUpdateControl));
        public int onHold
        {
            set { SetValue(OnHoldProperty, value); }
            get { return (int)GetValue(OnHoldProperty); }
        }

        public static readonly DependencyProperty priceProperty =
           DependencyProperty.Register("price", typeof(int), typeof(MedicineBatchUpdateControl));
        public int price
        {
            set { SetValue(priceProperty, value); }
            get { return (int)GetValue(priceProperty); }
        }


        public static readonly DependencyProperty expiDateProeperty =
            DependencyProperty.Register("expDate", typeof(DateTime), typeof(MedicineBatchUpdateControl));
        public DateTime expDate
        {
            set { SetValue(expiDateProeperty, value); txtExp.Text = value.ToShortDateString(); }
            get { return (DateTime)GetValue(expiDateProeperty); }
        }

        /* Action*/
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            thisControlRemoveEvent(this.Name);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            thisControlSaveButtonEvent(this.Name);
        }

        private void comMed_TextChanged(object sender, TextChangedEventArgs e)
        {
            thisComboBoxItemChangeEvent(this.Name);
        }

        private void comMed_LostFocus(object sender, RoutedEventArgs e)
        {
            Foreground = new SolidColorBrush(Colors.White);
        }

        private void comMed_GotFocus(object sender, RoutedEventArgs e)
        {
            Foreground = new SolidColorBrush(Colors.Orange);
        }

        


    }
}
