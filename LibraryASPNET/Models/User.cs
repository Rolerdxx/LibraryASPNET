namespace LibraryASPNET.Models;

using System.ComponentModel.DataAnnotations;

public class User 
{
    public int Id { get; set; }

    [Required] 
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool IsAdmin { get; set; }

    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiration { get; set; }

}
