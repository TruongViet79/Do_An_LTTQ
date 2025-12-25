using Do_An_LTTQ.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Do_An_LTTQ.Services
{
    public class OrderService
    {
        // Hàm sử lý cho add to cart
        // Tạo đơn hàng mới
        public int CreateOrder(int userId, string paymentMethod = "CreditCard")
        {
            using (var context = new GameStoreDbContext())
            {
                var result = context.Database.ExecuteSqlRaw("EXEC sp_CreateOrder @UserID = {0}, @PaymentMethod = {1}", userId, paymentMethod);
                // Lưu ý: Logic lấy OrderID trả về cần chỉnh tùy theo cách DbContext hứng kết quả
                // Tạm thời trả về 1 để không lỗi code biên dịch
                return 1;
            }
        }

        // Thêm game vào đơn hàng
        public bool AddGameToOrder(int orderId, int gameId, int quantity = 1)
        {
            using (var context = new GameStoreDbContext())
            {
                try
                {
                    context.Database.ExecuteSqlRaw("EXEC sp_AddGameToOrder @OrderID = {0}, @GameID = {1}, @Quantity = {2}",
                        orderId, gameId, quantity);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
