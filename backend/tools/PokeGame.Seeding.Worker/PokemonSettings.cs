using Logitar.Net.Http;
using Logitar.Portal.Client;

namespace PokeGame.Seeding.Worker;

internal record PokemonSettings
{
  public string? BaseUrl { get; set; }
  public BasicCredentials? Basic { get; set; }

  public IHttpApiSettings ToHttpApiSettings()
  {
    HttpApiSettings settings = new();

    if (!string.IsNullOrWhiteSpace(BaseUrl))
    {
      settings.BaseUri = new Uri(BaseUrl.Trim(), UriKind.Absolute);
    }

    if (Basic != null && !string.IsNullOrWhiteSpace(Basic.Username) && !string.IsNullOrWhiteSpace(Basic.Password))
    {
      settings.Authorization = HttpAuthorization.Basic(Basic.Username.Trim(), Basic.Password.Trim());
    }

    return settings;
  }
}
