using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_LTTQ.Models
{
    public class User
    {
        [Key] // Đánh dấu đây là Khóa Chính (khớp với UserID trong SQL)
        public int UserID { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        // Mấy trường này trong SQL cho phép NULL nên mình thêm dấu ?
        public string? AvatarURL { get; set; }

        public decimal? WalletBalance { get; set; }

        public string? CountryCode { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
    }
}
