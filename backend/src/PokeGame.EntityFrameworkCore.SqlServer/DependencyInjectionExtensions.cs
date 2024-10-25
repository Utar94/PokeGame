using Logitar.EventSourcing.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PokeGame.EntityFrameworkCore.SqlServer;

public static class DependencyInjectionExtensions
{
  private const string ConfigurationKey = "SQLCONNSTR_PokeGame";

  public static IServiceCollection AddPokeGameWithEntityFrameworkCoreSqlServer(this IServiceCollection services, IConfiguration configuration)
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
    return services.AddPokeGameWithEntityFrameworkCoreSqlServer(connectionString.Trim());
  }
  public static IServiceCollection AddPokeGameWithEntityFrameworkCoreSqlServer(this IServiceCollection services, string connectionString)
  {
    return services
      .AddLogitarEventSourcingWithEntityFrameworkCoreSqlServer(connectionString)
      .AddPokeGameWithEntityFrameworkCore()
      .AddDbContext<PokeGameContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("PokeGame.EntityFrameworkCore.SqlServer")))
      .AddSingleton<ISqlHelper, SqlServerHelper>();
  }
}
