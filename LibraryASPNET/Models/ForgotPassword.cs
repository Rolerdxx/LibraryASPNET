using System.ComponentModel.DataAnnotations;

namespace LibraryASPNET.Models
{
    public class ForgotPassword
    {
        [Required]
        public string email { get; set; }
        public bool emailsent {  get; set; }
    }
}
