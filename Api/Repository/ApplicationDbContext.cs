using AuthTokens.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthTokens.Repository
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) :
            base(options)
        {
            
        }
        public DbSet<Customer> Customers { get; set; }
    }
}