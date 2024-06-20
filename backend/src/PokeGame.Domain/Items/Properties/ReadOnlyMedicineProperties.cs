using PokeGame.Contracts.Items.Properties;

namespace PokeGame.Domain.Items.Properties;

public record ReadOnlyMedicineProperties : ItemProperties, IMedicineProperties
{
  public ReadOnlyMedicineProperties()
  {
  }

  public ReadOnlyMedicineProperties(IMedicineProperties _) : this()
  {
    // TODO(fpion): implement
  }
}
