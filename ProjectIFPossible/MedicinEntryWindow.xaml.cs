using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using ProjectIFPossible.ConnectionRouter;
using ProjectIFPossible.ConnectionRouter.MySqlClasses;
namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for MedicinEntryWindow.xaml
    /// </summary>
    public partial class MedicinEntryWindow : Window
    {
        public delegate void updateMedicineIntemCount();
        public updateMedicineIntemCount onCloserUpdate;

        private int confirmentionPassWord;
        List<MySqlMedicineEntry> MedicineList;
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
            MySqlMedicineEntry msme = new MySqlMedicineEntry(txtMedName.Text, txtMedMafName.Text, txtDate.Text);
            MedcineEntryConfirmPopUpWindow widC = new MedcineEntryConfirmPopUpWindow
            {
                Owner = this,
                passwordForConfirm = confirmentionPassWord.ToString()
            };
            if(msme.validateManufactName())
            {
                widC.ShowDialog();
                if (widC.isPasswordConfimed == true)
                {
                    msme.SaveAndCommit();
                    GenateConfirmationPassWord();
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
    }

}
