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
        public Game FeaturedGame { get; set; }
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

                // Dọn sạch các danh sách cũ để tránh lỗi lặp khi chuyển trang
                ClearCollections();

                var allGamesFromDB = new List<Game>();
                foreach (DataRow row in dt.Rows)
                {
                    allGamesFromDB.Add(new Game
                    {
                        GameID = Convert.ToInt32(row["GameID"]),
                        Title = row["Title"].ToString(),
                        BasePrice = Convert.ToDecimal(row["BasePrice"]),
                        MainCoverImageURL = row["MainCoverImageURL"].ToString(),
                        DeveloperName = row["DeveloperName"].ToString(),
                        Categories = row["Categories"].ToString() // Đã thêm vào Model Game
                    });
                }

                if (allGamesFromDB.Count > 0)
                {
                    // 1. Lấy game mới nhất (dòng đầu tiên) cho Featured Game
                    FeaturedGame = allGamesFromDB[0];

                    // 2. Dùng .Skip(1) để lấy các game còn lại, KHÔNG BAO GỒM game đã làm Featured
                    // Điều này giải quyết triệt để lỗi "Marvel Rivals" hiện 3 lần như trong ảnh
                    var listForGrid = allGamesFromDB.Skip(1).ToList();

                    foreach (var game in listForGrid)
                    {
                        // Giới hạn 6 game cho mục New Releases
                        if (NewReleases.Count < 6) NewReleases.Add(game);

                        // Phân loại vào danh mục (Dựa trên dữ liệu categories trả về)
                        string cats = game.Categories.ToLower();
                        if (cats.Contains("hành d?ng") || cats.Contains("action")) ActionGames.Add(game);
                        if (cats.Contains("phiêu luu") || cats.Contains("adventure")) AdventureGames.Add(game);
                        if (cats.Contains("mô ph?ng") || cats.Contains("casual")) CasualGames.Add(game);
                        if (cats.Contains("chi?n lu?c") || cats.Contains("strategy")) StrategyGames.Add(game);

                        AllGames.Add(game);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi hiển thị Store: " + ex.Message); }
        }
        private void ClearCollections()
        {
            NewReleases.Clear(); TopSellers.Clear(); AllGames.Clear();
            ActionGames.Clear(); AdventureGames.Clear(); CasualGames.Clear(); StrategyGames.Clear();
        }
    }
}
