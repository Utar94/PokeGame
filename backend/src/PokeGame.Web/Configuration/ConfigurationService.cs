using Logitar.Portal.Client;
using Logitar.Portal.Core;
using Logitar.Portal.Core.Users.Models;
using Logitar.Portal.Core.Users.Payloads;
using PokeGame.Web.Models.Api.Configuration;

namespace PokeGame.Web.Configuration
{
  internal class ConfigurationService : IConfigurationService
  {
    private readonly IUserService _userService;

    public ConfigurationService(IUserService userService)
    {
      _userService = userService;
    }

    public async Task InitializeAsync(InitializeConfigurationModel payload, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(payload);

      var createUserPayload = new CreateUserPayload
      {
        Realm = Constants.Realm,
        Username = payload.User.Username,
        Password = payload.User.Password,
        ConfirmEmail = true,
        Email = payload.User.Email,
        FirstName = payload.User.FirstName,
        LastName = payload.User.LastName,
        Locale = payload.User.Locale
      };
      await _userService.CreateAsync(createUserPayload, cancellationToken);
    }

    public async Task<bool> IsInitializedAsync(CancellationToken cancellationToken)
    {
      ListModel<UserSummary> users = await _userService.GetAsync(realm: Constants.Realm, cancellationToken: cancellationToken);

      return users.Items.Any();
    }
  }
}
