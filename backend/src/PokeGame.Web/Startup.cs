using Logitar.Portal.Client;
using Microsoft.AspNetCore.Authorization;
using PokeGame.Application;
using PokeGame.Infrastructure;
using PokeGame.Infrastructure.ReadModel;
using PokeGame.Web.Authentication;
using PokeGame.Web.Authorization;
using PokeGame.Web.Configuration;
using PokeGame.Web.Filters;
using PokeGame.Web.Middlewares;
using System.Text.Json.Serialization;

namespace PokeGame.Web
{
  internal class Startup : StartupBase
  {
    private readonly PortalSettings _portalSettings;

    public Startup(IConfiguration configuration)
    {
      _portalSettings = configuration.GetSection("Portal").Get<PortalSettings>() ?? new();
    }

    public override void ConfigureServices(IServiceCollection services)
    {
      base.ConfigureServices(services);

      services
        .AddControllersWithViews(options =>
        {
          options.Filters.Add<BadRequestExceptionFilterAttribute>();
          options.Filters.Add<ConflictExceptionFilterAttribute>();
          options.Filters.Add<ErrorExceptionFilterAttribute>();
          options.Filters.Add<NotFoundExceptionFilterAttribute>();
          options.Filters.Add<ValidationExceptionFilterAttribute>();
        })
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

      services
        .AddAuthentication()
        .AddScheme<SessionAuthenticationOptions, SessionAuthenticationHandler>(Constants.Schemes.Session, options => { });

      services.AddAuthorization(options =>
      {
        options.AddPolicy(Constants.Policies.Administrator, new AuthorizationPolicyBuilder(Constants.Schemes.All)
          .RequireAuthenticatedUser()
          .AddRequirements(new AdministratorAuthorizationRequirement())
          .Build());
        options.AddPolicy(Constants.Policies.AuthenticatedUser, new AuthorizationPolicyBuilder(Constants.Schemes.All)
          .RequireAuthenticatedUser()
          .AddRequirements(new UserAuthorizationRequirement())
          .Build());
      });

      services
        .AddSession(options =>
        {
          options.Cookie.SameSite = SameSiteMode.Strict;
          options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        })
        .AddDistributedMemoryCache();

      services.AddHttpContextAccessor();
      services.AddOpenApi();

      services.AddApplicationInsightsTelemetry();
      services
        .AddHealthChecks()
        .AddDbContextCheck<EventContext>()
        .AddDbContextCheck<ReadContext>();

      services.AddSingleton(_portalSettings);
      services.AddSingleton<IAuthorizationHandler, AdministratorAuthorizationHandler>();
      services.AddSingleton<IAuthorizationHandler, UserAuthorizationHandler>();
      services.AddSingleton<IUserContext, HttpUserContext>();
      services.AddSingleton<UserService>();
      services.AddScoped<IConfigurationService, ConfigurationService>();

      services.AddPokeGameApplication();
      services.AddPokeGameInfrastructure();
      services.AddPokeGameReadModel();

      services.AddPortalClient(_portalSettings);
    }

    public override void Configure(IApplicationBuilder applicationBuilder)
    {
      if (applicationBuilder is WebApplication application)
      {
        if (application.Environment.IsDevelopment())
        {
          application.UseOpenApi();
        }

        application.UseHttpsRedirection();
        application.UseStaticFiles();
        application.UseSession();
        application.UseMiddleware<RenewSession>();
        application.UseAuthentication();
        application.UseAuthorization();
        application.MapControllers();
        application.MapHealthChecks("/health");
      }
    }
  }
}
