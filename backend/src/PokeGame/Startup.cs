using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Portal.Client;
using PokeGame.Application;
using PokeGame.EntityFrameworkCore;
using PokeGame.EntityFrameworkCore.SqlServer;
using PokeGame.Extensions;
using PokeGame.Filters;
using PokeGame.Infrastructure;
using PokeGame.Middlewares;
using PokeGame.MongoDB;
using PokeGame.Settings;

namespace PokeGame;

internal class Startup : StartupBase
{
  private readonly IConfiguration _configuration;
  private readonly bool _enableOpenApi;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
    _enableOpenApi = configuration.GetValue<bool>("EnableOpenApi");
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    CorsSettings corsSettings = _configuration.GetSection(CorsSettings.SectionKey).Get<CorsSettings>() ?? new();
    services.AddSingleton(corsSettings);
    services.AddCors(corsSettings);

    CookiesSettings cookiesSettings = _configuration.GetSection(CookiesSettings.SectionKey).Get<CookiesSettings>() ?? new();
    services.AddSingleton(cookiesSettings);

    services.AddControllers(options =>
    {
      options.Filters.Add<ExceptionHandling>();
      options.Filters.Add<OperationLogging>();
    }).AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    services.AddSingleton<IActivityContextResolver, HttpActivityContextResolver>();

    services.AddApplicationInsightsTelemetry();
    IHealthChecksBuilder healthChecks = services.AddHealthChecks();

    if (_enableOpenApi)
    {
      services.AddOpenApi();
    }

    DatabaseProvider databaseProvider = _configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.EntityFrameworkCoreSqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        services.AddPokeGameWithEntityFrameworkCoreSqlServer(_configuration);
        healthChecks.AddDbContextCheck<EventContext>();
        healthChecks.AddDbContextCheck<PokeGameContext>();
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }
    services.AddPokeGameWithMongoDB(_configuration);

    services.AddLogitarPortalClient(_configuration);
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (_enableOpenApi)
    {
      builder.UseOpenApi();
    }

    builder.UseHttpsRedirection();
    builder.UseCors();
    builder.UseMiddleware<Logging>();

    if (builder is WebApplication application)
    {
      application.MapControllers();
      application.MapHealthChecks("/health");
    }
  }
}
