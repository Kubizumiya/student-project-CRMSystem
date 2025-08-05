using System.Windows;
using CRMSystemZhukov.Models;

namespace CRMSystemZhukov.Views
{
    public partial class DealEditWindow : Window
    {
        public DealEditWindow(Deal deal)
        {
            InitializeComponent();
            DataContext = deal;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 