using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Resit_Project.Models;

namespace Resit_Project.Models

{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<PriceList> PriceLists { get; set; }

        public DbSet<Work> Works { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }

    }
}