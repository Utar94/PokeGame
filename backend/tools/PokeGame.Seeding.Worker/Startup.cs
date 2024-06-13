using Logitar.Portal.Client;
using PokeGame.Seeding.Worker.Pokemon;
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
    PokemonSettings pokemonSettings = _configuration.GetSection("Pokemon").Get<PokemonSettings>() ?? new();
    services.AddSingleton(pokemonSettings);

    services.AddHttpClient();
    services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    services.AddSingleton<IPokemonClient, PokemonClient>();

    IPortalSettings portalSettings = _configuration.GetSection("Portal").Get<PortalSettings>() ?? new();
    portalSettings = WorkerPortalSettings.Initialize(portalSettings);
    services.AddLogitarPortalClient(portalSettings);

    services.AddHostedService<Worker>();
  }
}
