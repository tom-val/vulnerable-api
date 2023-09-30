using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VulnerableAPI.Database.Models;
using VulnerableAPI.Options;

namespace VulnerableAPI.Database;

public class UserDbContext : DbContext
{
    public DbSet<Ledger> Ledgers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<MoneyRequest> MoneyRequests { get; set; }
    private IHttpContextAccessor? _httpContextAccessor { get; }
    public string DatabasesLocation { get; }
    private Guid? _userId { get; set; }

    public UserDbContext(IOptions<SqliteOptions> options, IHttpContextAccessor httpContextAccessor)
    {
        DatabasesLocation = options.Value.DatabasesLocation;
        _httpContextAccessor = httpContextAccessor;
    }

    public UserDbContext(IOptions<SqliteOptions> options, Guid userId)
    {
        DatabasesLocation = options.Value.DatabasesLocation;
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
            options.UseSqlite($"Data Source={Path.Join(DatabasesLocation, $"{userId}.db")}");
        }
        else if (_userId.HasValue)
        {
            options.UseSqlite($"Data Source={Path.Join(DatabasesLocation, $"{_userId}.db")}");
        }
        else
        {
            options.UseSqlite($"Data Source={Path.Join(DatabasesLocation, "temp_db_for_migrations.db")}");
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

        var userIds = new List<Guid>
        {
            Guid.Parse("15a61231-a186-4fc9-ae24-a83d582eaa4d"),
            Guid.Parse("f373061d-5a04-4725-9140-243236afd26a"),
            Guid.Parse("ff9763ee-b247-48a3-84bd-7099a54a2074"),
            Guid.Parse("fd4bd871-01e0-46fc-bbbd-8b0311659f57"),
            Guid.Parse("842b911c-7000-4714-b615-52a8849d569f"),
            Guid.Parse("a28af4ba-d62c-4a7a-8109-9ff9fac19423"),
            Guid.Parse("ed807edb-f6bb-4dcf-b293-72d5e4262157"),
            Guid.Parse("d3ece0e4-6cb7-4af6-a98b-7bb4169c8b8d"),
            Guid.Parse("92b23f71-e891-4616-8e62-f28c761feb25"),
            Guid.Parse("82f6754f-5d67-4b59-a2c1-936e1d72396d"),
            Guid.Parse("fc66e5ef-d1e8-4f7d-a845-903f93a90387"),
            Guid.Parse("8adb7e81-d14a-45d7-9b5d-dfc222643029"),
            Guid.Parse("fcffdaad-83bb-4ba1-83e9-90caeecf147d"),
            Guid.Parse("133e3479-43b7-484a-8449-0ce31bc82f4c")
        };

        builder.Entity<User>()
            .HasData(userIds.Select(id => new User
            {
                Id = id,
                Email = Faker.Internet.Email(),
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                PasswordSalt = "salt",
                PasswordHash = "hash",
                IsAdmin = false
            }));

        builder.Entity<Ledger>()
            .HasData(userIds.Select(id => new Ledger
            {
                Id = Guid.NewGuid(),
                Currency = Currency.EUR,
                Balance = Faker.RandomNumber.Next(0, 700),
                UserId = id,
                Iban = IbanGenerator.GenerateIban()
            }));
    }
}
