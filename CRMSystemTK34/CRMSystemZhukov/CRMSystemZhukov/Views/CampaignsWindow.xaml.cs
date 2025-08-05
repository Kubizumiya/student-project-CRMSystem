using System.Windows;

namespace CRMSystemZhukov.Views
{
    public partial class CampaignsWindow : Window
    {
        public CampaignsWindow()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 