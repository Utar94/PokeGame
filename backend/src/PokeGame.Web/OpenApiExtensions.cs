using Microsoft.OpenApi.Models;
using PokeGame.Web.Settings;

namespace PokeGame.Web
{
  internal static class OpenApiExtensions
  {
    public static IServiceCollection AddOpenApi(this IServiceCollection services, ApiSettings apiSettings, Version version)
    {
      return services.AddSwaggerGen(config =>
      {
        config.SwaggerDoc(name: $"v{(version.Major == 0 ? 1 : version.Major)}", new OpenApiInfo
        {
          Contact = apiSettings.Contact?.ToOpenApiContact(),
          Description = apiSettings.Description,
          License = apiSettings.License?.ToOpenApiLicense(),
          Title = apiSettings.Title,
          Version = $"v{version}"
        });
      });
    }

    public static void UseOpenApi(this WebApplication application, ApiSettings apiSettings, Version version)
    {
      application.UseSwagger();
      application.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", $"{apiSettings.Title} v{version}"));
    }
  }
}
