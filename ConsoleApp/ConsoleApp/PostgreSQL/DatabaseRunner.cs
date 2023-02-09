using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
                foreach(var blog in _context.Blogs) {
                    Console.WriteLine(blog.BlogId);
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Exception caught:");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}