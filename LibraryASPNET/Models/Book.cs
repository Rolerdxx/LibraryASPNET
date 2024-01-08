using System.ComponentModel.DataAnnotations;

namespace LibraryASPNET.Models
{
    public class Book
    {

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Genre { get; set; }

        public string Available { get; set; } = "YES";

        [Required]
        public byte[] ImageData { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsFavorite { get; set; }
    }
}
