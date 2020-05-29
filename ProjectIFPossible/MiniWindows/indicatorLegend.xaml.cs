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

namespace ProjectIFPossible.MiniWindows
{
    /// <summary>
    /// Interaction logic for indicatorLegend.xaml
    /// </summary>
    public partial class indicatorLegend : UserControl
    {
        public SolidColorBrush FillBrush { get; set; } = new SolidColorBrush(Colors.Coral);
        public string text { get; set; } = "Default Text";
        public SolidColorBrush foreColor { get; set; } = new SolidColorBrush(Colors.White); 
        public indicatorLegend()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
