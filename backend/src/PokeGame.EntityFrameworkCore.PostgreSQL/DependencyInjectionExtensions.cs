using Logitar.EventSourcing.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PokeGame.EntityFrameworkCore.PostgreSQL;

public static class DependencyInjectionExtensions
{
  private const string ConfigurationKey = "POSTGRESQLCONNSTR_PokeGame";

  public static IServiceCollection AddPokeGameWithEntityFrameworkCorePostgreSQL(this IServiceCollection services, IConfiguration configuration)
  {
    string? connectionString = Environment.GetEnvironmentVariable(ConfigurationKey);
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      connectionString = configuration.GetValue<string>(ConfigurationKey);
    }
    if (string.IsNullOrWhiteSpace(connectionString))
    {
      throw new ArgumentException($"The configuration '{ConfigurationKey}' could not be found.", nameof(configuration));
    }
    return services.AddPokeGameWithEntityFrameworkCorePostgreSQL(connectionString.Trim());
  }
  public static IServiceCollection AddPokeGameWithEntityFrameworkCorePostgreSQL(this IServiceCollection services, string connectionString)
  {
    return services
      .AddLogitarEventSourcingWithEntityFrameworkCorePostgreSQL(connectionString)
      .AddPokeGameWithEntityFrameworkCore()
      .AddDbContext<PokeGameContext>(options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly("PokeGame.EntityFrameworkCore.PostgreSQL")))
      .AddSingleton<ISqlHelper, PostgreSQLHelper>();
  }
}
