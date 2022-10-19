using Logitar.Portal.Core.Users.Models;
using PokeGame.Web.Models.Api.Configuration;

namespace PokeGame.Web.Configuration
{
  public interface IConfigurationService
  {
    Task<UserModel> InitializeAsync(InitializeConfigurationModel payload, CancellationToken cancellationToken = default);
    Task<bool> IsInitializedAsync(CancellationToken cancellationToken = default);
  }
}
