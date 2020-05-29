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

namespace ProjectIFPossible.MiniWindows
{
    /// <summary>
    /// Interaction logic for loginAgainWindow.xaml
    /// </summary>
    public partial class loginAgainWindow : Window
    {
        public delegate void logindelegate(string v, string p);
        public logindelegate connect;
        public string user { get; set; } = "";
        public string pass { get; set; } = "";
        public loginAgainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            connect(user, pass);
        }
    }
}
