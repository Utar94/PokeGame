namespace PokeGame.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      var startup = new Startup();
      startup.ConfigureServices(builder.Services);

      WebApplication application = builder.Build();

      startup.Configure(application);

      application.Run();
    }
  }
}