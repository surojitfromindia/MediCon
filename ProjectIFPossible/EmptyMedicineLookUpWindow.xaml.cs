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
    /// Interaction logic for EmptyMedicineLookUpWindow.xaml
    /// </summary>
    public partial class EmptyMedicineLookUpWindow : Window
    {
        EmptyStockMedicine emptyStock;
        List<Medicine> medicines;
        public EmptyMedicineLookUpWindow()
        {
            InitializeComponent();
            emptyStock = new EmptyStockMedicine();
            medicines = emptyStock.getEmptyStockedMedicineNames;
            LoadCards();
        }


        void LoadCards()
        {
            foreach(Medicine e in medicines)
            {
                MediListCardControlSmall ctSmall = new MediListCardControlSmall(e.name, 0, 0, 0, 0);
                ctSmall.Schedule = e.medicineScheduleInfo.noOfSch;
                holderPanel2.Children.Add(ctSmall);
            }
        }


        private void btnCPu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.Purple);
        }

        private void btnCGr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.Green);
        }

        private void btnCRe_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.Red);
        }

        private void btnCDr_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.dark);
        }

        private void btnCNi_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cc(Themes.night);
        }

        private void btnCBlue_MouseDown(object sender, MouseButtonEventArgs e)
        {

            cc(Themes.blue);
           
        }

        //temp
        void cc(Theme t)
        {

            
            holderPanel.Background = t.MainBodyBackColor;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadSearchResult();
        }

        void LoadSearchResult()
        {
            if (txtSearchBox.Text != string.Empty)
            {
               
                txtSearchBox.Focus();
            }
        }
    }
}
