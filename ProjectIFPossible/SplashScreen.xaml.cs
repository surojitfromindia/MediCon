using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        LoginWindow lgw;
        public SplashScreen()
        {
            InitializeComponent();
            lgw = new LoginWindow();
            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(1);
            dt.Tick += timer_Tick;
            dt.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
          
            pbarFake.Value += 0.9;
            if (pbarFake.Value == 100) { Close(); lgw.Show();  }

        }
    }
}
