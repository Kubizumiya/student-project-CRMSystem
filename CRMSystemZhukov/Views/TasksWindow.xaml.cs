using System.Windows;
using CRMSystemZhukov.ViewModels;

namespace CRMSystemZhukov.Views
{
    public partial class TasksWindow : Window
    {
        public TasksWindow(int dealId)
        {
            InitializeComponent();
            DataContext = new TasksViewModel(dealId);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 