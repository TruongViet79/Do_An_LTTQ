using Do_An_LTTQ.Models;
using Do_An_LTTQ.Services;
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
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        private readonly DatabaseManager _dbManager = new DatabaseManager();

        public DashboardPage()
        {
            InitializeComponent();
            txtDisplayUsername.Text = App.CurrentUsername;
            LoadData();
        }

        private void FriendList_MouseEnter(object sender, MouseEventArgs e)
        {
            FriendList.Width = 200;
        }

        private void FriendList_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!FriendList.IsMouseOver) { FriendList.Width = 100; }
        }

        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            var frame = System.Windows.Navigation.NavigationService.GetNavigationService(this);
            frame?.Navigate(new SettingsPage());

        }
        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            // Điều hướng sang trang CartPage
            NavigationService.Navigate(new CartPage());
        }

        private void LoadData()
        {
            try
            {
                // 1. Lấy dữ liệu từ Database
                DataTable dtGames = _dbManager.GetDashboardGames();

                // 2. LỌC TRÙNG THEO TIÊU ĐỀ (TITLE)
                // Vì database của bạn có các game trùng tên nhưng khác ID (VD: CS2 ở ID 7, 27, 35)
                // Ta dùng Dictionary với Key là Title để ép mỗi tên game chỉ hiện 1 lần.
                var uniqueGamesByTitle = new Dictionary<string, Game>();

                foreach (DataRow row in dtGames.Rows)
                {
                    string title = row["Title"].ToString();

                    if (!uniqueGamesByTitle.ContainsKey(title))
                    {
                        uniqueGamesByTitle.Add(title, new Game
                        {
                            GameID = Convert.ToInt32(row["GameID"]),
                            Title = title,
                            FinalPrice = row["FinalPrice"] != DBNull.Value ? Convert.ToDecimal(row["FinalPrice"]) : 0,
                            MainCoverImageURL = row["MainCoverImageURL"].ToString(),
                        });
                    }
                }

                // Chuyển Dictionary thành List sạch
                var cleanList = uniqueGamesByTitle.Values.ToList();

                // 3. Phân loại và đổ vào ItemsControl (ItemsSource nhận List<Game>)
                icFreeGames.ItemsSource = cleanList.Where(g => g.FinalPrice == 0).ToList();
                icPaidGames.ItemsSource = cleanList.Where(g => g.FinalPrice > 0).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách game: " + ex.Message);
            }
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            // Vì ItemsSource của đại ca là DataView, nên Item là DataRowView
            if (btn.DataContext is DataRowView row)
            {
                // Chuyển DataRow thành Object Game
                Game selectedGame = new Game
                {
                    GameID = (int)row["GameID"],
                    Title = row["Title"].ToString(),
                    FinalPrice = row["FinalPrice"] != DBNull.Value ? Convert.ToDecimal(row["FinalPrice"]) : 0,
                    MainCoverImageURL = row["MainCoverImageURL"].ToString(),

                };

                // Chuyển trang
                NavigationService.Navigate(new GameDetailPage(selectedGame));
            }
        }
    }
}
