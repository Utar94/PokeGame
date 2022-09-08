using MediatR;

namespace PokeGame.Domain.Trainers.Events
{
  public class RemovedPokedex : DomainEvent, INotification
  {
    public RemovedPokedex(Guid speciesId)
    {
      SpeciesId = speciesId;
    }

    public Guid SpeciesId { get; private set; }
  }
}
