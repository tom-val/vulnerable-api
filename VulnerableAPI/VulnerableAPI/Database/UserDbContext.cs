using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VulnerableAPI.Database.Enums;
using VulnerableAPI.Database.Models;
using VulnerableAPI.Options;

namespace VulnerableAPI.Database;

public class UserDbContext : DbContext
{
    public DbSet<Ledger> Ledgers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<MoneyRequest> MoneyRequests { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Referral> Referrals { get; set; }
    private IHttpContextAccessor? _httpContextAccessor { get; }
    private Guid? _userId { get; set; }


    public DatabaseOptions DatabaseOptions { get; }

    public UserDbContext(IOptions<DatabaseOptions> options, IHttpContextAccessor httpContextAccessor)
    {
        DatabaseOptions = options.Value;
        _httpContextAccessor = httpContextAccessor;
    }

    public UserDbContext(IOptions<DatabaseOptions> options, Guid userId)
    {
        DatabaseOptions = options.Value;
        _userId = userId;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var identity = _httpContextAccessor?.HttpContext?.User.Identity;
        if (identity is not null && identity.IsAuthenticated)
        {
            var claim = _httpContextAccessor!.HttpContext?.User.Claims.First(x => x.Type == "please_do_not_change_thanks");
            var valueBytes = Convert.FromBase64String(claim.Value);
            var userId = Encoding.UTF8.GetString(valueBytes);
            options.UseNpgsql(DatabaseOptions.ConnectionString(userId));
        }
        else if (_userId.HasValue)
        {
            options.UseNpgsql(DatabaseOptions.ConnectionString(_userId.ToString()));
        }
        else
        {
            options.UseNpgsql(DatabaseOptions.ConnectionString("temp_db_for_migrations"));
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
