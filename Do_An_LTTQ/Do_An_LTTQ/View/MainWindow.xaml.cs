using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Do_An_LTTQ.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Navigate(new UserPage.DashboardPage());
        }
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(null);
            MainContent.Navigate(new UserPage.SettingsPage());
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(null);
            MainContent.Navigate(new UserPage.DashboardPage());
        }
    }
}