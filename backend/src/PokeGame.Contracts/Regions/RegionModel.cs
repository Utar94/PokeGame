using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Regions;

public class RegionModel : Aggregate
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public RegionModel() : this(string.Empty)
  {
  }

  public RegionModel(string uniqueName)
  {
    UniqueName = uniqueName;
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
