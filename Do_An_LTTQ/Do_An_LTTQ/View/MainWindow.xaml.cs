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

        private void btnStore_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(null);
            MainContent.Navigate(new UserPage.StorePage());
        }

        private void btnLibrary_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Navigate(null);
            MainContent.Navigate(new UserPage.LibraryPage());
        }

        public void UpdateLastPlayedGame(string title, string imagePath)
        {
            // 1. Hiện dòng chữ tiêu đề
            txtStatusLabel.Text = "Last game played:";

            // 2. Hiện tên game ngay bên dưới tiêu đề
            txtGameTitleDisplay.Text = title;

            // 3. Thay đổi text bên trong nút bấm thành "Play Again?"
            txtButtonText.Text = "Play Again?";

            // 4. Cập nhật hình ảnh
            try
            {
                if (!string.IsNullOrEmpty(imagePath))
                {
                    imgLastGame.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                }
            }
            catch
            {
                imgLastGame.ImageSource = new BitmapImage(new Uri("/Resources/idle2.png", UriKind.RelativeOrAbsolute));
            }
        }

    }
}