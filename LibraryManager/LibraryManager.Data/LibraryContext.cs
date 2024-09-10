using LibraryManager.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Data
{
    public class LibraryContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Borrower> Borrower { get; set; }
        public DbSet<CheckoutLog> CheckoutLog { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<MediaType> MediaType {  get; set; }

        public LibraryContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

    }
}
