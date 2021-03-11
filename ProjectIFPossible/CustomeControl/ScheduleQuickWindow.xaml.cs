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

namespace ProjectIFPossible.CustomeControl
{
    /// <summary>
    /// Interaction logic for ScheduleQuickWindow.xaml
    /// </summary>
    public partial class ScheduleQuickWindow : Window
    {
        MedicineScheduleInfo ss;
        private string medicineName;
        public ScheduleQuickWindow()
        {
            InitializeComponent();
        }

        public ScheduleQuickWindow(string medname)
        {
            medicineName = medname;
            ss = new MedicineScheduleInfo(medicineName);
            InitializeComponent();
        }

        private void btnBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

       


        void LoadInfos()
        {
            
            lblMedName.Text = medicineName;
            lblOnSchedule.Text = ss.GetScheduleInfo().noOfSch.ToString();
            
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInfos();
        }


        //Add/Update the Schedule
        private void btnScheduleReplace_Click(object sender, RoutedEventArgs e)
        {
            if (txtScheduleInput.Text != string.Empty)
            {
                int o = int.Parse(txtScheduleInput.Text);
                //For Replace Zero is Allowed Because it will remove the Schedule Task
                //if encounter zero.
                if (o > -1)
                {
                    if (o == 0)
                    {
                        ss.Remove();
                        lblOnSchedule.Text = ss.GetScheduleInfo().noOfSch.ToString();
                        txtScheduleInput.Clear();
                    }
                    else
                    {
                        var done = ss.MakeSchedule(o);
                        if (done)
                        {
                            lblOnSchedule.Text = ss.GetScheduleInfo().noOfSch.ToString();
                            txtScheduleInput.Clear();
                        }
                    }

                }
            }
        }
        private void btnScheduleAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtScheduleInput.Text != string.Empty)
            {
                int o = int.Parse(txtScheduleInput.Text);
                if (o > 0)
                {
                    var done = ss.MakeSchedule(o + (ss.GetScheduleInfo().noOfSch));
                    if (done)
                    {
                        lblOnSchedule.Text = ss.GetScheduleInfo().noOfSch.ToString();
                        txtScheduleInput.Clear();
                    }
                }
            }
        }
    }
}
