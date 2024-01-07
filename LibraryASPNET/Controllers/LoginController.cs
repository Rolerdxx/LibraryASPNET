using Microsoft.AspNetCore.Mvc;
using LibraryASPNET.Models;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Text;
using System.Security.Cryptography;

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

    [HttpPost]
    public IActionResult Login(User user)
    {
        var existingUser = _context.users.FirstOrDefault(u => u.Email == user.Email);

        if (existingUser != null)
        {
            string hashedPassword = HashPassword(user.Password);


            if (existingUser.Password == hashedPassword)
            {
                TempData["LoginSuccess"] = "Login successful!";
                TempData.Remove("LoginError");
                TempData["ConnectedUserId"] = existingUser.Id;
                return RedirectToAction("GetAllBooks", "Book");
            }
        }

        TempData["LoginError"] = "Invalid email or password";
        return View();
    }


    [HttpGet]
    public ActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ForgotPassword(string email)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            TempData["LoginError"] = "User with this email does not exist.";
            return View();
        }


        string resetToken = Guid.NewGuid().ToString();
        user.ResetToken = resetToken;
        user.ResetTokenExpiration = DateTime.UtcNow.AddMinutes(10);

        _context.SaveChanges();


        string resetLink = Url.Action("ResetPassword", "Login", new { token = resetToken }, Request.Scheme);
        string emailBody = $"Please click <a href=\"{resetLink}\">here</a> to reset your password.";

        await SendEmailAsync(user.Email, "Password Reset", emailBody);

        return ForgotPasswordConfirmation();
    }

    private async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Library Admin", "technoforge1@gmail.com"));
        message.To.Add(new MailboxAddress("Recipient", recipientEmail));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync("technoforge1@gmail.com", "nljx kjmk pxsb awdm");

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
    public ActionResult GetAllBooks()
    {
        return View("~/Views/Book/GetAllBooks.cshtml");
    }
    public ActionResult ForgotPasswordConfirmation()
    {
        return View("~/Views/Login/Login.cshtml");
    }

    [HttpGet]
    public ActionResult ResetPassword(string token)
    {
/*        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }*/

        var user = _context.users.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiration > DateTime.UtcNow);

        if (user == null)
        {
            return View();
        }
        else
        {
            ViewBag.ResetToken = token;
            return View();
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ResetPassword(string token, string newPassword)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiration > DateTime.UtcNow);



        if (user == null)
        {

            return View();
        }
        else
        {
            string hashedPassword = HashPassword(newPassword);
            user.Password = hashedPassword;
            user.ResetToken = null;
            user.ResetTokenExpiration = null;
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Login", "Login");
    }

    public IActionResult LoginSuccess()
    {
        return View("~/Views/Book/GetAllBooks.cshtml");
    }
}