using Do_An_LTTQ.Helpers;
using Do_An_LTTQ.Models;
using Do_An_LTTQ.Services;
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
using System.Globalization;

namespace Do_An_LTTQ.View.UserPage
{
    /// <summary>
    /// Interaction logic for CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            LoadCart();
        }

        private void LoadCart()
        {
            // Lấy danh sách từ CartSession
            lvCart.ItemsSource = null; // Reset để UI cập nhật
            lvCart.ItemsSource = CartSession.CartItems;

            // Cập nhật tổng tiền
            decimal total = CartSession.GetTotalAmount();
            txtTotalAmount.Text = string.Format("{0:N0} VNĐ", total);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            // Lấy game từ nút được nhấn
            var button = sender as Button;
            var gameToRemove = button.DataContext as Game;

            if (gameToRemove != null)
            {
                CartSession.RemoveFromCart(gameToRemove);
                LoadCart(); // Tải lại giao diện
            }
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            // 1. Kiểm tra giỏ hàng
            if (CartSession.CartItems == null || CartSession.CartItems.Count == 0)
            {
                MessageBox.Show("Giỏ hàng của bạn đang trống!", "Thông báo");
                return;
            }

            OrderService orderService = new OrderService();
            DatabaseManager dbManager = new DatabaseManager();
            int userId = App.CurrentUserID;

            try
            {
                // 2. Tạo đơn hàng duy nhất và lấy ID
                int orderId = orderService.CreateOrder(userId, "CreditCard");

                // 3. Sử dụng vòng lặp ToList() để tránh lỗi sửa đổi danh sách khi đang duyệt
                var itemsToProcess = CartSession.CartItems.ToList();

                foreach (var game in itemsToProcess)
                {
                    // 1. Thêm vào chi tiết đơn hàng
                    orderService.AddGameToOrder(orderId, game.GameID);

                    // 2. GIẢI PHÁP MỚI: Truyền giá tiền dưới dạng chuỗi và để SQL tự xử lý dấu phẩy
                    // Cách này giúp tránh hoàn toàn việc SQL hiểu nhầm dấu phẩy là dấu ngăn cách cột
                    string rawPrice = game.FinalPrice.ToString(); // Lấy giá trị mặc định (có thể là 150000,00)

                    string sqlInsertLibrary = $@"
                        INSERT INTO USERLIBRARIES (UserID, GameID, PurchasePrice, OrderID, IsFavorite)
                        VALUES (
                            {userId}, 
                            {game.GameID}, 
                           REPLACE('{rawPrice}', ',', '.'), -- Ép dấu phẩy thành dấu chấm ngay trong SQL
                            {orderId}, 
                            0
                            )";

                    dbManager.ExecuteQuery(sqlInsertLibrary);
                }

                MessageBox.Show("Thanh toán thành công! Trò chơi đã được thêm vào Thư viện.", "Thành công");

                // 7. Dọn dẹp giỏ hàng và chuyển trang
                CartSession.ClearCart();
                NavigationService.Navigate(new LibraryPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi: " + ex.Message);
            }
        }
    }
}
