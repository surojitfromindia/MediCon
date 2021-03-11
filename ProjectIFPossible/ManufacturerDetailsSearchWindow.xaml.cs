using ProjectIFPossible.ConnectionRouter.MySqlClasses.Models;
using ProjectIFPossible.CustomeControl;
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
    /// Interaction logic for ManufacturerDetailsWindow.xaml
    /// </summary>
    public partial class ManufacturerDetailsSearchWindow : Window
    {
        Theme currentTheme;
        public ManufacturerDetailsSearchWindow()
        {
            InitializeComponent();
        }

        private void btnSerach_Click(object sender, RoutedEventArgs e)
        {
            Search(txtSearch.Text);
        }

        void Search(string searchingvalue)
        {
            int len = searchingvalue.Length;
            List<string> temp = new List<string>(20);
            foreach (string itm in listBxCompanies.Items)
            {
                string trs = itm.ToLower();
                if (trs.Length >= searchingvalue.Length)
                {
                    if (trs.Substring(0, len) == searchingvalue)
                    {
                        temp.Add(itm);
                       
                    }
                }
            }
            if (temp.Count > 0)
            { 
                int idex = listBxCompanies.Items.IndexOf(temp[0]);
                listBxCompanies.SelectedIndex = idex;
            }
            
        }
        void Clean()
        {
            listBxCompanies.ItemsSource = ManufacturerInfo.GetAllManufacturerName();
        }
        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            Clean();
        }


        //Theme
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(ManufacturerDetailsSearchWindow));
        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }
        private void SetTheme(Theme value)
        {
            currentTheme = value;
            refWindow.Background = value.ContainerBackColor;
            listBxCompanies.Background = value.InfoBackColor;

        }

        //ListBox populate
        private void refWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listBxCompanies.ItemsSource = ManufacturerInfo.GetAllManufacturerName();
        }

        private void listBxCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            conP.Content = null;
            string value = (string)listBxCompanies.SelectedValue;
            ManufacDetailsCard mfdw = new ManufacDetailsCard(value);
            mfdw.ControlTheme = currentTheme;
            conP.Content = mfdw;
        }
    }
}
