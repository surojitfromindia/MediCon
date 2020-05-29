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
    /// Interaction logic for MediListCardControl.xaml
    /// </summary>
    public partial class MediListCardControl : UserControl
    {
        Dictionary<string, Color> tags = new Dictionary<string, Color>(10);
        private string medName;
        private int stocked;
        private int batch;
        private int batchExp;


        public MediListCardControl(string medName, int stocked, int batch, int batchExp , Dictionary<string, Color> tagsList)
        {
            this.medName = medName;
            this.stocked = stocked;
            this.batch = batch;
            this.batchExp = batchExp;
            tags = tagsList;
            InitializeComponent();
            VisualDataOnControl();
            LoadTagsOnTagPanel();
        }

        void VisualDataOnControl()
        {
            lblMedicine.Text = medName;
            lblStocked.Text = stocked.ToString();
            lblB.Text = batch.ToString();
            lblBEXp.Text = batchExp.ToString();
        }


        void LoadTagsOnTagPanel()
        {
            PillControlClass.Chips chips;   
            foreach (var tag in tags.Keys)
            {
                chips = new PillControlClass.Chips(tags[tag], tag);
                chips.Name = tag;
                tagPanel.RegisterName(tag, chips);
                tagPanel.Children.Add(chips);
            }
            if(tagPanel.Children.Count==1)
            {
                TextBlock tb = new TextBlock();
                tb.Text = "    No tags found! :-/ , Edit Medicine Entry and Add tags!";
                tb.Foreground =new SolidColorBrush( Colors.Gray);
                tagPanel.Children.Add(tb);
            }
        }

       

        

      
        

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            selectInd.Fill = new SolidColorBrush(Color.FromRgb(231,249,86));
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            selectInd.Fill = new SolidColorBrush(Color.FromRgb(68, 65, 65));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
