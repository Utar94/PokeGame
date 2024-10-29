using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Regions;

public class RegionModel : Aggregate
{
  public string Name { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public RegionModel() : this(string.Empty)
  {
  }

  public RegionModel(string name)
  {
    Name = name;
  }

  public override string ToString() => $"{Name} | {base.ToString()}";
}
