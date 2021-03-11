using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using ProjectIFPossible.ConnectionRouter;
using ProjectIFPossible.ConnectionRouter.MySqlClasses;
namespace ProjectIFPossible
{
    
    public partial class MedicinEntryWindow : Window
    {
        public delegate void updateMedicineIntemCount();
        public updateMedicineIntemCount onCloserUpdate;
        private int confirmentionPassWord;
        List<MySqlMedicineEntry> MedicineList;
        Theme currtTheme;

        public MedicinEntryWindow()
        {
          
            MedicineList = MySqlMedicineEntry.GetMedicineList();
            InitializeComponent();          
            DataContext = this;
            medList.ItemsSource = MedicineList;
            GenateConfirmationPassWord();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MySqlMedicineEntry msme = new MySqlMedicineEntry(txtMedName.Text, txtMedMafName.Text, txtDate.Text,int.Parse(txtPrice.Text));
            MedcineEntryConfirmPopUpWindow widC = new MedcineEntryConfirmPopUpWindow
            {
                Owner = this,
                //Password is going into Password Confirme Window
                passwordForConfirm = confirmentionPassWord.ToString()
            };
            //If Entry Is Valid Then
            if(msme.validateManufactName())
            {
                //Show the Password Confiremation Field
                //If the Password is Correct 
                widC.ShowDialog();
                if (widC.isPasswordConfimed == true)
                {
                    //Complete The transaction
                    msme.SaveAndCommit();
                    //Generate another password for next entry.
                    GenateConfirmationPassWord();
                    //Update the items of ListBox
                    MedicineList = MySqlMedicineEntry.GetMedicineList();
                    medList.ItemsSource = MedicineList;
                    medList.SelectedIndex = medList.Items.Count;
                }
            }            
            
        }

        private void GenateConfirmationPassWord()
        {
            Random j = new Random();
            int rand = j.Next(1000, 9999);
            confirmentionPassWord = rand;
            tbPassWord.Text = confirmentionPassWord.ToString(); 

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            onCloserUpdate();
        }

        private void btnNewManufacEntry_Click(object sender, RoutedEventArgs e)
        {
            //get the manufacturer name from textfield if not empty
            string manfname = txtMedMafName.Text.Length == 0 ? "" : txtMedMafName.Text ;
            ManfuacturerRegistraionWindow MFRW = new ManfuacturerRegistraionWindow(manfname);
            MFRW.ControlTheme = currtTheme; 
            MFRW.ShowDialog();
        }


        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(MedicinEntryWindow));

        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }

        private void SetTheme(Theme value)
        {
            currtTheme = value;
            refBack.Background = value.MainBodyBackColor;
            btnReg.Background = value.ContainerBackColor;
        }
    }

}
