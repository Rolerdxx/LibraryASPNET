using Microsoft.EntityFrameworkCore;

using LibraryASPNET.Models.User;
using LibraryASPNET.Models.Book;

namespace LibraryASPNET
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Book> books { get; set; }
        public object Users { get; internal set; }
    }
}
