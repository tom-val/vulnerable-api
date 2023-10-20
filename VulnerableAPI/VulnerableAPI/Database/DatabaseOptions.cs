namespace VulnerableAPI.Database;

public record DatabaseOptions
{
    public const string ConfigSection = "Database";

    public string? Port { get; init; }
    public string? Host { get; init; }
    public string? User { get; init; }
    public string? Password { get; init; }

    public string ConnectionString(string database) => $"Host={Host};Port={Port};Database={database};Uid={User};Pwd={Password};Timeout=5;TCP Keepalive=true;";
}
