namespace PokeGame.Web
{
  internal class Startup : StartupBase
  {
    public override void ConfigureServices(IServiceCollection services)
    {
      base.ConfigureServices(services);

      services.AddControllers();
      services.AddOpenApi();
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
        application.UseAuthorization();
        application.MapControllers();
      }
    }
  }
}
