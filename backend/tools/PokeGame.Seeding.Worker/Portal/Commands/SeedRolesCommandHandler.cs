using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Roles;
using Logitar.Portal.Contracts.Search;
using MediatR;

namespace PokeGame.Seeding.Worker.Portal.Commands;

internal class SeedRolesCommandHandler : INotificationHandler<SeedRolesCommand>
{
  private readonly ILogger<SeedRolesCommandHandler> _logger;
  private readonly IRoleClient _roles;

  public SeedRolesCommandHandler(ILogger<SeedRolesCommandHandler> logger, IRoleClient roleClient)
  {
    _logger = logger;
    _roles = roleClient;
  }

  public async Task Handle(SeedRolesCommand _, CancellationToken cancellationToken)
  {
    RequestContext context = new(cancellationToken);

    string json = await File.ReadAllTextAsync("Portal/roles.json", cancellationToken);
    IEnumerable<RoleSummary>? summaries = JsonSerializer.Deserialize<IEnumerable<RoleSummary>>(json);
    if (summaries != null)
    {
      SearchResults<Role> results = await _roles.SearchAsync(new SearchRolesPayload(), context);
      Dictionary<string, Role> roles = new(capacity: results.Items.Count);
      foreach (Role item in results.Items)
      {
        roles[item.UniqueName] = item;
      }

      foreach (RoleSummary summary in summaries)
      {
        string status = "created";
        if (roles.TryGetValue(summary.UniqueName, out Role? role))
        {
          status = "updated";
        }
        else
        {
          CreateRolePayload createPayload = new(summary.UniqueName);
          role = await _roles.CreateAsync(createPayload, context);
          roles[role.UniqueName] = role;
        }

        ReplaceRolePayload replacePayload = new(role.UniqueName)
        {
          DisplayName = summary.DisplayName,
          Description = summary.Description,
          CustomAttributes = role.CustomAttributes
        };
        role = await _roles.ReplaceAsync(role.Id, replacePayload, role.Version, context)
          ?? throw new InvalidOperationException($"The role 'Id={role.Id}' replace result should not be null.");

        _logger.LogInformation("The role '{UniqueName}' has been {Status} (Id={Id}).", role.UniqueName, status, role.Id);
      }
    }
  }
}
