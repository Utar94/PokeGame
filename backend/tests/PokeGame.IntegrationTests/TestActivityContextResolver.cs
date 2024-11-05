using PokeGame.Application;

namespace PokeGame;

internal class TestActivityContextResolver : IActivityContextResolver
{
  private readonly ActivityContext _activityContext;

  public TestActivityContextResolver(ActivityContext activityContext)
  {
    _activityContext = activityContext;
  }

  public Task<ActivityContext> ResolveAsync(CancellationToken cancellationToken)
  {
    return Task.FromResult(_activityContext);
  }
}
