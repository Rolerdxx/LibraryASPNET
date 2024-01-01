using Microsoft.AspNetCore.Mvc;
using LibraryASPNET.Models;
using System.Text;
using System.Security.Cryptography;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [HttpPost]
    public IActionResult SignUp(User user)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _context.users.FirstOrDefault(u => u.Email == user.Email);

            if (existingUser != null)
            {
                TempData["SignUpMessage"] = "This email is already in use. Would you like to log in instead?";
                TempData["ExistingEmail"] = user.Email;
            }
            else
            {
                string hashedPassword = HashPassword(user.Password);
                user.Password = hashedPassword;
                _context.users.Add(user);
                _context.SaveChanges();

                TempData["SignUpMessage"] = "Account created successfully!";
                return SignUpm();
            }
        }
        else
        {
            
            TempData["SignUpMessage"] = "Failed to create the account. Please check the entered details.";
            return SignUpm(); 
        }

        return SignUpm(); 
    }

    private string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }



    public IActionResult ShowLoginOption()
        {
        return View("~/Views/Login/Login.cshtml");
        }

    public IActionResult Login()
    {
        return View("~/Views/Login/Login.cshtml");
    }

    public IActionResult SignUpm()
    {
        return View("~/Views/Home/Index.cshtml"); 
    }
}
