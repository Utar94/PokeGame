using Logitar.EventSourcing.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PokeGame.Infrastructure.SqlServer;

public static class DependencyInjectionExtensions
{
  private const string ConnectionStringKey = "SQLCONNSTR_PokeGame";

  public static IServiceCollection AddPokeGameInfrastructureWithSqlServer(this IServiceCollection services, IConfiguration configuration)
  {
    string connectionString = configuration.GetValue<string>(ConnectionStringKey)
      ?? throw new InvalidOperationException($"The configuration '{ConnectionStringKey}' is missing.");

    return services
      .AddLogitarEventSourcingWithEntityFrameworkCoreSqlServer(connectionString)
      .AddDbContext<PokeGameContext>(options => options.UseSqlServer(connectionString,
        builder => builder.MigrationsAssembly("PokeGame.Infrastructure.SqlServer")))
      .AddSingleton<ISqlHelper, SqlServerHelper>();
  }
}
