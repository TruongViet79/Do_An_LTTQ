using Do_An_LTTQ.Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class StorePage : Page
    {
        public StorePage()
        {
            InitializeComponent();
        }

        private void GameBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Lấy đối tượng Border đã được click
            if (sender is Border clickedBorder)
            {
                // Lấy ID từ thuộc tính Tag mà ta đã gán ở XAML
                if (int.TryParse(clickedBorder.Tag.ToString(), out int gameId))
                {
                    // CÁCH 1: Nếu bạn đã có hàm GetGameById trong Service
                    // Game selectedGame = _gameService.GetGameById(gameId);

                    // CÁCH 2: (Tạm thời) Tạo đối tượng Game giả lập nếu chưa có DB hoàn chỉnh
                    // Vì GameDetailPage nhận vào một đối tượng Game
                    Game selectedGame = new Game()
                    {
                        GameID = gameId,
                        // Các thông tin khác có thể để Service bên trang Detail tự load lại 
                        // dựa trên ID, như logic bạn đã viết bên GameDetailPage.xaml.cs
                    };

                    // Thực hiện chuyển trang và truyền object game qua
                    NavigationService.Navigate(new GameDetailPage(selectedGame));
                }
            }
        }
    }
}
