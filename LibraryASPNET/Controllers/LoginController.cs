using Microsoft.AspNetCore.Mvc;
using LibraryASPNET.Models;

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
        return View();
    }

    [HttpPost]
   
    public IActionResult Login(User user)
    {

   
            var existingUser = _context.users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

            if (existingUser != null)
            {
                TempData["LoginSuccess"] = "Login successful!";
                return RedirectToAction("Home", "Index");
            }
            else
            {
                TempData["LoginError"] = "Invalid email or password";
            }
        

        return View();


    }

    public IActionResult LoginSuccess()
    {
        return View();
    }
}
