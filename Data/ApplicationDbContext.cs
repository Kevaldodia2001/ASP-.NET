// ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JockWebApp.Models;

namespace JockWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Jock> Jock { get; set; }
    }
}

// Jock.cs
namespace JockWebApp.Models
{
    public class Jock
    {
        public int ID { get; set; }
        public string JockQuestion { get; set; }
        public string JockAnswer { get; set; }
    }
}
