using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Do_An_LTTQ.Models
{
    public class Developer
    {
        [Key]
        public int DeveloperID { get; set; }
        public string DeveloperName { get; set; }
        public string? LogoURL { get; set; }
        public string? WebsiteURL { get; set; }
    }
}
