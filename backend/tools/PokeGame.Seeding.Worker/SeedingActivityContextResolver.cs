using PokeGame.Application;
using ActivityContext = PokeGame.Application.ActivityContext;

namespace PokeGame.Seeding.Worker;

internal class SeedingActivityContextResolver : IActivityContextResolver
{
  public Task<ActivityContext> ResolveAsync(CancellationToken cancellationToken)
  {
    ActivityContext context = new(Session: null, User: null);
    return Task.FromResult(context);
  }
}
