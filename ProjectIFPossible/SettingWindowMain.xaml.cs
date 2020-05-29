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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class settingWindow : Window
    {
        private const string USER_NAME = "Admin";
       
        public settingWindow()
        {
            InitializeComponent();
            comboUserType.SelectedIndex = 0;
           
        }

       

        private void extendExtraField(object sender, EventArgs e)
        {
            if (comboUserType.Text == USER_NAME)
                txtSecondPassAdmin.Visibility = Visibility.Visible;
            else txtSecondPassAdmin.Visibility = Visibility.Collapsed;
        }

        

        private void btnSettingMainLogin_Click(object sender, RoutedEventArgs e)
        {
            UserCreadential put = new UserCreadential(txtUserID.Text, txtPass1.Password, txtSecondPassAdmin.Password);
            if (put.validate())
                MessageBox.Show("Successful");

        }
    }
}
