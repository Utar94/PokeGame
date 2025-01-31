using Logitar.EventSourcing;
using PokeGame.Domain;
using PokeGame.Domain.Speciez;
using PokeGame.Domain.Speciez.Events;

namespace PokeGame.Application.Speciez;

internal class SpeciesManager : ISpeciesManager
{
  private readonly ISpeciesQuerier _speciesQuerier;
  private readonly ISpeciesRepository _speciesRepository;

  public SpeciesManager(ISpeciesQuerier speciesQuerier, ISpeciesRepository speciesRepository)
  {
    _speciesQuerier = speciesQuerier;
    _speciesRepository = speciesRepository;
  }

  public async Task SaveAsync(Species species, CancellationToken cancellationToken)
  {
    UniqueName? uniqueName = null;
    foreach (IEvent change in species.Changes)
    {
      if (change is SpeciesCreated created)
      {
        uniqueName = created.UniqueName;
      }
      else if (change is SpeciesUpdated updated && updated.UniqueName != null)
      {
        uniqueName = updated.UniqueName;
      }
    }

    if (uniqueName != null)
    {
      SpeciesId? conflictId = await _speciesQuerier.FindIdAsync(uniqueName, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(species.Id))
      {
        throw new UniqueNameAlreadyUsedException(species, conflictId.Value);
      }
    }

    // TODO(fpion): RegionalNumbers

    await _speciesRepository.SaveAsync(species, cancellationToken);
  }
}
