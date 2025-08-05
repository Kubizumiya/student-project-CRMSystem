using System.Windows;
using CRMSystemZhukov.Models;

namespace CRMSystemZhukov.Views
{
    public partial class ClientEditWindow : Window
    {
        public ClientEditWindow(Client client)
        {
            InitializeComponent();
            DataContext = client;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var client = DataContext as Client;
            if (string.IsNullOrWhiteSpace(client.Name))
            {
                MessageBox.Show("Поле 'ФИО' обязательно для заполнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(client.Email))
            {
                MessageBox.Show("Поле 'Email' обязательно для заполнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 