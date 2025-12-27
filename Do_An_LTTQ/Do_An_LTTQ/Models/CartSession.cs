using Do_An_LTTQ.Models;
using System.Collections.Generic;
using System.Linq;

namespace Do_An_LTTQ.Helpers
{
    public static class CartSession
    {
        // Danh sách các game trong giỏ
        public static List<Game> CartItems { get; private set; } = new List<Game>();

        public static void AddToCart(Game game)
        {
            // Kiểm tra nếu game đã có trong giỏ chưa, nếu chưa thì thêm
            if (!CartItems.Any(g => g.GameID == game.GameID))
            {
                CartItems.Add(game);
            }
        }

        public static void RemoveFromCart(Game game)
        {
            var item = CartItems.FirstOrDefault(g => g.GameID == game.GameID);
            if (item != null)
            {
                CartItems.Remove(item);
            }
        }

        public static void ClearCart()
        {
            CartItems.Clear();
        }

        public static decimal GetTotalAmount()
        {
            // Tính tổng tiền, xử lý null bằng cách chuyển về 0
            return CartItems.Sum(g => g.FinalPrice ?? 0);
        }
    }
}