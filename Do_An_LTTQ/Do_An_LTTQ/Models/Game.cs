using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Do_An_LTTQ.Models
{
    public class Game
    {
        [Key] // Khóa chính
        public int GameID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? FinalPrice { get; set; }
        public string MainCoverImageURL { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string AgeRating { get; set; }
        public string Categories { get; set; }
        public string SystemRequirements { get; set; } 
        public string SupportedLanguages { get; set; }
        [NotMapped]
        public string DeveloperName { get; set; }

        [NotMapped]
        public string PublisherName { get; set; }

        public List<string> CategoryList
        {
            get
            {
                if (string.IsNullOrEmpty(Categories)) return new List<string>();
                return Categories.Split(',').Select(s => s.Trim()).ToList();
            }
        }
    }
}
