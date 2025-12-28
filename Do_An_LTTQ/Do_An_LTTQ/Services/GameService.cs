using System;
using System.Collections.Generic;
using System.Linq;
using Do_An_LTTQ.Helpers;
using Do_An_LTTQ.Models;
using Microsoft.EntityFrameworkCore;

namespace Do_An_LTTQ.Services
{
    public class GameService
    {
        private readonly GameStoreDbContext _context = new GameStoreDbContext();

        public List<Game> GetAllGames()
        {
            using (var context = new GameStoreDbContext())
            {
                // Lấy toàn bộ danh sách game từ bảng GAMES
                return context.Games.ToList();
            }
        }

        public List<Game> SearchGames(string searchTerm)
        {
            return _context.Games
                .FromSqlRaw("EXEC sp_SearchGames @SearchTerm = {0}", searchTerm ?? (object)DBNull.Value)
                .ToList();
        }

        public Game GetGameDetails(int gameId)
        {
            using (var context = new GameStoreDbContext())
            {
                // 1. Lấy thông tin game (EF sẽ bỏ qua cột Categories nhờ [NotMapped])
                var game = context.Games.FirstOrDefault(g => g.GameID == gameId);

                if (game != null)
                {
                    // 2. Truy vấn thủ công để lấy danh sách tên thể loại
                    // Giả sử bảng danh mục tên là CATEGORY, cột tên là CategoryName
                    // Bảng trung gian là GAMECATEGORY
                    try
                    {
                        // Câu lệnh SQL lấy tên thể loại dựa trên GameID
                        string sql = @"
                            SELECT c.CategoryName 
                            FROM CATEGORIES c
                            JOIN GAMECATEGORIES gc ON c.CategoryID = gc.CategoryID
                            WHERE gc.GameID = {0}";

                        // Chạy query lấy list tên
                        var catNames = context.Database
                                              .SqlQueryRaw<string>(sql, gameId)
                                              .ToList();

                        // 3. Nối lại thành chuỗi (VD: "Action, RPG") để UI hiển thị được
                        game.Categories = string.Join(", ", catNames);
                    }
                    catch
                    {
                        // Nếu lỡ bảng tên khác thì nó ko chết app, chỉ trống category thôi
                        game.Categories = "";
                    }
                }
                return game;
            }
        }

        public List<Game> GetGamesByCategory(int categoryId)
        {
            using (var context = new GameStoreDbContext())
            {
                return context.Games
                    .FromSqlRaw("EXEC sp_GetGamesByCategory @CategoryID = {0}", categoryId)
                    .ToList();
            }
        }
    }
}