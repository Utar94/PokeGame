using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Items.Properties;

namespace PokeGame.Domain.Items.Events;

public class PokeBallPropertiesChangedEvent : DomainEvent, INotification
{
  public ReadOnlyPokeBallProperties Properties { get; }

  public PokeBallPropertiesChangedEvent(ReadOnlyPokeBallProperties properties)
  {
    Properties = properties;
  }
}
