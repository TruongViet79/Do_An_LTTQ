using Do_An_LTTQ.Helpers;
using Do_An_LTTQ.Models;
using Do_An_LTTQ.Services;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Do_An_LTTQ.View.UserPage
{
    public partial class GameDetailPage : Page
    {
        private Game _currentGame;
        private GameService _gameService = new GameService();

        public GameDetailPage(Game game)
        {
            InitializeComponent();

            try
            {
                if (game == null)
                {
                    MessageBox.Show("Dữ liệu trò chơi không hợp lệ.");
                    // Nếu không có game, có thể quay lại trang trước ngay
                    if (NavigationService != null && NavigationService.CanGoBack)
                        NavigationService.GoBack();
                    return;
                }

                // Lấy thông tin chi tiết từ DB để có đầy đủ Description, SysReq, v.v.
                _currentGame = _gameService.GetGameDetails(game.GameID);

                // Nếu DB lỗi hoặc không tìm thấy, dùng tạm object truyền qua
                if (_currentGame == null)
                {
                    _currentGame = game;
                }

                // Quan trọng: Gán DataContext để XAML Binding hoạt động
                this.DataContext = _currentGame;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị: " + ex.Message);
                _currentGame = game;
                this.DataContext = _currentGame;
            }
        }

        // Sự kiện quay lại trang trước
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        // Sự kiện thêm vào giỏ hàng
        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra user đăng nhập (Giả sử UserID = 0 là chưa đăng nhập hoặc Guest)
            // Bạn có thể tùy chỉnh logic check user ở đây
            if (App.CurrentUserID <= 0)
            {
                MessageBox.Show("Vui lòng đăng nhập để mua hàng!", "Thông báo");
                return;
            }

            // Kiểm tra sở hữu
            int currentUserId = App.CurrentUserID;
            DatabaseManager dbManager = new DatabaseManager();

            string sqlCheck = $"EXEC sp_CheckGameOwnership @UserID = {currentUserId}, @GameID = {_currentGame.GameID}";

            try
            {
                DataTable dt = dbManager.ExecuteQuery(sqlCheck);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int isOwned = Convert.ToInt32(dt.Rows[0]["IsOwned"]);
                    if (isOwned == 1)
                    {
                        MessageBox.Show($"Bạn đã sở hữu trò chơi '{_currentGame.Title}' trong thư viện!", "Đã sở hữu");
                        return;
                    }
                }

                // Thêm vào giỏ
                if (_currentGame != null)
                {
                    CartSession.AddToCart(_currentGame);
                    MessageBox.Show($"Đã thêm {_currentGame.Title} vào giỏ hàng!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra sở hữu: " + ex.Message);
            }
        }

        // Sự kiện mở Website game
        
    }
}