using LibraryASPNET.Models;
using Microsoft.AspNetCore.Mvc;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using PdfSharp.Fonts;
using PdfSharp.Drawing.Layout;

namespace LibraryASPNET.Controllers
{
    public class ReserveController : Controller
    {
        //private int? connectedUserId;
        static ReserveController()
        {
            GlobalFontSettings.FontResolver = new CustomFontResolver();
        }

        private readonly ApplicationDbContext _context;

        public ReserveController(ApplicationDbContext context)
        {
            _context = context;
        }
        private byte[] GenerateReservationPdf(int bookId, string reservationDate, string duration, string userEmail)
        {
            PdfDocument pdf = new PdfDocument();
            PdfPage page = pdf.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont titleFont = new XFont("Verdana", 24);
            XFont sectionTitleFont = new XFont("Verdana", 16);
            XFont contentFont = new XFont("Verdana", 12);

            // Draw a line for separation
            XPen linePen = new XPen(XColors.DarkGray, 1);
            gfx.DrawLine(linePen, 30, 70, page.Width - 30, 70);

            // Title
            gfx.DrawString("Reservation Details", titleFont, XBrushes.Black, new XRect(30, 30, page.Width.Point, 40), XStringFormats.TopLeft);

            // Book ID section
            gfx.DrawString("Book ID:", sectionTitleFont, XBrushes.Black, new XRect(30, 90, page.Width.Point, 40), XStringFormats.TopLeft);
            gfx.DrawString(bookId.ToString(), contentFont, XBrushes.Black, new XRect(120, 90, page.Width.Point - 120, 40), XStringFormats.TopLeft);

            // Date section
            gfx.DrawString("Date:", sectionTitleFont, XBrushes.Black, new XRect(30, 120, page.Width.Point, 40), XStringFormats.TopLeft);
            gfx.DrawString(reservationDate, contentFont, XBrushes.Black, new XRect(120, 120, page.Width.Point - 120, 40), XStringFormats.TopLeft);

            // Duration section
            gfx.DrawString("Duration:", sectionTitleFont, XBrushes.Black, new XRect(30, 150, page.Width.Point, 40), XStringFormats.TopLeft);
            gfx.DrawString(duration, contentFont, XBrushes.Black, new XRect(120, 150, page.Width.Point - 120, 40), XStringFormats.TopLeft);

            // User Email section
            gfx.DrawString("User Email:", sectionTitleFont, XBrushes.Black, new XRect(30, 180, page.Width.Point, 40), XStringFormats.TopLeft);
            gfx.DrawString(userEmail, contentFont, XBrushes.Black, new XRect(120, 180, page.Width.Point - 120, 40), XStringFormats.TopLeft);

            using (MemoryStream stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                return stream.ToArray();
            }
        }


        /* public IActionResult ReserveAsync(int bookId)
         {
             _context.reservations.Add(new Reservation { BookId = bookId, Date = DateTime.Now.ToShortDateString(), Duration = 2, UserId = (int)TempData["ConnectedUserId"] });
             _context.SaveChanges();
             var bookToUpdate = _context.books.Find(bookId);

             if (bookToUpdate != null)
             {
                 bookToUpdate.Available = "NO";
                 _context.SaveChanges();
             }

             var user = _context.users.FirstOrDefault(u => u.Id == (int)TempData["ConnectedUserId"]);

             if (user != null)
             {
                 string reservationDate = DateTime.Now.ToShortDateString();
                 string duration = "2 hours"; 


                 byte[] pdfBytes = GenerateReservationPdf(bookId, reservationDate, duration, user.Email);


                 string fileName = $"Reservation_{bookId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";


                 return File(pdfBytes, "application/pdf", fileName);
             }

             return RedirectToAction("GetAllBooks", "Book");
         }
 */

        public IActionResult MyReservations()
        {
            if (TempData["ConnectedUserId"] != null && TempData["ConnectedUserId"] is int connectedUserId)
            {
                var reservations = _context.reservations.Where(r => r.UserId == connectedUserId).ToList();
                return View(reservations);
            }
            else
            {
                return RedirectToAction("GetAllBooks", "Book");
            }
        }

        [HttpPost]
        public IActionResult DeleteReservation(int reservationId)
        {
            var reservation = _context.reservations.Find(reservationId);
            if (reservation != null)
            {
                _context.reservations.Remove(reservation);
                _context.SaveChanges();
                return RedirectToAction("MyReservations");
            }
            return NotFound();
        }


        [HttpPost]
        public IActionResult ReserveForm(int bookId, string date, int duration)
        {
            try
            {
                var connectedUserId = TempData["ConnectedUserId"];

                if (connectedUserId != null && int.TryParse(connectedUserId.ToString(), out int userId))
                {
                    _context.reservations.Add(new Reservation { BookId = bookId, Date = date, Duration = duration, UserId = userId });
                    _context.SaveChanges();

                    var bookToUpdate = _context.books.Find(bookId);
                    if (bookToUpdate != null)
                    {
                        bookToUpdate.Available = "NO";
                        _context.SaveChanges();
                    }

                    var user = _context.users.FirstOrDefault(u => u.Id == userId);

                    if (user != null)
                    {
                        string reservationDate = date;
                        string userEmail = user.Email;
                        string durationString = $"{duration} Days";

                        byte[] pdfBytes = GenerateReservationPdf(bookId, reservationDate, durationString, userEmail);

                        string fileName = $"Reservation_{bookId}_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                        return File(pdfBytes, "application/pdf", fileName);
                    }
                }

                TempData["ErrorMessage"] = "Please log in to perform this action.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return RedirectToAction("Error", "Home");
            }
        }




    }
}
