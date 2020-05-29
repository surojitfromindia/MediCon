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
    /// Interaction logic for MediListCardControlSmall.xaml
    /// </summary>
    public partial class MediListCardControlSmall : UserControl
    {
        public MediListCardControlSmall()
        {
            InitializeComponent();
        }
      
        private string medName;
        private int stocked;
        private int batch;
        private int batchExp;


        public MediListCardControlSmall(string medName, int stocked, int batch, int batchExp)
        {
            this.medName = medName;
            this.stocked = stocked;
            this.batch = batch;
            this.batchExp = batchExp;
           
            InitializeComponent();
            VisualDataOnControl();
            
        }

        void VisualDataOnControl()
        {
            lblMedicine.Text = medName;
            lblStocked.Text = stocked.ToString();
            lblB.Text = batch.ToString();
            lblBEXp.Text = batchExp.ToString();
        }
    }
}
