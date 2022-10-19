using Microsoft.EntityFrameworkCore;
using PokeGame.Infrastructure;
using PokeGame.ReadModel;

namespace PokeGame.Web
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      var startup = new Startup(builder.Configuration);
      startup.ConfigureServices(builder.Services);

      WebApplication application = builder.Build();

      startup.Configure(application);

      if (builder.Configuration.GetValue<bool>("MigrateDatabase"))
      {
        using IServiceScope scope = application.Services.CreateScope();

        using var eventContext = scope.ServiceProvider.GetRequiredService<EventContext>();
        await eventContext.Database.MigrateAsync();

        using var readContext = scope.ServiceProvider.GetRequiredService<ReadContext>();
        await readContext.Database.MigrateAsync();
      }

      application.Run();
    }
  }
}
