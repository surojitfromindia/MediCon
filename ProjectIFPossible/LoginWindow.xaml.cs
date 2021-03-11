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
using ProjectIFPossible.ConnectionRouter;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        DashBoard dt;
        public LoginWindow()
        {
            InitializeComponent();
          
        }

        private void TextBlock_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Notified");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string u = txtU.Text;
            string p = txtP.Password;                       

            mySqlConnection msC2 = new mySqlConnection(u,p);
            if (msC2.ValidateConnection())
            {
   
                msC2.Connect();
                dt = new DashBoard();
               /* dt.Owner = this;*/
                dt.Show();
                Close();
             
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
