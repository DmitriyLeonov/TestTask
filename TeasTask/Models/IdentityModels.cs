using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestTask.Models;

namespace TestTask.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

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
    }

    public class ArticleContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ArticleContext(): base("ArticleContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().HasMany(c => c.Tags)
                .WithMany(s => s.Articles)
                .Map(t => t.MapLeftKey("ArticleId")
                .MapRightKey("TagId")
                .ToTable("ArticleTag"));
            modelBuilder.Entity<Category>().HasMany(c => c.Articles)
                .WithMany(s => s.Categories)
                .Map(t => t.MapLeftKey("CategoryId")
                .MapRightKey("ArticleId")
                .ToTable("CategoryArticle"));
        }
    }
}