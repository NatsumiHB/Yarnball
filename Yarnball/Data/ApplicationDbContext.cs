using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Yarnball.Data
{
    public class ApplicationDbContext : IdentityDbContext<YarnballUser, YarnballRole, Guid>
    {
        public DbSet<Post> Posts { get; set; }
        public override DbSet<YarnballUser> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.UserId);

            base.OnModelCreating(builder);
        }
    }
}