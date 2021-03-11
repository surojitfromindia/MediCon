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
    /// Interaction logic for CardsControl.xaml
    /// </summary>
    public partial class CardsControl : UserControl
    {
        Theme currentTheme;
        SolidColorBrush colorOnHoverEntry;
        SolidColorBrush colorOnHoverExit = new SolidColorBrush(Colors.Transparent);

        public string descp { get; set; } = "Descrption goes here.";
        public string textColor { get; set; } = "red";
        public string buttonText { get; set; } = "Details";
        public string button2Text { get; set; }
        public Visibility b2visibility { get; set; }
        public event RoutedEventHandler detailBtnHadler;
        public event RoutedEventHandler refreshBtnHadler;

        public CardsControl()
        {
           
            InitializeComponent();
            DataContext = this;
            SetTheme(Themes.Green);
           
        }
 
       


        /* Hover event*/
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            gdBack.Background = colorOnHoverEntry;
            //opPanel.Visibility = Visibility.Visible;
        }

        private void gdBack_MouseLeave(object sender, MouseEventArgs e)
        {
            gdBack.Background = colorOnHoverExit;
            //opPanel.Visibility = Visibility.Collapsed;
        }
        /* end hover event*/

        private void detail_btn_Click(object sender, RoutedEventArgs e)
        {
            detailBtnHadler?.Invoke(this, e);
        }

        private void refresh_btn_click(object sender, RoutedEventArgs e)
        {
            refreshBtnHadler?.Invoke(this, e);
        }

        
        public static readonly DependencyProperty textProperty =
            DependencyProperty.Register("text", typeof(string), typeof(CardsControl), new UIPropertyMetadata(string.Empty
           ));
        public string text
        {
            get { return (string)GetValue(textProperty); }
            set { SetValue(textProperty, value); }
        }

        public static readonly DependencyProperty basicPropetry =
           DependencyProperty.Register("Basic", typeof(string), typeof(CardsControl), new UIPropertyMetadata(string.Empty
          ));
        public string Basic
        {
            get { return (string)GetValue(basicPropetry); }
            set { SetValue(basicPropetry, value); }
        }

        public static readonly DependencyProperty buttonColorProperty =
           DependencyProperty.Register("ButtonColor", typeof(string), typeof(CardsControl));
        public SolidColorBrush ButtonColor
        {
            get { return (SolidColorBrush)GetValue(buttonColorProperty); }
            set { SetValue(buttonColorProperty, value); leftButton.Background = value; RightButton.Background = value; }
        }

        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(CardsControl));

        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }

        private void SetTheme(Theme value)
        {
            currentTheme = value;
            leftButton.Background = value.MainBodyBackColor;
            RightButton.Background = value.MainBodyBackColor;
            refBasic.Foreground = value.ContainerBackColor;
            ImagG.Foreground = value.InfoBackColor;
            colorOnHoverEntry = value.FontColor;
        }
    }
}
