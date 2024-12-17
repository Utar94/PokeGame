using MediatR;
using Microsoft.FeatureManagement;
using PokeGame.Constants;
using PokeGame.Infrastructure.Commands;
using PokeGame.Infrastructure.SqlServer;

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

    services.AddPokeGameInfrastructureWithSqlServer(_configuration);
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

    if (await featureManager.IsEnabledAsync(Features.UseOpenApi))
    {
      application.MapOpenApi();
    }

    application.UseHttpsRedirection();
    application.UseCors();

    application.MapControllers();

    if (await featureManager.IsEnabledAsync(Features.MigrateDatabase))
    {
      using IServiceScope scope = application.Services.CreateScope();
      IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
      await mediator.Publish(new InitializeDatabaseCommand());
    }
  }
}
