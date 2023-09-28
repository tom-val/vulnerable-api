using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VulnerableAPI.Database.Models;
using VulnerableAPI.Options;

namespace VulnerableAPI.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Ledger> Ledgers { get; set; }
    public DbSet<User> Users { get; set; }
    private IHttpContextAccessor _httpContextAccessor { get; }
    public string DatabasesLocation { get; }

    public DatabaseContext(IOptions<SqliteOptions> options, IHttpContextAccessor httpContextAccessor)
    {
        DatabasesLocation = options.Value.DatabasesLocation;
        _httpContextAccessor = httpContextAccessor;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var identity = _httpContextAccessor.HttpContext?.User.Identity;
        if (identity is not null && identity.IsAuthenticated)
        {
            options.UseSqlite($"Data Source={Path.Join(DatabasesLocation, $"{identity.Name}.db")}");
        }
        else
        {
            options.UseSqlite($"Data Source={Path.Join(DatabasesLocation, "admin.db")}");
        }
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
