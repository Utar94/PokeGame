using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Portal.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using PokeGame.Application;
using PokeGame.Authentication;
using PokeGame.Authorization;
using PokeGame.Constants;
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
  private readonly string[] _authenticationSchemes;
  private readonly bool _enableOpenApi;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
    _authenticationSchemes = Schemes.GetEnabled(configuration);
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
    AuthenticationBuilder authenticationBuilder = services.AddAuthentication();
    //  .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(Schemes.ApiKey, options => { }) // TODO(fpion): API Key Authentication
    //  .AddScheme<BearerAuthenticationOptions, BearerAuthenticationHandler>(Schemes.Bearer, options => { }) // TODO(fpion): Bearer Authentication
    //  .AddScheme<SessionAuthenticationOptions, SessionAuthenticationHandler>(Schemes.Session, options => { }); // TODO(fpion): Session Authentication
    if (_authenticationSchemes.Contains(Schemes.Basic))
    {
      authenticationBuilder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(Schemes.Basic, options => { });
    }

    services.AddAuthorizationBuilder()
      .SetDefaultPolicy(new AuthorizationPolicyBuilder(_authenticationSchemes).RequireAuthenticatedUser().Build())
      .AddPolicy(Policies.Gamemaster, new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddRequirements(new RoleAuthorizationRequirement("gamemaster"))
        .Build());
    services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();

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
    builder.UseSession();
    //builder.UseMiddleware<Logging>(); // TODO(fpion): Logging
    //builder.UseMiddleware<RenewSession>(); // TODO(fpion): Session Renewal
    builder.UseAuthentication();
    builder.UseAuthorization();

    if (builder is WebApplication application)
    {
      application.MapControllers();
      application.MapHealthChecks("/health");
    }
  }
}
