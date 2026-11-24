using Microsoft.EntityFrameworkCore;
using UTS_IF133_1003260026.Models;

namespace UTS_IF133_1003260026.Data;

/// <summary>
/// DbContext aplikasi. Koneksi ke SQLite diatur lewat connection string
/// "DefaultConnection" di appsettings.json (lihat Program.cs).
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);

            // Kode produk harus unik (mis. PRD-001 tidak boleh dobel)
            entity.HasIndex(p => p.Code)
                .IsUnique();

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Price)
                .HasPrecision(18, 2);
        });
    }
}
