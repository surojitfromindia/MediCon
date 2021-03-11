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
    /// Interaction logic for MultipleItemsEntryBox.xaml
    /// </summary>
    public partial class MultipleItemsEntryBox : Window
    {
        public List<string> items = new List<string>(5);
        public List<string> suggestion = new List<string>(5);
        private bool isSuggestionEnable = false;

        public MultipleItemsEntryBox(string title, bool isSuggestionEnable)
        {
            InitializeComponent();
            lbltype.Text = title;
            this.isSuggestionEnable = isSuggestionEnable;
            combSuggestion.Focus();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //Theme.
        void SetTheme(Theme t)
        {
            lbltype.Foreground = t.ContainerBackColor;
        }
        public static readonly DependencyProperty ControlThemeProperty = DependencyProperty.Register("ControlTheme", typeof(Theme), typeof(MultipleItemsEntryBox));
        public Theme ControlTheme
        {
            set { SetValue(ControlThemeProperty, value); SetTheme(value); }
        }



        public void Generate(List<string> pp, List<string> qq)
        {
            PopulateListBox(pp);
            PoplateCombobox(qq);
        }
        void PopulateListBox(List<string> items)
        {
            if (items.Count > 0)
            {
                foreach (string it in items)
                { listBoxForItems.Items.Add(it); this.items.Add(it); }

            }

        }
        void PoplateCombobox(List<string> suggestion)
        {
            if (suggestion != null)
                if (suggestion.Count > 0)
                {
                    combSuggestion.ItemsSource = suggestion;
                }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (combSuggestion.Text != string.Empty)
            {
                var s = combSuggestion.Text;
                listBoxForItems.Items.Add(s);
                items.Add(s);
                combSuggestion.Text = "";
            }
        }

        private void btnRemoveItemFromList_Click(object sender, RoutedEventArgs e)
        {
            int i = listBoxForItems.SelectedIndex;
            if (i > -1)
            {
                listBoxForItems.Items.RemoveAt(i);
                items.RemoveAt(i);
            }

        }
    }
}
