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

        [Required]
        public string Available { get; set; }
        public byte[] ImageData { get; set; }
        public bool IsFavorite { get; internal set; }
    }
}
