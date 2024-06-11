using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using PokeGame.Application;
using PokeGame.EntityFrameworkCore;
using PokeGame.EntityFrameworkCore.SqlServer;
using PokeGame.Extensions;
using PokeGame.Filters;
using PokeGame.Infrastructure;
using PokeGame.Settings;

namespace PokeGame;

internal class Startup : StartupBase
{
  private readonly IConfiguration _configuration;
  private readonly bool _enableOpenApi;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
    _enableOpenApi = _configuration.GetValue<bool>("EnableOpenApi");
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    CorsSettings corsSettings = _configuration.GetSection("Cors").Get<CorsSettings>() ?? new();
    services.AddSingleton(corsSettings);
    services.AddCors(corsSettings);

    OpenAuthenticationSettings openAuthenticationSettings = _configuration.GetSection("OAuth").Get<OpenAuthenticationSettings>() ?? new();
    services.AddSingleton(openAuthenticationSettings);
    // TODO(fpion): Authentication

    // TODO(fpion): Authorization

    CookiesSettings cookiesSettings = _configuration.GetSection("Cookies").Get<CookiesSettings>() ?? new();
    services.AddSingleton(cookiesSettings);
    services.AddSession(options =>
    {
      options.Cookie.SameSite = cookiesSettings.Session.SameSite;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
    services.AddDistributedMemoryCache();

    services.AddControllers(options => options.Filters.Add<ExceptionHandling>()) // TODO(fpion): LoggingFilter
      .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    services.AddTransient<IActivityContextResolver, HttpActivityContextResolver>();

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
        healthChecks.AddDbContextCheck<PokemonContext>();
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (_enableOpenApi)
    {
      builder.UseOpenApi();
    }

    builder.UseHttpsRedirection();
    builder.UseCors();
    builder.UseSession();
    //builder.UseMiddleware<Logging>(); // TODO(fpion): Logging
    //builder.UseMiddleware<RenewSession>(); // TODO(fpion): ession Renewal
    builder.UseAuthentication();
    builder.UseAuthorization();

    if (builder is WebApplication application)
    {
      application.MapControllers();
      application.MapHealthChecks("/health");
    }
  }
}
