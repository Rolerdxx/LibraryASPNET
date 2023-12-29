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
using MimeKit;

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


        string resetToken = Guid.NewGuid().ToString();
        user.ResetToken = resetToken;
        user.ResetTokenExpiration = DateTime.UtcNow.AddHours(1); 

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

    public ActionResult ForgotPasswordConfirmation()
    {
        return View("~/Views/Login/Login.cshtml");
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

        user.Password = newPassword; 
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
