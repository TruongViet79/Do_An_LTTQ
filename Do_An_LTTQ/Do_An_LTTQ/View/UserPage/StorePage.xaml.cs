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
                DataTable dt = db.ExecuteQuery("EXEC sp_GetAllGamesForDisplay");
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
