using PokeGame.Domain.Speciez;

namespace PokeGame.Application.Speciez;

public interface ISpeciesManager
{
  Task SaveAsync(Species species, CancellationToken cancellationToken = default);
}
