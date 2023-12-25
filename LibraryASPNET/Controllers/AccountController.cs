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
    public IActionResult SignUp()
    {
        return View();
    }
    public IActionResult Login()
    {
        return View(); 
    }

    [HttpPost]
    [HttpGet]
    public IActionResult SignUp(User user)
    {
        var existingUser = _context.users.FirstOrDefault(u => u.Email == user.Email);
        if (ModelState.IsValid)
        {
             if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
             {
                    TempData["SignUpErrorMessage"] = "All the inputs should be filled";
                        return View("SignUp");
             } else if (existingUser != null)
             {
                    TempData["SignUpErrorMessage"] = "An account with this email already exists.";
                        return View("SignUp");
                
             }
            _context.users.Add(user);
            _context.SaveChanges();

                    TempData["SignUpMessage"] = "Account created successfully!";
                         return View("SignUp");
        }

        return View(user);
    }

    public IActionResult SignUpSuccess()
    {
        return View();
    }
}
