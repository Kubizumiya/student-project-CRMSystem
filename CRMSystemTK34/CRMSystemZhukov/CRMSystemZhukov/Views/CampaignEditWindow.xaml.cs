using System.Windows;
using CRMSystemZhukov.Models;

namespace CRMSystemZhukov.Views
{
    public partial class CampaignEditWindow : Window
    {
        public CampaignEditWindow(Campaign campaign)
        {
            InitializeComponent();
            DataContext = campaign;
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