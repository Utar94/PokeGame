using Logitar.Portal.Contracts.Users;
using PokeGame.Application;
using PokeGame.Domain;
using ActivityContext = PokeGame.Application.ActivityContext;

namespace PokeGame.Seeding.Worker;

internal class SeedingActivityContextResolver : IActivityContextResolver
{
  private const string UserIdKey = nameof(UserId);

  private readonly ActivityContext _context;

  public SeedingActivityContextResolver(IConfiguration configuration)
  {
    string userId = configuration.GetValue<string>(UserIdKey) ?? throw new InvalidOperationException($"The configuration '{UserIdKey}' is required.");
    User user = new()
    {
      Id = new UserId(userId).ToGuid()
    };
    _context = new ActivityContext(Session: null, user);
  }

  public Task<ActivityContext> ResolveAsync(CancellationToken cancellationToken)
  {
    return Task.FromResult(_context);
  }
}
