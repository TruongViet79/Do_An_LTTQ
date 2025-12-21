using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Do_An_LTTQ.Helpers;
using Do_An_LTTQ.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace Do_An_LTTQ.Services
{
    public class UserService
    {
        private readonly GameStoreDbContext _context = new GameStoreDbContext();

        // Hàm lưu người dùng mới
        public bool RegisterUser(string username, string email, string password)
        {
            try
            {
                // Gọi Stored Procedure từ SQL của bạn
                _context.Database.ExecuteSqlRaw("EXEC sp_AddUser @Username={0}, @Email={1}, @PasswordHash={2}",
                                                 username, email, password);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
