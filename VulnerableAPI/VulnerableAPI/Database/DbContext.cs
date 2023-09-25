using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VulnerableAPI.Database.Models;
using VulnerableAPI.Options;

namespace VulnerableAPI.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Ledger> Ledgers { get; set; }
    private IHttpContextAccessor _httpContextAccessor { get; }
    public string DatabasesLocation { get; }

    public DatabaseContext(IOptions<SqliteOptions> options, IHttpContextAccessor httpContextAccessor)
    {
        DatabasesLocation = options.Value.DatabasesLocation;
        _httpContextAccessor = httpContextAccessor;
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is not null)
        {
            var identity = context.User.Identity.Name;
            options.UseSqlite($"Data Source={Path.Join(DatabasesLocation, $"{identity}.db")}");
        }
        else
        {
            options.UseSqlite($"Data Source={Path.Join(DatabasesLocation, "admin.db")}");
        }
    }
}
