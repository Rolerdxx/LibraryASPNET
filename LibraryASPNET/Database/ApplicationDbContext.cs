using Microsoft.EntityFrameworkCore;

using LibraryASPNET.Models.User;

namespace LibraryASPNET
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public object Users { get; internal set; }
    }
}
