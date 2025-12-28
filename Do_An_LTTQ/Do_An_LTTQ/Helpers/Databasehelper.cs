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
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        // Thêm DbSet cho các bảng khác (Users, Categories...) tại đây

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 1. Lấy chuỗi kết nối hiện tại
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GameStoreConn"].ConnectionString;

            // 2. Thêm "TrustServerCertificate=True" vào cuối chuỗi nếu chưa có
            if (!connectionString.Contains("TrustServerCertificate"))
            {
                connectionString += ";TrustServerCertificate=True";
            }

            // 3. Cấu hình UseSqlServer với chuỗi mới và EnableRetryOnFailure
            optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.EnableRetryOnFailure();
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().ToTable("GAMES");
            modelBuilder.Entity<User>().ToTable("USERS"); // Account là class bạn đổi tên để tránh trùng
            modelBuilder.Entity<Developer>().ToTable("DEVELOPERS");
            modelBuilder.Entity<Publisher>().ToTable("PUBLISHERS");
        }
    }
}
