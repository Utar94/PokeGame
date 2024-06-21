using Logitar.Portal.Contracts;
using PokeGame.Contracts.Items.Properties;

namespace PokeGame.Contracts.Items;

public class Item : Aggregate
{
  public ItemCategory Category { get; set; }
  public int? Price { get; set; }

  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }
  public string? Picture { get; set; }

  public MedicineProperties? Medicine { get; set; }
  public PokeBallProperties? PokeBall { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  public Item() : this(string.Empty)
  {
  }

  public Item(string uniqueName)
  {
    UniqueName = uniqueName;
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
