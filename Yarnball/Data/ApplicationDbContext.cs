using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Yarnball.Data
{
    public class ApplicationDbContext : IdentityDbContext<YarnballUser, YarnballRole, Guid>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public override DbSet<YarnballUser> Users { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Add user-to-post relationship
            builder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.UserId);

            // Add post-to-tags relationship
            builder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });
            builder.Entity<PostTag>()
                .HasOne(p => p.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(p => p.PostId);
            builder.Entity<PostTag>()
                .HasOne(t => t.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(t => t.TagId);

            base.OnModelCreating(builder);
        }
    }
}