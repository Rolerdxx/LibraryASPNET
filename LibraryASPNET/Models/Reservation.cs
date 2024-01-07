using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryASPNET.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public int Duration { get; set; }

    }
}
