using Logitar.Identity.Contracts;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Roles;
using Logitar.Portal.Contracts.Search;
using Logitar.Portal.Contracts.Users;
using MediatR;
using PokeGame.Application.Accounts;
using PokeGame.Contracts.Accounts;

namespace PokeGame.Seeding.Worker.Portal.Commands;

internal class SeedUsersCommandHandler : INotificationHandler<SeedUsersCommand>
{
  private readonly ILogger<SeedUsersCommandHandler> _logger;
  private readonly IUserClient _users;

  public SeedUsersCommandHandler(ILogger<SeedUsersCommandHandler> logger, IUserClient users)
  {
    _logger = logger;
    _users = users;
  }

  public async Task Handle(SeedUsersCommand _, CancellationToken cancellationToken)
  {
    RequestContext context = new(cancellationToken);

    string json = await File.ReadAllTextAsync("Portal/users.json", Encoding.UTF8, cancellationToken);
    IEnumerable<UserSummary>? summaries = JsonSerializer.Deserialize<IEnumerable<UserSummary>>(json);
    if (summaries != null)
    {
      SearchResults<User> results = await _users.SearchAsync(new SearchUsersPayload(), context);
      Dictionary<string, User> users = new(capacity: results.Items.Count);
      foreach (User item in results.Items)
      {
        users[item.UniqueName] = item;
      }

      foreach (UserSummary summary in summaries)
      {
        string status = "created";
        if (users.TryGetValue(summary.EmailAddress, out User? user))
        {
          status = "updated";
        }
        else
        {
          CreateUserPayload createPayload = new(summary.EmailAddress);
          user = await _users.CreateAsync(createPayload, context);
          users[user.UniqueName] = user;
        }

        UpdateUserPayload updatePayload = new()
        {
          Password = new ChangePasswordPayload(summary.Password),
          Email = new Modification<EmailPayload>(new EmailPayload(summary.EmailAddress, isVerified: true)),
          FirstName = new Modification<string>(summary.FirstName),
          MiddleName = new Modification<string>(summary.IsGamemaster ? "(Master)" : "(Player)"),
          LastName = new Modification<string>(summary.LastName),
          Locale = new Modification<string>(summary.Locale),
          TimeZone = new Modification<string>(summary.TimeZone),
          Picture = new Modification<string>(summary.Picture)
        };
        updatePayload.SetMultiFactorAuthenticationMode(MultiFactorAuthenticationMode.None);
        if (!user.IsProfileCompleted())
        {
          updatePayload.CompleteProfile();
        }
        updatePayload.Roles.Add(new RoleModification("gamemaster", summary.IsGamemaster ? CollectionAction.Add : CollectionAction.Remove));
        user = await _users.UpdateAsync(user.Id, updatePayload, context)
          ?? throw new InvalidOperationException($"The user 'Id={user.Id}' update result should not be null.");

        _logger.LogInformation("The user '{UniqueName}' has been {Status} (Id={Id}).", user.UniqueName, status, user.Id);
      }
    }
  }
}
