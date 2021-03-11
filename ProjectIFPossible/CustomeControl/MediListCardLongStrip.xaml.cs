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

namespace ProjectIFPossible.CustomeControl
{
    
    public partial class MediListCardLongStrip : UserControl
    {
        
        private string lblHeader;
        private Theme currentTheme;

        public MediListCardLongStrip(string lblHeader)
        {
            InitializeComponent();
            this.lblHeader = lblHeader;
            txtHeader.Text = lblHeader;
        }
        private void btnDelAdmin_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            //detailButtonHandler?.Invoke(this, e);
            MedicineDetailsInfoWindow mdiw  = new MedicineDetailsInfoWindow(lblHeader);
            mdiw.ControlTheme = currentTheme;
            mdiw.ShowDialog();
            
        }



        //Theme
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(MediListCardLongStrip));
        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }
        private void SetTheme(Theme value)
        {
            currentTheme = value;
            refBack.Background = value.InfoBackColor;
        }
    }
}
