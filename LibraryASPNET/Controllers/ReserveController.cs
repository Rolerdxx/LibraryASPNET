using LibraryASPNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryASPNET.Controllers
{
    public class ReserveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReserveController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Reserve(int bookId)
        {
            _context.reservations.Add(new Reservation { BookId = bookId, Date = DateTime.Now.ToShortDateString(), Duration = 2, UserId = (int)TempData["ConnectedUserId"] });
            _context.SaveChanges();
            var bookToUpdate = _context.books.Find(bookId);

            if (bookToUpdate != null)
            {
                bookToUpdate.Available = "NO"; 
                _context.SaveChanges();
            }
            return RedirectToAction("GetAllBooks", "Book");
        }
    }
}
