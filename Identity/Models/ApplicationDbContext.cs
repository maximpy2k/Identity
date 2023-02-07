using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class DbUser : IdentityUser
    {
        public DbUser() : base()
        {
         
        }
        public int Age { get; set; }
    }

    public class DbRole : IdentityRole
    {
        public DbRole():base()
        {

        }
    }
    public class ApplicationDbContext : IdentityDbContext<DbUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


    }
}