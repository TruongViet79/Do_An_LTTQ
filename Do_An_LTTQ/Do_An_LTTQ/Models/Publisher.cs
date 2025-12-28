using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_An_LTTQ.Models
{
    public class Publisher
    {
        [Key]
        public int PublisherID { get; set; }
        public string PublisherName { get; set; }
        public string? LogoURL { get; set; }
        public string? WebsiteURL { get; set; }
    }
}
