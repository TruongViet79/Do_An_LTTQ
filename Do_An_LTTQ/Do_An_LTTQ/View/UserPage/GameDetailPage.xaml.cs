using Do_An_LTTQ.Models;
using Do_An_LTTQ.Services;
using System.Windows;
using System.Windows.Controls;

namespace Do_An_LTTQ.View.UserPage
{
    public partial class GameDetailPage : Page
    {
        private GameService _gameService = new GameService();

        public GameDetailPage(Game selectedGame)
        {
            InitializeComponent();

            var fullDetailGame = _gameService.GetGameDetails(selectedGame.GameID);

            if (fullDetailGame != null)
            {
                this.DataContext = fullDetailGame;
            }
            else
            {
                this.DataContext = selectedGame;
            }
        }


        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đã thêm vào giỏ hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}