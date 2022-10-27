using Logitar.Portal.Client;
using Logitar.Portal.Core;
using Logitar.Portal.Core.Users.Models;
using Logitar.Portal.Core.Users.Payloads;
using PokeGame.Web.Models.Api.Configuration;
using PokeGame.Web.Settings;

namespace PokeGame.Web.Configuration
{
  internal class ConfigurationService : IConfigurationService
  {
    private readonly ClientPortalSettings _portalSettings;
    private readonly IUserService _userService;

    public ConfigurationService(ClientPortalSettings portalSettings, IUserService userService)
    {
      _portalSettings = portalSettings;
      _userService = userService;
    }

    public async Task<UserModel> InitializeAsync(InitializeConfigurationModel payload, CancellationToken cancellationToken)
    {
      var createUserPayload = new CreateUserPayload
      {
        Realm = _portalSettings.Realm,
        Username = payload.User.Username,
        Password = payload.User.Password,
        ConfirmEmail = true,
        Email = payload.User.Email,
        FirstName = payload.User.FirstName,
        LastName = payload.User.LastName,
        Locale = payload.User.Locale
      };
      return await _userService.CreateAsync(createUserPayload, cancellationToken);
    }

    public async Task<bool> IsInitializedAsync(CancellationToken cancellationToken)
    {
      ListModel<UserSummary> users = await _userService.GetAsync(realm: _portalSettings.Realm, cancellationToken: cancellationToken);

      return users.Items.Any();
    }
  }
}
