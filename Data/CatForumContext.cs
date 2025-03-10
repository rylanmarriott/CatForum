using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CatForum.Models;
namespace CatForum.Data
{
    public class CatForumContext : IdentityDbContext<ApplicationUser>
    {
        public CatForumContext(DbContextOptions<CatForumContext> options)
            : base(options)
        {
        }

        public DbSet<Discussion> Discussion { get; set; }
        public DbSet<Comment> Comment { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define relationships
            builder.Entity<Discussion>()
                .HasOne(d => d.ApplicationUser)
                .WithMany()
                .HasForeignKey(d => d.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Comment>()
                .HasOne(c => c.ApplicationUser)
                .WithMany()
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

}
