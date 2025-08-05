using System.Windows;
using CRMSystemZhukov.Models;

namespace CRMSystemZhukov.Views
{
    public partial class TaskEditWindow : Window
    {
        public TaskEditWindow(Task task)
        {
            InitializeComponent();
            DataContext = task;
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