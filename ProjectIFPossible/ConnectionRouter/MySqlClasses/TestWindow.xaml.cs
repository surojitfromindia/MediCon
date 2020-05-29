using System;
using System.Windows;

namespace ProjectIFPossible.ConnectionRouter.MySqlClasses
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            MySqlMedicineBatchEntry newBatch = new MySqlMedicineBatchEntry(n1.Text, 12);
            try
            { newBatch.SaveAndCommit(); }
            catch (NonRegisterNameException ex)
            {
                MessageBox.Show("Sorry Seems like a New Medicine Name");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
