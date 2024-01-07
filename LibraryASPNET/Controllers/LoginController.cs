using Microsoft.AspNetCore.Mvc;
using LibraryASPNET.Models;
using Microsoft.EntityFrameworkCore;

public class LoginController : Controller
{
    private readonly ApplicationDbContext _context;

    public LoginController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        TempData.Clear();
        return View();
    }

    [HttpPost]
    public IActionResult Login(User user)
    {
        var existingUser = _context.users.FirstOrDefault(u => u.Email == user.Email);

        TempData.Remove("LoginError");
        if (existingUser != null && existingUser.Password == user.Password)
        {
            TempData["LoginSuccess"] = "Login successful!";
            TempData.Keep("LoginSuccess");
            TempData.Remove("LoginError");
            TempData["ConnectedUserId"] = existingUser.Id;
            return RedirectToAction("GetAllBooks", "Book");
        }
        else
        {
            TempData["LoginError"] = "Invalid email or password";
            TempData.Keep("LoginError");
            TempData.Remove("LoginSuccess");
            return View();
        }
    }


    public IActionResult LoginSuccess()
    {
        return View();
    }
}
