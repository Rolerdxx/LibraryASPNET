using Microsoft.EntityFrameworkCore;

namespace LibraryASPNET.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<Book> books { get; set; }
        public DbSet<Reservation> reservations { get; set; }
        public object Users { get; internal set; }
    }
}
