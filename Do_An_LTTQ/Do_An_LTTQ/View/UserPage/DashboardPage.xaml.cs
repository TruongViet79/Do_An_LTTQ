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

        private void LoadData()
        {
            try
            {
                // 1. Lấy toàn bộ game từ DatabaseManager
                DataTable dtGames = _dbManager.GetDashboardGames(); 

                // 2. Lọc danh sách Game Miễn Phí (Giá = 0)
                DataView freeView = new DataView(dtGames); 
                freeView.RowFilter = "FinalPrice = 0"; 
                icFreeGames.ItemsSource = freeView; // Đổ vào ItemsControl Free 

                // 3. Lọc danh sách Game Trả Phí (Giá > 0)
                DataView paidView = new DataView(dtGames); 
                paidView.RowFilter = "FinalPrice > 0"; 
                icPaidGames.ItemsSource = paidView; // Đổ vào ItemsControl Paid 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hiển thị danh sách game: " + ex.Message); 
                }   
        }
    }
}
