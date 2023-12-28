using Microsoft.AspNetCore.Mvc;
using LibraryASPNET.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net.Mail.Abstractions;
using MailKit.Net.Smtp;
using SmtpClient = System.Net.Mail.SmtpClient;

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

        if (existingUser != null && existingUser.Password == user.Password)
        {
            TempData["LoginSuccess"] = "Login successful!";
            TempData.Remove("LoginError");
            return RedirectToAction("GetAllBooks", "Book");
        }
        else if (existingUser != null && existingUser.Password != user.Password)
        {
            TempData["LoginError"] = "Invalid email or password";
            return View();
        }

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
            TempData["UserNotFound"] = "User with this email does not exist.";
            return RedirectToAction("UserNotFound", "Error");
        }

        // Generate a reset token and set its expiration
        string resetToken = Guid.NewGuid().ToString();
        user.ResetToken = resetToken;
        user.ResetTokenExpiration = DateTime.UtcNow.AddHours(1); // Example: Token expires in 1 hour

        _context.SaveChanges();

        // Send an email containing the reset link with the token
        string resetLink = Url.Action("ResetPassword", "Login", new { token = resetToken }, Request.Scheme);
        string emailBody = "Please click to reset your password.";

        await SendEmailAsync(user.Email, "Password Reset", emailBody);

        return RedirectToAction("ForgotPasswordConfirmation", "User");
    }

    private async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {

        using (var client = new SmtpClient("smtp.gmail.com", 587)) // Replace with your SMTP server details
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("technoforge1@gmail.com", "HAMZA2001"); // Replace with your credentials
            client.EnableSsl = true; // Enable SSL if required
            /*
            var message = new MailMessage("technoforge1@gmail.com", recipientEmail, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);*/

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("technoforge1@gmail.com");
            mailMessage.To.Add(recipientEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;
        }
    }

    public ActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public ActionResult ResetPassword(string token)
    {
        var user = _context.users.FirstOrDefault(u => u.ResetToken == token && u.ResetTokenExpiration > DateTime.UtcNow);

        if (user == null)
        {
            return RedirectToAction("InvalidToken", "Error");
        }

        ViewBag.ResetToken = token;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> ResetPassword(string token, string newPassword)
    {
        var user = await _context.users.FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiration > DateTime.UtcNow);

        if (user == null)
        {
            return RedirectToAction("InvalidToken", "Error");
        }

        user.Password = newPassword; // Remember to hash the password
        user.ResetToken = null;
        user.ResetTokenExpiration = null;

        _context.SaveChanges();

        return RedirectToAction("Login", "User");
    }

    public IActionResult LoginSuccess()
    {
        return View();
    }
}
