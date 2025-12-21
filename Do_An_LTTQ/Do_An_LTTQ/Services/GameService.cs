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
    }
}