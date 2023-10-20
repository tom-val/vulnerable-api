using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VulnerableAPI.Database.Models;
using VulnerableAPI.Options;

namespace VulnerableAPI.Database;

public class AdminDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DatabaseOptions DatabaseOptions { get; }

    public AdminDbContext(IOptions<DatabaseOptions> options)
    {
        DatabaseOptions = options.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(DatabaseOptions.ConnectionString("admin"));
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
