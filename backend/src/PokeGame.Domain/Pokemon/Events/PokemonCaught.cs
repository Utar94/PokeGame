using MediatR;

namespace PokeGame.Domain.Pokemon.Events
{
  public class PokemonCaught : DomainEvent, INotification
  {
    public PokemonCaught(string location, Guid trainerId, byte position, byte? box = null, string? surname = null)
    {
      Box = box;
      Location = location ?? throw new ArgumentNullException(nameof(location));
      Position = position;
      Surname = surname;
      TrainerId = trainerId;
    }

    public string? Surname { get; private set; }

    public string Location { get; private set; }
    public Guid TrainerId { get; private set; }

    public byte Position { get; private set; }
    public byte? Box { get; private set; }
  }
}
