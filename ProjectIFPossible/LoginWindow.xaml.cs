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
        public LoginWindow()
        {
            InitializeComponent();
            _ = new mySqlConnection();

        }

        private void TextBlock_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Notified");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string u = "surojit";//txtU.Text;
            string p = "123@uio";// txtP.Password;                       

            mySqlConnection msC2 = new mySqlConnection(u,p);
            if (msC2.ValidateConnection())
            {
   
                msC2.Connect();
                DashBoard dt = new DashBoard();
                dt.Owner = this;
                dt.ShowDialog();
             
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
