using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoremNET;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleApp.PostgreSQL
{
    class DatabaseRunner
    {
        BloggingContext _context = new BloggingContext();

        public async Task CheckInfoInDb() {
            
            try {
                    
                var canConnect = await _context.Database.CanConnectAsync();
                if (!canConnect) {
                    Console.WriteLine("ERROR could not connect to db");
                    return;
                }
                Console.WriteLine("Migrating database");
                await _context.Database.MigrateAsync();

                var migrations = await _context.Database.GetAppliedMigrationsAsync();
                foreach (var migration in migrations) {
                    Console.WriteLine("Applied: " + migration.ToString());
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Exception caught:");
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task SeedDatabase() {
            // Adding fake blogs
            for (var i = 0; i < 10000; i++) {
                var blog = new Blog() {
                    BlogId = Guid.NewGuid(),
                    Url = "fakeurl.com",
                    Posts = new List<Post>()
                };
                for (var j = 0; j < 10; j++) {
                    blog.Posts.Add(new Post() {
                            Content = Lorem.Words(10, 50),
                            PostId = Guid.NewGuid(),
                            Title = "LameTitle"
                        }
                    );
                }
                await _context.Blogs.AddAsync(blog);
            }
            await _context.SaveChangesAsync();
        }

        public void Benchmark() {
            var word = Lorem.Words(1,1);
                
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var q = _context.Posts.Where(post => 
                post.Content.Contains(word)).Count();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            watch = System.Diagnostics.Stopwatch.StartNew();
            var q2 = _context.Posts.Where(post => 
                post.TsVector.Matches(EF.Functions.ToTsQuery(word))).Count();

            watch.Stop();
            var elapsedMsTsVector = watch.ElapsedMilliseconds;


            Console.WriteLine(q);
            Console.WriteLine(q2);
            Console.WriteLine("Contains: " + elapsedMs);
            Console.WriteLine("TsVector: " + elapsedMsTsVector);
        }
    }
}