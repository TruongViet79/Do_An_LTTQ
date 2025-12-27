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

        public GameDetailPage(Game game)
        {
            InitializeComponent();
            _currentGame = game;
            this.DataContext = _currentGame;
        }


        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (_currentGame != null)
            {
                // Thêm vào giỏ hàng
                CartSession.AddToCart(_currentGame);
                MessageBox.Show($"Đã thêm {_currentGame.Title} vào giỏ hàng!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}