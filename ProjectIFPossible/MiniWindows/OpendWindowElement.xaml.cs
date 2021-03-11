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

namespace ProjectIFPossible.MiniWindows
{
    
    public partial class OpendWindowElement : UserControl
    {
        public event RoutedEventHandler closebtnHandaler;
        public event RoutedEventHandler showbtnHandaler;

        public OpendWindowElement(string windowname)
        {
            InitializeComponent();
            txtWindowname.Text = windowname;
        }

        private void btnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            closebtnHandaler?.Invoke(this, e);
        }

        private void txtWindowname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showbtnHandaler?.Invoke(this, e);
        }

        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(OpendWindowElement));

        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }

        private void SetTheme(Theme value)
        {
            Back.Background = value.ContainerBackColor;
        }
    }
}
