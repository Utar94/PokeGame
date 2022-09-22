using MediatR;

namespace PokeGame.Domain.Species.Events
{
  public class SpeciesEvolutionRemoved : DomainEvent, INotification
  {
    public SpeciesEvolutionRemoved(Guid speciesId)
    {
      SpeciesId = speciesId;
    }

    public Guid SpeciesId { get; }
  }
}
