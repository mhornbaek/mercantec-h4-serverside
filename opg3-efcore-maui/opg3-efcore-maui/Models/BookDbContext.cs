using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace opg3_efcore_maui.Models
{
    public class BookDbContext : IdentityDbContext<BookIdentitiy>
    {
        public BookDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
    }
}
