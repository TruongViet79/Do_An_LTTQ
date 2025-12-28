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
                // Sử dụng Find hoặc FirstOrDefault để lấy game
                var game = context.Games.FirstOrDefault(g => g.GameID == gameId);

                if (game != null)
                {
                    try
                    {
                        // Thay vì SqlQueryRaw, hãy dùng logic Linq nếu có thể để EF quản lý kết nối tốt hơn
                        string sql = @"
                    SELECT c.CategoryName 
                    FROM CATEGORIES c
                    JOIN GAMECATEGORIES gc ON c.CategoryID = gc.CategoryID
                    WHERE gc.GameID = {0}";

                        var catNames = context.Database
                                              .SqlQueryRaw<string>(sql, gameId)
                                              .ToList();

                        game.Categories = (catNames != null && catNames.Count > 0)
                                          ? string.Join(", ", catNames)
                                          : "";
                    }
                    catch (Exception)
                    {
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