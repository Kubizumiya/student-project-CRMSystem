using System.Windows;

namespace CRMSystemZhukov.Views
{
    public partial class DealsWindow : Window
    {
        public DealsWindow()
        {
            InitializeComponent();
        }

        private void OpenTasks_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ViewModels.DealsViewModel;
            if (vm?.SelectedDeal != null)
            {
                var win = new TasksWindow(vm.SelectedDeal.Id);
                win.Owner = this;
                win.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите сделку для просмотра задач.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 