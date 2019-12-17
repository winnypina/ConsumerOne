using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ConsumerOne.Api.Models;

namespace ConsumerOne.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<UseTerm> UseTerms { get; set; }

        public DbSet<PrivacyPolicy> PrivacyPolicies { get; set; }

        public DbSet<UsersByDateReport> UsersByDate { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostComment> PostComments { get; set; }

        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostMedia> PostMedias { get; set; }
        public DbSet<PostReport> PostReports { get; set; }

        public DbSet<TextTranslation> TextTranslations { get; set; }

        public DbSet<Relation> Relations { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<UserMessage> UserMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            //builder.Entity<Relation>().HasKey(table => new {
            //    table.ParentId,
            //    table.ChildId
            //});

            

            builder.Entity<AppUser>()
                .HasMany(c => c.Posts)
                .WithOne(e => e.AppUser);

            builder.Entity<Post>().HasMany(n => n.PostLikes);

            builder.Entity<UserMessage>().HasOne(n => n.To);
            builder.Entity<UserMessage>().HasOne(n => n.From);

            builder.Entity<AppUser>().HasMany(n => n.Ratings).WithOne(n => n.To);
            builder.Entity<Rating>().HasOne(n => n.From);


            base.OnModelCreating(builder);
        }
    }
}