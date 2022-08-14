using Microsoft.EntityFrameworkCore;
using PokeGame.Infrastructure;

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
        using var dbContext = scope.ServiceProvider.GetRequiredService<PokeGameDbContext>();
        await dbContext.Database.MigrateAsync();
      }

      application.Run();
    }
  }
}
