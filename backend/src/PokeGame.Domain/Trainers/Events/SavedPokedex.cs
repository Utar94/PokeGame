using MediatR;

namespace PokeGame.Domain.Trainers.Events
{
  public class SavedPokedex : DomainEvent, INotification
  {
    public SavedPokedex(Guid speciesId, bool hasCaught)
    {
      SpeciesId = speciesId;
      HasCaught = hasCaught;
    }

    public Guid SpeciesId { get; private set; }
    public bool HasCaught { get; private set; }
  }
}
