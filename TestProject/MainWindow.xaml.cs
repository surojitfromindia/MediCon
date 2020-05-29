using System.Windows;
using TestProject.NewFolder1;
using System.Collections.Generic;
using System.Windows.Controls;
using System;
using System.Windows.Media;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int kp = 1;
        public MainWindow()
        {
            InitializeComponent();


        }

        private void comb_GotFocus(object sender, RoutedEventArgs e)
        {
            comb.IsDropDownOpen = true;
        }

        private void comb_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            comb.IsDropDownOpen = true;
        }

        private void comb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine(comb.Text);
        }

        private void StackPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            xClose.Visibility = Visibility.Visible;
        }

        private void xClose_Click(object sender, RoutedEventArgs e)
        {
            hold.Children.Remove(child1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PillControl pc = new PillControl();
            pc.Name = $"pc{kp}";
            pc.textValue = txtQ.Text;
            pc.xCloseClick = removeFromControl;
            pc.Margin = new Thickness(3, 0, 0, 0);
            hold.RegisterName(pc.Name, pc);
           hold.Children.Add(pc);
            
            kp++;

        }


        private void removeFromControl(string Name)
        {
            PillControl pc =(PillControl) hold.FindName(Name);
            if(pc!=null)
            {
                hold.UnregisterName(pc.Name);
                hold.Children.Remove(pc);
            }
        }


        private Color BodyColor()
        {
            Random j = new Random();
            byte r =(byte) j.Next(30, 255);
            byte g =(byte) j.Next(30, 255);
            byte b =(byte) j.Next(30, 255);
            Console.WriteLine($"{r}--{g}--{b}");
            return Color.FromRgb(r,g,b);
        }

        private  Color ContraustColor(Color color)
        {
            byte d = 0;
            double lumin = (0.299 * color.R + 0.587 * color.G + 0.114 * color.G) / 255;
            if (lumin > 0.5)
                d = 0;
            else
                d = 255;
            return Color.FromRgb(d, d, d);
        }
    }
}
