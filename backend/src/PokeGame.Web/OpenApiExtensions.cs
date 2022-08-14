using Microsoft.OpenApi.Models;

namespace PokeGame.Web
{
  internal static class OpenApiExtensions
  {
    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
      ArgumentNullException.ThrowIfNull(services);

      return services.AddSwaggerGen(config =>
      {
        config.SwaggerDoc(name: $"v{Constants.Version.Split('.').First()}", new OpenApiInfo
        {
          Contact = new OpenApiContact
          {
            Email = "francispion@hotmail.com",
            Name = "Francis Pion",
            Url = new Uri("https://www.francispion.ca/")
          },
          Description = "Pokémon game management Web API.",
          License = new OpenApiLicense
          {
            Name = "Use under MIT",
            Url = new Uri("https://github.com/Utar94/PokeGame/blob/main/LICENSE")
          },
          Title = "PokéGame API",
          Version = $"v{Constants.Version}"
        });
      });
    }

    public static void UseOpenApi(this WebApplication application)
    {
      application.UseSwagger();
      application.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", $"PokéGame API v{Constants.Version}"));
    }
  }
}
