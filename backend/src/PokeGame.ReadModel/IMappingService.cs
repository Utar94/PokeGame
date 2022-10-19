using PokeGame.Application.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel
{
  internal interface IMappingService
  {
    Task<T?> MapAsync<T>(object? value, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> MapAsync<T>(IEnumerable<Entity> entities, CancellationToken cancellationToken = default) where T : AggregateModel;
  }
}
