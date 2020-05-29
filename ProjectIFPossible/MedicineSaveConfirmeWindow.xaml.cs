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
    /// Interaction logic for MedicineSaveConfirmeWindow.xaml
    /// </summary>
    public partial class MedicineSaveConfirmationWindow : Window
    {
        //MedicineBatchUpdateControl md;
        MedListControl mdL;
        public string userName;
        private HashSet<MedicineBatchUpdateControl> mds = new HashSet<MedicineBatchUpdateControl>();

        public MedicineSaveConfirmationWindow()
        {
           
            InitializeComponent();          
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(MedicineBatchUpdateControl md in mds){
                mdL = new MedListControl();
                mdL.medName =md.medName;
                mdL.mafName = md.manfName;
                mdL.expDate = md.expDate.ToShortDateString();
                mdL.eDate = md.entryDate;
                mdL.curBatchCount = md.cuurtBatc;
                mdL.userName = userName;
                HolderPanel.Children.Add(mdL);
           }
        }

        public void SetHashSet(HashSet<MedicineBatchUpdateControl> mduc)
        {
            mds = mduc;
        }
    }
}
