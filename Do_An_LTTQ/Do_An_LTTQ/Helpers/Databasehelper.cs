using Do_An_LTTQ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_An_LTTQ.Helpers
{
    public class GameStoreDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        // Thêm DbSet cho các bảng khác (Users, Categories...) tại đây

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Thay đổi Server Name cho khớp với máy của bạn
            string connectionString = @"Server=.;Database=GameStoreDB;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().ToTable("GAMES");
            modelBuilder.Entity<User>().ToTable("USERS"); // Account là class bạn đổi tên để tránh trùng
        }
    }
}
