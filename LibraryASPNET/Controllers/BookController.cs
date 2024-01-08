using LibraryASPNET.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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
/*            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account"); 
            }*/
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
        [HttpPost]
        public IActionResult ToggleFavorite(int bookId)
        {
            var book = _context.books.Find(bookId);
            if (book != null)
            {
              
                book.IsFavorite = !book.IsFavorite;
                _context.SaveChanges(); 
                return Ok(); 
            }

            return BadRequest(); 
        }

        [HttpGet]
        public IActionResult MyFavorites()
        {
            var favoriteBooks = _context.books.Where(b => b.IsFavorite).ToList();
            return View(favoriteBooks);
        }

    }
}
