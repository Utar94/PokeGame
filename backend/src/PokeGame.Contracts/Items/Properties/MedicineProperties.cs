namespace PokeGame.Contracts.Items.Properties;

public record MedicineProperties : IMedicineProperties
{
  public MedicineProperties()
  {
  }

  public MedicineProperties(IMedicineProperties medicine) : this()
  {
  }
}
