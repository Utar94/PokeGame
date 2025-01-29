using MediatR;
using Microsoft.FeatureManagement;
using PokeGame.Application;
using PokeGame.Constants;
using PokeGame.Infrastructure;
using PokeGame.Infrastructure.Commands;
using PokeGame.Infrastructure.SqlServer;
using Scalar.AspNetCore;

namespace PokeGame;

internal class Startup : StartupBase
{
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public override void ConfigureServices(IServiceCollection services)
  {
    base.ConfigureServices(services);

    services.AddControllers()
      .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    services.AddCors();

    services.AddFeatureManagement();

    services.AddOpenApi();

    services.AddPokeGameApplication();
    services.AddPokeGameInfrastructure();
    services.AddPokeGameInfrastructureWithSqlServer(_configuration);
    services.AddSingleton<IApplicationContext, HttpApplicationContext>();
  }

  public override void Configure(IApplicationBuilder builder)
  {
    if (builder is WebApplication application)
    {
      ConfigureAsync(application).Wait();
    }
  }
  public virtual async Task ConfigureAsync(WebApplication application)
  {
    IFeatureManager featureManager = application.Services.GetRequiredService<IFeatureManager>();

    if (await featureManager.IsEnabledAsync(Features.UseScalarUI))
    {
      application.MapOpenApi();
      application.MapScalarApiReference();
    }

    application.UseHttpsRedirection();
    application.UseCors();

    application.MapControllers();

    if (await featureManager.IsEnabledAsync(Features.MigrateDatabase))
    {
      using IServiceScope scope = application.Services.CreateScope();
      IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      await mediator.Publish(new MigrateDatabaseCommand());
    }
  }
}
