using Do_An_LTTQ.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Data.SqlClient;

namespace Do_An_LTTQ.View.UserPage
{
    /// <summary>
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class StorePage : Page
    {
        // 1. Các danh sách cho mục chính
        public ObservableCollection<Game> NewReleases { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<Game> TopSellers { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<Game> OnSales { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<Game> AllGames { get; set; } = new ObservableCollection<Game>();

        // 2. Các danh sách cho mục Category
        public ObservableCollection<Game> ActionGames { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<Game> AdventureGames { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<Game> CasualGames { get; set; } = new ObservableCollection<Game>();
        public ObservableCollection<Game> StrategyGames { get; set; } = new ObservableCollection<Game>();
        private Game _featuredGame;
        public Game FeaturedGame
        {
            get => _featuredGame;
            set { _featuredGame = value; }
        }
        DatabaseManager db = new DatabaseManager(); // Khởi tạo đối tượng quản lý DB
        private readonly DatabaseManager _dbManager = new DatabaseManager();
        public StorePage()
        {
            InitializeComponent();
            LoadStoreData();
            this.DataContext = this;
        }

        private void LoadStoreData()
        {
            try
            {
                int currentUserId = 1;
                DataTable dt = _dbManager.ExecuteQuery($"EXEC sp_GetAllGamesForDisplay @UserID = 1");
                ClearCollections();

                // Bước 1: Dùng Dictionary để "ép" dữ liệu SẠCH ngay từ DataTable
                // Key là GameID (int), Value là đối tượng Game. 
                // Nếu ID đã tồn tại, nó sẽ không được thêm vào nữa.
                var uniqueGamesMap = new Dictionary<int, Game>();

                foreach (DataRow row in dt.Rows)
                {
                    int gID = Convert.ToInt32(row["GameID"]);
                    if (!uniqueGamesMap.ContainsKey(gID))
                    {
                        uniqueGamesMap.Add(gID, new Game
                        {
                            GameID = gID,
                            Title = row["Title"].ToString(),
                            BasePrice = Convert.ToDecimal(row["BasePrice"]),
                            MainCoverImageURL = row["MainCoverImageURL"].ToString(),
                            DeveloperName = row["DeveloperName"]?.ToString() ?? "Unknown",
                            Categories = row["Categories"]?.ToString() ?? ""
                        });
                    }
                }

                // Chuyển sang danh sách sạch để xử lý hiển thị
                var cleanGamesList = uniqueGamesMap.Values.ToList();

                if (cleanGamesList.Count > 0)
                {
                    // 1. Featured Game: Lấy game đầu tiên từ danh sách sạch
                    FeaturedGame = cleanGamesList[0];

                    foreach (var game in cleanGamesList)
                    {
                        // 2. All Games: Nạp 100% danh sách sạch (Chắc chắn không lặp)
                        AllGames.Add(game);

                        // 3. New Releases: Loại bỏ game đã hiện ở ô to Featured
                        if (game.GameID != FeaturedGame.GameID && NewReleases.Count < 6)
                        {
                            NewReleases.Add(game);
                        }

                        // 4. Các mục đặc biệt khác
                        if (game.BasePrice > 500000) TopSellers.Add(game);
                        if (game.BasePrice < 200000) OnSales.Add(game);

                        // 5. Phân loại Category (Dùng dữ liệu từ SQL)
                        string cats = game.Categories.ToLower();
                        if (cats.Contains("action") || cats.Contains("hành d?ng")) ActionGames.Add(game);
                        if (cats.Contains("adventure") || cats.Contains("phiêu luu")) AdventureGames.Add(game);
                        if (cats.Contains("casual") || cats.Contains("mô ph?ng")) CasualGames.Add(game);
                        if (cats.Contains("strategy") || cats.Contains("chi?n lu?c")) StrategyGames.Add(game);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }
        private void ClearCollections()
        {
            NewReleases.Clear(); TopSellers.Clear(); AllGames.Clear();
            ActionGames.Clear(); AdventureGames.Clear(); CasualGames.Clear(); StrategyGames.Clear();
        }

        private void GameBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Game selectedGame)
            {
                NavigationService.Navigate(new GameDetailPage(selectedGame));
            }
        }

        private void BtnBuyNow_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem có game nổi bật nào đang hiện không
            if (FeaturedGame != null)
            {
                // Chuyển sang trang Detail và mang theo thông tin game đó
                NavigationService.Navigate(new GameDetailPage(FeaturedGame));
            }
        }

        // 1. Thêm Constructor nhận tham số category
        public StorePage(string categoryToSelect) : this() // Gọi constructor gốc để LoadStoreData trước
        {
            // Sau khi load xong dữ liệu thì chọn tab
            SelectCategoryTab(categoryToSelect);
        }

        // 2. Hàm xử lý chọn Tab dựa trên tên
        private void SelectCategoryTab(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName)) return;

            // Chuyển về chữ thường để so sánh cho dễ (vì DB có thể là "Action", "action"...)
            string cat = categoryName.ToLower().Trim();

            // Dựa vào thứ tự Tab trong file XAML của bạn:
            // Tab 0: Featured
            // Tab 1: Top Sellers
            // Tab 2: On Sales
            // Tab 3: All Games
            // Tab 4: (Gạch ngang - Separator)
            // Tab 5: Action
            // Tab 6: Adventure
            // Tab 7: Casual
            // Tab 8: Strategy

            int tabIndex = -1;

            if (cat.Contains("action") || cat.Contains("hành động")) tabIndex = 6;
            else if (cat.Contains("adventure") || cat.Contains("phiêu lưu")) tabIndex = 7;
            else if (cat.Contains("casual") || cat.Contains("mô phỏng")) tabIndex = 8;
            else if (cat.Contains("strategy") || cat.Contains("chiến lược")) tabIndex = 9;

            // Nếu tìm thấy Tab phù hợp thì chuyển sang
            if (tabIndex != -1 && MainStoreTabControl != null)
            {
                MainStoreTabControl.SelectedIndex = tabIndex;
            }
            else
            {
                // Nếu không có tab riêng (VD: RPG), ta có thể chuyển về tab "All Games" (Tab 3)
                // Hoặc xử lý lọc nâng cao sau này. Tạm thời về All Games.
                MainStoreTabControl.SelectedIndex = 3;
            }
        }
    }
}
