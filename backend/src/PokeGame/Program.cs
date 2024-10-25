using MediatR;
using PokeGame.Infrastructure.Commands;

namespace PokeGame;

internal class Program
{
  public static async Task Main(string[] args)
  {
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    IConfiguration configuration = builder.Configuration;

    Startup startup = new(configuration);
    startup.ConfigureServices(builder.Services);

    WebApplication application = builder.Build();

    startup.Configure(application);

    if (configuration.GetValue<bool>("EnableMigrations"))
    {
      using IServiceScope scope = application.Services.CreateScope();
      IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
      await publisher.Publish(new InitializeDatabaseCommand());
    }

    application.Run();
  }
}
