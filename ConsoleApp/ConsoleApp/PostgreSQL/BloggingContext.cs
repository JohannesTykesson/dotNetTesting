using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System;

namespace ConsoleApp.PostgreSQL
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(DatabaseInfo.ConnString);

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Post>()
                .HasGeneratedTsVectorColumn(
                    p => p.TsVector,
                    "english",
                    p => new { p.Title, p.Content })  
                .HasIndex(p => p.TsVector)
                .HasMethod("GIN");
        }
    }

    public class Blog
    {
        public Guid BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public NpgsqlTsVector TsVector { get; set; }
        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}