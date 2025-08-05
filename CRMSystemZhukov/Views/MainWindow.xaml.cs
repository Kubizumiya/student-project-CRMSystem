using System.Windows;
using CRMSystemZhukov.Views;

namespace CRMSystemZhukov.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Добавим обработчик для перетаскивания окна по кастомной панели
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void OpenMarketing_Click(object sender, RoutedEventArgs e)
        {
            var win = new CampaignsWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void OpenDeals_Click(object sender, RoutedEventArgs e)
        {
            var win = new DealsWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void OpenAnalytics_Click(object sender, RoutedEventArgs e)
        {
            var win = new AnalyticsWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void OpenAutomation_Click(object sender, RoutedEventArgs e)
        {
            var win = new AutomationWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MainWindow_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Перетаскивание окна по кастомной панели
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }
    }
} 