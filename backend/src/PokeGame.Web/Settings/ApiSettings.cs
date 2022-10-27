using Microsoft.OpenApi.Models;

namespace PokeGame.Web.Settings
{
  public class ApiSettings
  {
    public ContactInfo? Contact { get; set; }
    public string? Description { get; set; }
    public LicenseInfo? License { get; set; }
    public string? Title { get; set; }
  }

  public class ContactInfo
  {
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }

    public OpenApiContact ToOpenApiContact() => new()
    {
      Email = Email,
      Name = Name,
      Url = string.IsNullOrEmpty(Url) ? null : new Uri(Url)
    };
  }

  public class LicenseInfo
  {
    public string? Name { get; set; }
    public string? Url { get; set; }

    public OpenApiLicense ToOpenApiLicense() => new()
    {
      Name = Name,
      Url = string.IsNullOrEmpty(Url) ? null : new Uri(Url)
    };
  }
}
