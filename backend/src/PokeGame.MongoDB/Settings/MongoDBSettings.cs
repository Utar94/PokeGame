namespace PokeGame.MongoDB.Settings;

internal record MongoDBSettings
{
  public const string SectionKey = "MongoDB";

  public string ConnectionString { get; set; }
  public string DatabaseName { get; set; }

  public MongoDBSettings() : this(string.Empty, string.Empty)
  {
  }

  public MongoDBSettings(string connectionString, string databaseName)
  {
    ConnectionString = connectionString;
    DatabaseName = databaseName;
  }
}
