using PokeGame.Core.Models;

namespace PokeGame.Core
{
  public interface IMappingService
  {
    Task<T> MapAsync<T>(object source, CancellationToken cancellationToken = default);
    Task<ListModel<TDestination>> MapAsync<TSource, TDestination>(PagedList<TSource> list, CancellationToken cancellationToken = default);
  }
}
