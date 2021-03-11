using ProjectIFPossible.ConnectionRouter.MySqlClasses;
using ProjectIFPossible.ConnectionRouter.MySqlClasses.Models;
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
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Medicine p = new Medicine("AirTop").MedicineLongInfo();
            something.Text = p.medicineScheduleInfo.noOfSch.ToString();
        }
    }
}
