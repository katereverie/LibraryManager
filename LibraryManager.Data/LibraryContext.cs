using LibraryManager.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Data;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Borrower>(e =>
        {
            e.HasKey(b => b.BorrowerID);
            e.Property(b => b.FirstName).IsRequired();
            e.Property(b => b.LastName).IsRequired();
            e.Property(b => b.Email).IsRequired();
            e.Property(b => b.Phone).IsRequired();
            e.HasMany(b => b.CheckoutLogs)
            .WithOne(c => c.Borrower)
            .HasForeignKey(c => c.BorrowerID);
        });

        modelBuilder.Entity<Media>(e =>
        {
            e.HasKey(m => m.MediaID);
            e.Property(m => m.Title).IsRequired();
            e.HasOne(m => m.MediaType)
            .WithMany(mt => mt.Medias)
            .HasForeignKey(m => m.MediaTypeID)
            .OnDelete(DeleteBehavior.Restrict);
            e.HasMany(m => m.CheckoutLogs)
            .WithOne(c => c.Media)
            .HasForeignKey(c => c.MediaID)
            .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<MediaType>(e =>
        {
            e.HasKey(mt => mt.MediaTypeID);
            e.Property(mt => mt.MediaTypeName).IsRequired();
        });

        modelBuilder.Entity<CheckoutLog>(e =>
        {
            e.HasKey(c => c.CheckoutLogID);
            e.Property(c => c.CheckoutDate).IsRequired();
            e.Property(c => c.DueDate).IsRequired();
        });
    }

}
