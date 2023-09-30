using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VulnerableAPI.Database.Models;
using VulnerableAPI.Options;

namespace VulnerableAPI.Database;

public class AdminDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public string DatabasesLocation { get; }

    public AdminDbContext(IOptions<SqliteOptions> options)
    {
        DatabasesLocation = options.Value.DatabasesLocation;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={Path.Join(DatabasesLocation, "admin.db")}");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<User>()
            .HasIndex(u => u.Id)
            .IsUnique();
    }
}
