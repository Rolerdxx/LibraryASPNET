using Microsoft.AspNetCore.Mvc;
using LibraryASPNET.Models.User;
using LibraryASPNET;

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
            _context.users.Add(user);
            _context.SaveChanges();

            TempData["SignUpMessage"] = "Account created successfully!";
        }

        return View();
    }


    public IActionResult SignUpSuccess()
    {
        return View(); 
    }
}
