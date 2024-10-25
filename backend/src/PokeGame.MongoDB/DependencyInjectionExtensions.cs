using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using PokeGame.Application.Logging;
using PokeGame.Infrastructure;
using PokeGame.MongoDB.Repositories;
using PokeGame.MongoDB.Settings;

namespace PokeGame.MongoDB;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddPokeGameWithMongoDB(this IServiceCollection services, IConfiguration configuration)
  {
    MongoDBSettings settings = configuration.GetSection(MongoDBSettings.SectionKey).Get<MongoDBSettings>() ?? new();
    if (!string.IsNullOrWhiteSpace(settings.ConnectionString) && !string.IsNullOrWhiteSpace(settings.DatabaseName))
    {
      MongoClient client = new(settings.ConnectionString.Trim());
      IMongoDatabase database = client.GetDatabase(settings.DatabaseName.Trim());
      services.AddSingleton(database).AddTransient<ILogRepository, LogRepository>();
    }

    return services.AddPokeGameInfrastructure();
  }
}
