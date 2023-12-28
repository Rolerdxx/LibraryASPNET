using Microsoft.AspNetCore.Mvc;
using LibraryASPNET.Models;

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
