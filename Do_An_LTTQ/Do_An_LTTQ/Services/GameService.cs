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
                // Sử dụng LINQ để Join bảng lấy tên Dev và Pub
                var query = from g in context.Games
                            where g.GameID == gameId
                            // Nối bảng Developer
                            join d in context.Developers on g.DeveloperID equals d.DeveloperID into dGroup
                            from dev in dGroup.DefaultIfEmpty() // Left Join (đề phòng null)
                                                                // Nối bảng Publisher
                            join p in context.Publishers on g.PublisherID equals p.PublisherID into pGroup
                            from pub in pGroup.DefaultIfEmpty() // Left Join
                            select new
                            {
                                GameObj = g,
                                DevName = dev != null ? dev.DeveloperName : "Unknown",
                                PubName = pub != null ? pub.PublisherName : "Unknown"
                            };

                var result = query.FirstOrDefault();

                if (result != null)
                {
                    var game = result.GameObj;

                    // Gán dữ liệu lấy được vào các biến [NotMapped] trong Game.cs
                    game.DeveloperName = result.DevName;
                    game.PublisherName = result.PubName;

                    // Xử lý Categories (Code cũ của bạn vẫn giữ nguyên)
                    try
                    {
                        string sqlCat = @"SELECT c.CategoryName FROM CATEGORIES c
                                  JOIN GAMECATEGORIES gc ON c.CategoryID = gc.CategoryID
                                  WHERE gc.GameID = {0}";
                        var catNames = context.Database.SqlQueryRaw<string>(sqlCat, gameId).ToList();
                        game.Categories = (catNames != null && catNames.Count > 0) ? string.Join(", ", catNames) : "General";
                    }
                    catch { game.Categories = ""; }

                    return game;
                }

                return null;
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