using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Do_An_LTTQ.View.UserPage
{
    /// <summary>
    /// Interaction logic for LibaryPage.xaml
    /// </summary>
    public partial class LibraryPage : Page
    {
        private readonly DatabaseManager _dbManager = new DatabaseManager();

        public LibraryPage()
        {
            InitializeComponent();
            LoadLibrary();
        }

        private void LoadLibrary()
        {
            try
            {
                // Sử dụng UserID = 1 (tạm thời hardcode cho user 'ellen' của bạn)
                int userId = App.CurrentUserID;

                // Thực thi Stored Procedure sp_GetUserLibrary đã có trong file SQL của bạn
                DataTable dt = _dbManager.ExecuteQuery($"EXEC sp_GetUserLibrary @UserID = {App.CurrentUserID}");

                if (dt != null)
                {
                    // Đổ dữ liệu vào sidebar bên trái
                    icLibrarySidebar.ItemsSource = dt.DefaultView;

                    // Đổ dữ liệu vào danh sách game chính bên phải
                    icLibraryMain.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị thư viện: " + ex.Message);
            }
        }
        private void btnAddGame_Click(object sender, RoutedEventArgs e)
        {
            // Gọi trực tiếp NavigationService mà không cần dùng 'this'
            if (NavigationService != null)
            {
                NavigationService.Navigate(new StorePage());
            }
        }

        private void btnPlayGame_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var gameData = button.DataContext as System.Data.DataRowView;

            if (gameData != null)
            {
                string title = gameData["Title"].ToString();
                string imagePath = gameData["MainCoverImageURL"].ToString();
                int gameId = Convert.ToInt32(gameData["GameID"]);

                // 1. Cập nhật SQL: Lưu thời gian chơi cuối (để đưa lên đầu danh sách)
                string sqlUpdate = $"UPDATE USERLIBRARIES SET LastPlayed = GETDATE() WHERE UserID = {App.CurrentUserID} AND GameID = {gameId}";
                _dbManager.ExecuteQuery(sqlUpdate);

                // 2. Gọi hàm cập nhật giao diện ở MainWindow
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.UpdateLastPlayedGame(title, imagePath);
                }

                // 3. Load lại thư viện để game vừa chơi nhảy lên đầu
                LoadLibrary();

                MessageBox.Show($"Starting {title}...");
            }
        }
    }
}
