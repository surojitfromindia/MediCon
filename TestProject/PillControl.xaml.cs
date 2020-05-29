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

namespace TestProject
{
    /// <summary>
    /// Interaction logic for PillControl.xaml
    /// </summary>
    public partial class PillControl : UserControl
    {
        public delegate void removeThisControlDelegate(string ControlName);
        public removeThisControlDelegate xCloseClick;
        public string textValue { get; set; } = "Pill Values goes here";
        public Brush fillColor { get; set; }
        public SolidColorBrush foreColor { get; set; } = new SolidColorBrush(Colors.White);
        public PillControl()
        {
            InitializeComponent();
            DataContext = this;
            Color cc = BodyColor();
            fillColor = new SolidColorBrush(cc);
            foreColor = new SolidColorBrush(ContraustColor(cc));

        }
        private void StackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            xClose.Visibility = Visibility.Visible;
        }

        private void xClose_Click(object sender, RoutedEventArgs e)
        {
            xCloseClick(Name);
        }
        private Color BodyColor()
        {
            Random j = new Random();
            byte r = (byte)j.Next(30, 255);
            byte g = (byte)j.Next(30, 255);
            byte b = (byte)j.Next(30, 255);           
            return Color.FromRgb(r, g, b);
        }

        private Color ContraustColor(Color color)
        {
            byte d = 0;
            double lumin = (0.299 * color.R + 0.587 * color.G + 0.114 * color.G) / 255;
            if (lumin > 0.5)
                d = 0;
            else
                d = 255;
            return Color.FromRgb(d, d, d);
        }

    }
}
