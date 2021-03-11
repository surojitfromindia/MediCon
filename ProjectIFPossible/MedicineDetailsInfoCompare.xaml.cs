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
using System.Windows.Shapes;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for MedicineDetailsInfo.xaml
    /// </summary>
    public partial class MedicineDetailsInfoCompare : Window
    {
        private string medicinenamefromtextbox1;
        private MediInfoWindowCard2 mdic1;
        private string medicinenamefromtextbox2;
        private MediInfoWindowCard2 mdic2;
        private string medicinenamefromtextbox3;
        private MediInfoWindowCard2 mdic3;
        private TextBlock errorText = new TextBlock();
        public MedicineDetailsInfoCompare()
        {
            InitializeComponent();
            {
                errorText.Text = $"Sorry, No Medicine Found By Name  ";
                errorText.Margin = new Thickness(15);
                errorText.FontSize = 23.0;
                errorText.Foreground = new SolidColorBrush(Colors.White);
                errorText.FontFamily = new FontFamilyConverter().ConvertFromString("Montserrat Light") as FontFamily;
            }
        }

        //Expand\Collapse Button Click
        private void btnAddMed1_Click(object sender, RoutedEventArgs e)
        {
            ExpandPanel1();
        }
        private void ExpandPanel1()
        {
            if (med1.Visibility == Visibility.Collapsed)
            {
                med1.Visibility = Visibility.Visible;
                rowdef1.Height = new GridLength(1, GridUnitType.Star);
                btnAddMed1.Content = '-';
            }
            else if (med1.Visibility == Visibility.Visible)
            {
                med1.Visibility = Visibility.Collapsed;
                rowdef1.Height = GridLength.Auto;
                btnAddMed1.Content = '+';
            }
        }
        private void btnAddMed2_Click(object sender, RoutedEventArgs e)
        {
            ExpandPanel2();
        }
        private void ExpandPanel2()
        {
            if (med2.Visibility == Visibility.Collapsed)
            {
                med2.Visibility = Visibility.Visible;
                rowdef2.Height = new GridLength(1, GridUnitType.Star);
                btnAddMed2.Content = '-';
            }
            else if (med2.Visibility == Visibility.Visible)
            {
                med2.Visibility = Visibility.Collapsed;
                rowdef2.Height = GridLength.Auto;
                btnAddMed2.Content = '+';
            }
        }
        private void btnAddMed3_Click(object sender, RoutedEventArgs e)
        {
            ExpandPanel3();
        }
        private void ExpandPanel3()
        {
            if (med3.Visibility == Visibility.Collapsed)
            {
                med3.Visibility = Visibility.Visible;
                rowdef3.Height = new GridLength(1, GridUnitType.Star);
                btnAddMed3.Content = '-';
            }
            else if (med3.Visibility == Visibility.Visible)
            {
                med3.Visibility = Visibility.Collapsed;
                rowdef3.Height = GridLength.Auto;
                btnAddMed3.Content = '+';
            }
        }



        //Search Button Click
        private void b1_Click(object sender, RoutedEventArgs e)
        {
            LoadSearchResult1();
            med1.Visibility = Visibility.Visible;
        }
        private void b2_Click(object sender, RoutedEventArgs e)
        {
            LoadSearchResult2();
            med2.Visibility = Visibility.Visible;
        }
        private void b3_Click(object sender, RoutedEventArgs e)
        {
            LoadSearchResult3();
            med3.Visibility = Visibility.Visible;
        }



        //Search Results
        void LoadSearchResult1()
        {
            if (txtFMed.Text != string.Empty)
            {
                MED1.Children.Clear();
                medicinenamefromtextbox1 = txtFMed.Text;
                //Create The Control
                mdic1 = new MediInfoWindowCard2(medicinenamefromtextbox1)
                {
                    //Hide The Color Changing Buttons.
                    ControlTheme = Themes.Purple,
                    isColorChangeButtonVisible = Visibility.Visible
                };
                //If dataRecived is true then Show
                if (mdic1.dataRecived == true)
                {
                    MED1.Children.Add(mdic1);
                }
                //Else show a error Text;
                else
                {
                   /* errorText.Text = $"Sorry, No Medicine Found On Name \"{medicinenamefromtextbox1}\" ";
                    MED1.Children.Add(errorText);*/
                }
                txtFMed.Focus();
            }
        }
        void LoadSearchResult2()
        {
            if (txtSMed.Text != string.Empty)
            {
                MED2.Children.Clear();
                medicinenamefromtextbox2 = txtSMed.Text;
                //Create The Control
                mdic2 = new MediInfoWindowCard2(medicinenamefromtextbox2)
                {
                    //Hide The Color Changing Buttons.
                    ControlTheme = Themes.Green,
                    isColorChangeButtonVisible = Visibility.Visible
                };
                //If dataRecived is true then Show
                if (mdic2.dataRecived == true)
                {
                    MED2.Children.Add(mdic2);
                }
                //Else show a error Text;
                else
                {
                   /* errorText.Text = $"Sorry, No Medicine Found On Name \"{medicinenamefromtextbox2}\" ";
                    MED2.Children.Add(errorText);*/
                }
                txtSMed.Focus();
            }
        }
        void LoadSearchResult3()
        {
            if (txtTMed.Text != string.Empty)
            {
                MED3.Children.Clear();
                medicinenamefromtextbox3 = txtTMed.Text;
                //Create The Control
                mdic3 = new MediInfoWindowCard2(medicinenamefromtextbox3)
                {
                    //Hide The Color Changing Buttons.
                    ControlTheme = Themes.dark,
                    isColorChangeButtonVisible = Visibility.Visible
                };
                //If dataRecived is true then Show
                if (mdic3.dataRecived == true)
                {
                    MED3.Children.Add(mdic3);
                }
                //Else show a error Text;
                else
                {
                   /* errorText.Text = $"Sorry, No Medicine Found On Name \"{medicinenamefromtextbox3}\" ";
                    MED3.Children.Add(errorText);*/
                }
                txtTMed.Focus();
            }
        }
    }
}
