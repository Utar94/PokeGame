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
using PokeGame.Middlewares;
using PokeGame.MongoDB;
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
    _enableOpenApi = configuration.GetValue<bool>("EnableOpenApi");
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    CorsSettings corsSettings = _configuration.GetSection(CorsSettings.SectionKey).Get<CorsSettings>() ?? new();
    services.AddSingleton(corsSettings);
    services.AddCors(corsSettings);

    OpenAuthenticationSettings openAuthenticationSettings = _configuration.GetSection(OpenAuthenticationSettings.SectionKey).Get<OpenAuthenticationSettings>() ?? new();
    services.AddSingleton(openAuthenticationSettings);
    AuthenticationBuilder authenticationBuilder = services.AddAuthentication()
      .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(Schemes.ApiKey, options => { })
      .AddScheme<BearerAuthenticationOptions, BearerAuthenticationHandler>(Schemes.Bearer, options => { })
      .AddScheme<SessionAuthenticationOptions, SessionAuthenticationHandler>(Schemes.Session, options => { });
    if (_authenticationSchemes.Contains(Schemes.Basic))
    {
      authenticationBuilder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(Schemes.Basic, options => { });
    }
    services.AddSingleton<IOpenAuthenticationService, OpenAuthenticationService>();

    services.AddAuthorizationBuilder()
      .SetDefaultPolicy(new AuthorizationPolicyBuilder(_authenticationSchemes)
        .RequireAuthenticatedUser()
        .Build())
      .AddPolicy(Policies.Administrator, new AuthorizationPolicyBuilder(_authenticationSchemes)
        .RequireAuthenticatedUser()
        .AddRequirements(new RoleAuthorizationRequirement(Roles.Administrator))
        .Build())
      .AddPolicy(Policies.User, new AuthorizationPolicyBuilder(_authenticationSchemes)
        .RequireAuthenticatedUser()
        .AddRequirements(new UserAuthorizationRequirement())
        .Build());
    services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
    services.AddSingleton<IAuthorizationHandler, UserAuthorizationHandler>();

    CookiesSettings cookiesSettings = _configuration.GetSection(CookiesSettings.SectionKey).Get<CookiesSettings>() ?? new();
    services.AddSingleton(cookiesSettings);
    services.AddSession(options =>
    {
      options.Cookie.SameSite = cookiesSettings.Session.SameSite;
      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
    services.AddDistributedMemoryCache();

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
    builder.UseSession();
    builder.UseMiddleware<Logging>();
    builder.UseMiddleware<RenewSession>();
    builder.UseAuthentication();
    builder.UseAuthorization();

    if (builder is WebApplication application)
    {
      application.MapControllers();
      application.MapHealthChecks("/health");
    }
  }
}
