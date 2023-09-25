namespace VulnerableAPI.Options;

public class SqliteOptions
{
    public const string ConfigSection = "Sqlite";
    public string DatabasesLocation { get; init; } = string.Empty;
}
