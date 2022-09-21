using PokeGame.Domain;

namespace PokeGame.Application
{
  public interface IRepository
  {
    Task<T?> LoadAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Aggregate;
    Task<T?> LoadAsync<T>(Guid id, int? version = null, CancellationToken cancellationToken = default) where T : Aggregate;
    Task<IEnumerable<T>> LoadAsync<T>(IEnumerable<Guid> ids, CancellationToken cancellationToken = default) where T : Aggregate;
    Task SaveAsync<T>(T instance, CancellationToken cancellationToken = default) where T : Aggregate;
    Task SaveAsync<T>(IEnumerable<T> instances, CancellationToken cancellationToken = default) where T : Aggregate;
  }
}
