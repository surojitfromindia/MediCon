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

    public partial class MedicineDetailsInfoWindow : Window
    {
        private string medicinenamefromtextbox = "";
        private MediInfoWindowCard2 mdic2;
        private Theme currentTheme= Themes.blue;
        private TextBlock errorText = new TextBlock();
        private SolidColorBrush White = new SolidColorBrush(Colors.White);
        private SolidColorBrush Black = new SolidColorBrush(Colors.Black);
      

        public MedicineDetailsInfoWindow()
        {
            InitializeComponent();
            {
                errorText.Text = $"Sorry, No Medicine Found By Name \"{medicinenamefromtextbox}\" or Stock Is EMPTY ";
                errorText.Margin = new Thickness(15);
                errorText.FontSize = 23.0;
                errorText.FontFamily = new FontFamilyConverter().ConvertFromString("Montserrat Light") as FontFamily;
            }
            

        }

        public MedicineDetailsInfoWindow(string medname)
        {
            InitializeComponent();
            {
                errorText.Text = $"Sorry, No Medicine Found By Name \"{medicinenamefromtextbox}\" or Stock Is EMPTY";
                errorText.Margin = new Thickness(15);
                errorText.FontSize = 23.0;
                errorText.FontFamily = new FontFamilyConverter().ConvertFromString("Montserrat Light") as FontFamily;
            }
            txtSearchBox.Text = medname;
            LoadSearchResult();
           
        }


        //Theme
        private void btnCPu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.Purple);
        }

        private void btnCGr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.Green);
        }

        private void btnCRe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.Red);
        }

        private void btnCDr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.dark);
        }

        private void btnCNi_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetTheme(Themes.night);
        }

        private void btnCBlue_MouseDown(object sender, MouseButtonEventArgs e)
        {
          
            SetTheme(Themes.blue);
            errorText.Foreground = Black;
        }

      
        void SetTheme(Theme t)
        {
         
            if (mdic2 != null)
                mdic2.ControlTheme = t;
            holderPanel.Background = t.MainBodyBackColor;

            if (currentTheme == Themes.blue) //does't work . no object equality is implemented
                errorText.Foreground = Black;
            else
                errorText.Foreground = White;
           currentTheme = t;

        }
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(MedicineDetailsInfoWindow));
        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadSearchResult();
        }
        void LoadSearchResult()
        {
            if (txtSearchBox.Text != string.Empty)
            {
                holderPanel2.Children.Clear();
                medicinenamefromtextbox = txtSearchBox.Text;
                //Create The Control
                mdic2 = new MediInfoWindowCard2(medicinenamefromtextbox)
                {
                    //Hide The Color Changing Buttons.
                    isColorChangeButtonVisible = Visibility.Hidden
                };
                //If dataRecived is true then Show
                if (mdic2.dataRecived == true)
                {
                    mdic2.ControlTheme = currentTheme;
                    holderPanel2.Children.Add(mdic2);
                }
                //Else show a error Text;
                else
                {
                    errorText.Text = $"Sorry, No Medicine Found On Name \"{medicinenamefromtextbox}\" or Stock Is EMPTY ";
                    holderPanel2.Children.Add(errorText);
                }
                txtSearchBox.Focus();
            }
        }
    }
}
