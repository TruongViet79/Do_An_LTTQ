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
            if (CartSession.CartItems.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Gọi OrderService để xử lý thanh toán
            OrderService orderService = new OrderService();

            // Giả sử lấy UserID từ App (bạn cần đảm bảo App.CurrentUserID có giá trị)
            // Nếu chưa có biến UserID toàn cục, bạn có thể tạm thời hardcode = 1 để test
            int userId = 1; // Thay bằng: App.CurrentUserID;

            try
            {
                // 1. Tạo Order mới
                // Lưu ý: Hàm CreateOrder trong OrderService của bạn hiện trả về 1 (hardcode). 
                // Bạn cần sửa OrderService để trả về SCOPE_IDENTITY() (OrderID thật) từ SQL.
                int orderId = orderService.CreateOrder(userId, "CreditCard");

                // 2. Thêm từng game vào OrderDetails
                foreach (var game in CartSession.CartItems)
                {
                    orderService.AddGameToOrder(orderId, game.GameID);
                }

                MessageBox.Show("Thanh toán thành công! Cảm ơn bạn đã mua hàng.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Xóa giỏ hàng và quay về Dashboard
                CartSession.ClearCart();
                NavigationService.Navigate(new DashboardPage());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message);
            }
        }
    }
}
