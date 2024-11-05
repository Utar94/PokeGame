using Logitar.Portal.Client;
using PokeGame.Application;
using PokeGame.Application.Logging;
using PokeGame.EntityFrameworkCore.PostgreSQL;
using PokeGame.EntityFrameworkCore.SqlServer;
using PokeGame.Infrastructure;
using PokeGame.Seeding.Worker.Portal;

namespace PokeGame.Seeding.Worker;

internal class Startup
{
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public void ConfigureServices(IServiceCollection services)
  {
    IPortalSettings portalSettings = _configuration.GetSection("Portal").Get<PortalSettings>() ?? new();
    portalSettings = WorkerPortalSettings.Initialize(portalSettings);
    services.AddLogitarPortalClient(portalSettings);

    services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    services.AddHostedService<SeedingWorker>();

    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.EntityFrameworkCoreSqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        services.AddPokeGameWithEntityFrameworkCorePostgreSQL(_configuration);
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        services.AddPokeGameWithEntityFrameworkCoreSqlServer(_configuration);
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    services.AddSingleton<IActivityContextResolver, SeedingActivityContextResolver>();
    services.AddSingleton<ILogRepository, SeedingLogRepository>();
  }
}
