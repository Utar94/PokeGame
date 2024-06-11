using PokeGame.Application;

namespace PokeGame;

internal class TestActivityContextResolver : IActivityContextResolver
{
  private readonly TestContext _context;

  public TestActivityContextResolver(TestContext context)
  {
    _context = context;
  }

  public Task<ActivityContext> ResolveAsync(CancellationToken cancellationToken)
  {
    ActivityContext context = new(ApiKey: null, Session: null, _context.User);
    return Task.FromResult(context);
  }
}
