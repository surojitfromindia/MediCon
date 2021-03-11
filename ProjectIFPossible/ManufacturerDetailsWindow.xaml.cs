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
    public partial class ManufacturerDetailsWindow : Window
    {
        private string manfname;
        private Theme currentTheme;

        public ManufacturerDetailsWindow(string maufname)
        {
            manfname = maufname;
            InitializeComponent();
        }
      

        void FillContactAndEmailList()
        {
            contactListBox.ItemsSource = ManufacturerInfo.GetAllContactOf(manfname);
            emailListBox.ItemsSource = ManufacturerInfo.GetAllEmailOf(manfname);
        }
        void LoadShortInfo()
        {
            ManufacturerInfo e = ManufacturerInfo.GetShortInfoOf(manfname);
            lblCompayName.Text = e.manufacturName;
            lblCompanyContact.Text = e.contact;
            lblCompanyLoc.Text = e.address;
            lblCompanyWebsite.Text = e.website;
            lblCompayEmail.Text = e.email;
            
        }
        void LoadMedicineNames() {
            List<string> medicinenames = ManufacturerInfo.GetMedicineNameOf(manfname);
            foreach(string medicine in medicinenames)
            {
                MediListCardLongStrip nameStrip = new MediListCardLongStrip(medicine);
                nameStrip.ControlTheme = currentTheme;
                holderpanel.Children.Add(nameStrip);
            }

        }

       

        private void contactListBox_Loaded(object sender, RoutedEventArgs e)
        {
            LoadShortInfo();
            FillContactAndEmailList();
            LoadMedicineNames();
        }


        //Theme.
        void SetTheme(Theme t)
        {
            currentTheme = t;
            refBack.Background = t.ContainerBackColor;
            shotInfoBack.Background = t.InfoBackColor;
            contactListBox.Background = t.InfoBackColor;
            emailListBox.Background = t.InfoBackColor;
        }
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(ManufacturerDetailsWindow));
        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }

    }
}
