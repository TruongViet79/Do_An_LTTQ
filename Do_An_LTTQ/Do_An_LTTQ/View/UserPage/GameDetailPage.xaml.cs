using Do_An_LTTQ.Helpers;
using Do_An_LTTQ.Models;
using Do_An_LTTQ.Services;
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

            // Lấy lại thông tin đầy đủ từ DB dựa trên ID
            // Vì đối tượng 'game' truyền qua có thể thiếu Description, SysReq...
            _currentGame = _gameService.GetGameDetails(game.GameID);

            // Phòng trường hợp ID sai hoặc ko tìm thấy
            if (_currentGame == null)
            {
                _currentGame = game; // Dùng tạm cái cũ
            }

            this.DataContext = _currentGame;
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
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