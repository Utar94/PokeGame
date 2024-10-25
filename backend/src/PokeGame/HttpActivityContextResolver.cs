using PokeGame.Application;
using PokeGame.Extensions;

namespace PokeGame;

internal class HttpActivityContextResolver : IActivityContextResolver
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  protected HttpContext Context => _httpContextAccessor.HttpContext ?? throw new InvalidOperationException($"The {nameof(_httpContextAccessor.HttpContext)} is required.");

  public HttpActivityContextResolver(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public Task<ActivityContext> ResolveAsync(CancellationToken cancellationToken)
  {
    ActivityContext context = new(Context.GetSession(), Context.GetUser());
    return Task.FromResult(context);
  }
}
