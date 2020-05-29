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
    /// Interaction logic for MedcineEntryConfirmPopUpWindow.xaml
    /// </summary>
    public partial class MedcineEntryConfirmPopUpWindow : Window
    {
        public string passwordForConfirm = "";
        public bool isPasswordConfimed = false;
        public MedcineEntryConfirmPopUpWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox1.Text == passwordForConfirm)
            {
                isPasswordConfimed = true;
                this.Close();
            }
            else
            { 
                txterror.Visibility = Visibility.Visible;               
                txtBox1.Focus();
            }
        }

        private void TxtBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            txterror.Visibility = Visibility.Collapsed;
        }
    }
}
