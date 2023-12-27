using LibraryASPNET.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryASPNET.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Book/GetAllBooks
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _context.books.ToList();
            return View(books);
        }

        // GET: /Book/GetOneBook/1
        [HttpGet]
        public IActionResult GetOneBook(int id)
        {
            var book = _context.books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return View("GetOneBook", book);

        }
    }
}
