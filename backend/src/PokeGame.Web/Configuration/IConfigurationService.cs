using PokeGame.Web.Models.Api.Configuration;

namespace PokeGame.Web.Configuration
{
  public interface IConfigurationService
  {
    Task InitializeAsync(InitializeConfigurationModel payload, CancellationToken cancellationToken = default);
    Task<bool> IsInitializedAsync(CancellationToken cancellationToken = default);
  }
}
