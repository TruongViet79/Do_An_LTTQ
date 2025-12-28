using Do_An_LTTQ.Helpers;
using Do_An_LTTQ.Models;
using Do_An_LTTQ.Services;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Do_An_LTTQ.View.UserPage
{
    public partial class GameDetailPage : Page
    {
        private Game _currentGame;
        private GameService _gameService = new GameService(); // Gọi service

        public GameDetailPage(Game game)
        {
            InitializeComponent();

            try
            {
                // 1. Kiểm tra nếu đối tượng game truyền vào bị null
                if (game == null)
                {
                    MessageBox.Show("Dữ liệu trò chơi không hợp lệ.");
                    return;
                }

                // 2. Gọi Service để lấy thông tin chi tiết (Description, SysReq...) từ DB
                // Đây là nơi dễ gây lỗi nhất nếu kết nối database có vấn đề
                _currentGame = _gameService.GetGameDetails(game.GameID);

                // 3. Nếu không tìm thấy trong DB, sử dụng dữ liệu tạm thời được truyền qua
                if (_currentGame == null)
                {
                    _currentGame = game;
                }
            }
            catch (Exception ex)
            {
                // 4. Bẫy lỗi: Nếu có lỗi SQL hoặc lỗi hệ thống, app sẽ hiện thông báo thay vì bị văng
                MessageBox.Show("Lỗi khi tải chi tiết game: " + ex.Message);
                _currentGame = game;
            }

            // Gán dữ liệu cho giao diện
            this.DataContext = _currentGame;
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            // 1. Lấy thông tin game từ nút bấm
            var button = sender as Button;
            var game = button.DataContext as Game;

            if (game == null) return;

            // 2. Kiểm tra xem user đã sở hữu game này chưa
            int currentUserId = App.CurrentUserID;
            DatabaseManager dbManager = new DatabaseManager();

            // Gọi procedure kiểm tra
            string sqlCheck = $"EXEC sp_CheckGameOwnership @UserID = {currentUserId}, @GameID = {game.GameID}";
            DataTable dt = dbManager.ExecuteQuery(sqlCheck);

            if (dt != null && dt.Rows.Count > 0)
            {
                int isOwned = Convert.ToInt32(dt.Rows[0]["IsOwned"]);

                if (isOwned == 1)
                {
                    // NẾU ĐÃ MUA: Hiện thông báo và dừng lại luôn, không cho thêm vào giỏ
                    MessageBox.Show($"Bạn đã sở hữu trò chơi '{game.Title}' trong Thư viện rồi!", "Thông báo sở hữu");
                    return;
                }
            }
            if (_currentGame != null)
            {
                CartSession.AddToCart(_currentGame);
                MessageBox.Show($"Đã thêm {_currentGame.Title} vào giỏ hàng!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Content is string categoryName)
            {
                // Điều hướng sang StorePage và gửi kèm tên thể loại (VD: "Action")
                NavigationService.Navigate(new StorePage(categoryName));
            }
        }
    }
}