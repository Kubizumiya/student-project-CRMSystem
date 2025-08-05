using System.Windows;

namespace CRMSystemZhukov.Views
{
    public partial class AutomationWindow : Window
    {
        public AutomationWindow()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 