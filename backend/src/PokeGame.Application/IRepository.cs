using PokeGame.Domain;

namespace PokeGame.Application
{
  public interface IRepository<T> where T : Aggregate
  {
    Task<T?> LoadAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T?> LoadAsync(Guid id, int? version = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> LoadAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task SaveAsync(T instance, CancellationToken cancellationToken = default);
    Task SaveAsync(IEnumerable<T> instances, CancellationToken cancellationToken = default);
  }
}
