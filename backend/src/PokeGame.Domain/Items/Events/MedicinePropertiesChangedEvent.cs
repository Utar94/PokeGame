using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Items.Properties;

namespace PokeGame.Domain.Items.Events;

public class MedicinePropertiesChangedEvent : DomainEvent, INotification
{
  public ReadOnlyMedicineProperties Properties { get; }

  public MedicinePropertiesChangedEvent(ReadOnlyMedicineProperties properties)
  {
    Properties = properties;
  }
}
