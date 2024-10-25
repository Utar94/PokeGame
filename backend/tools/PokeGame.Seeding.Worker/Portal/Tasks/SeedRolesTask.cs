using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Roles;
using Logitar.Portal.Contracts.Search;
using MediatR;

namespace PokeGame.Seeding.Worker.Portal.Tasks;

internal class SeedRolesTask : SeedingTask
{
  public override string? Description => "Seeds the user roles in the Portal.";
}

internal class SeedRolesTaskHandler : INotificationHandler<SeedDictionariesTask>
{
  private readonly ILogger<SeedRolesTaskHandler> _logger;
  private readonly IRoleClient _roles;

  public SeedRolesTaskHandler(ILogger<SeedRolesTaskHandler> logger, IRoleClient roles)
  {
    _logger = logger;
    _roles = roles;
  }

  public async Task Handle(SeedDictionariesTask _, CancellationToken cancellationToken)
  {
    RequestContext context = new(cancellationToken);

    string json = await File.ReadAllTextAsync("Portal/roles.json", Encoding.UTF8, cancellationToken);
    IEnumerable<RoleSummary>? summaries = JsonSerializer.Deserialize<IEnumerable<RoleSummary>>(json);
    if (summaries != null)
    {
      SearchResults<Role> results = await _roles.SearchAsync(new SearchRolesPayload(), context);
      Dictionary<string, Role> roles = new(capacity: results.Items.Count);
      foreach (Role role in results.Items)
      {
        roles[role.UniqueName] = role;
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
          CreateRolePayload payload = new(summary.UniqueName);
          role = await _roles.CreateAsync(payload, context);
          roles[role.UniqueName] = role;
        }

        ReplaceRolePayload replace = new(summary.UniqueName)
        {
          DisplayName = summary.DisplayName,
          Description = summary.Description
        };
        role = await _roles.ReplaceAsync(role.Id, replace, role.Version)
          ?? throw new InvalidOperationException($"The role 'Id={role.Id}' replace result should not be null.");

        _logger.LogInformation("The role '{UniqueName}' has been {Status} (Id={Id}).", role.UniqueName, status, role.Id);
      }
    }
  }
}
