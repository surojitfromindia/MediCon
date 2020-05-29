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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectIFPossible
{
    /// <summary>
    /// Interaction logic for MedListControl.xaml
    /// </summary>
    public partial class MedListControl : UserControl
    {
        public string medName { get; set; }
        public string mafName { get; set; }
        public string eDate { get; set; }
        public string expDate { get; set; }
        public int wasInSotrage { get; set; }
        public int curBatchCount { get; set; }
        public int willInStorage { get; set; }
        public int newEntry { get; set; }
        public string userName { get; set; }

        public MedListControl()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
