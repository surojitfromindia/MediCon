using ProjectIFPossible.ConnectionRouter.MySqlClasses.Models;
using ProjectIFPossible.MiniWindows;
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
    /// Interaction logic for ManfuacturerRegistraionWindow.xaml
    /// </summary>
    public partial class ManfuacturerRegistraionWindow : Window
    {

        //List Of Contact from A Mini MultiWindow
        List<string> contactsfrommini = new List<string>(3);
        List<string> emailsfrommini = new List<string>(3);


        Theme currentTheme;
        public ManfuacturerRegistraionWindow()
        {
            
            InitializeComponent();
        }
        public ManfuacturerRegistraionWindow(string  companyName)
        {

            InitializeComponent();
            txtName.Text = companyName;
        }
        
        private void txtContact_GotFocus(object sender, RoutedEventArgs e)
        {
           // txtEmail.Focus();
            MultipleItemsEntryBox hh = new MultipleItemsEntryBox("Add Contacts", false)
            {
                ControlTheme = currentTheme
            };
            hh.Generate(contactsfrommini,new List<string>(0));
            hh.ShowDialog();
            contactsfrommini = hh.items;
            if (contactsfrommini.Count == 1)
                txtContact.Text = contactsfrommini[0];
            else if (contactsfrommini.Count > 1)
                txtContact.Text = "--List--";
            else
                txtContact.Text = "";
        }
        private void txtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            txtWebsite.Focus();
            MultipleItemsEntryBox hh = new MultipleItemsEntryBox("Add Emails", false)
            {
                ControlTheme = currentTheme
            };
            hh.Generate(emailsfrommini, new List<string>(0));
            hh.ShowDialog();
            emailsfrommini = hh.items;
            if (emailsfrommini.Count == 1)
                txtEmail.Text = emailsfrommini[0];
            else if (emailsfrommini.Count > 1)
                txtEmail.Text = "--List--";
            else
                txtEmail.Text = "";


        }



        private void btnSaveAndNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               RegisterCompany(txtName.Text);

            }
            catch (Exception) { }
        }
        void RegisterCompany(string manfName)
        {
            if (CheckAvailabilty())
            {
                ManufacturerInfo manufacturer 
                    = new ManufacturerInfo(manfName, txtAdd.Text, contactsfrommini[0], emailsfrommini[0], txtWebsite.Text);
                bool registerSuccessfull = manufacturer.Register();
                if (registerSuccessfull)
                {
                   bool  suc = manufacturer.RecordContactsAndEmails(contactsfrommini,emailsfrommini);
                    if(suc)
                    {
                        MessageBox.Show("Saved");
                        CleanField();
                        listBxCompanies.Items.Add(manfName);
                    }
                }

            }
        }       
        bool CheckAvailabilty()
        {
            bool manufacturerIsNotEmpty = txtName.Text.Length==0 ? false : true;
            bool contactsIsNotEmpty = contactsfrommini.Count==0? false : true;
            bool emailsIsNotEmpty = emailsfrommini.Count==0 ? false : true ;
            bool websiteIsNotEmpty = txtWebsite.Text.Length==0 ? false : true;
            bool addressIsNotEmpty = txtAdd.Text.Length==0 ? false : true;
            return manufacturerIsNotEmpty && contactsIsNotEmpty && emailsIsNotEmpty && websiteIsNotEmpty && addressIsNotEmpty;

        }
        void CleanField()
        {
            txtName.Clear();
            txtAdd.Clear();
            txtContact.Clear();
            txtEmail.Clear();
            txtWebsite.Clear();
            contactsfrommini.Clear();
            emailsfrommini.Clear();

        }



        //Theme
        void SetTheme(Theme t)
        {
            refWindow.Background = t.MainBodyBackColor;
            btnSaveAndNext.Background = t.ContainerBackColor;
            currentTheme = t;
        }
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(ManfuacturerRegistraionWindow));
        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }

        private void refWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listBxCompanies.ItemsSource = ManufacturerInfo.GetAllManufacturerName();
        }
    }
}
