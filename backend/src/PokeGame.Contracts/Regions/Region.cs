using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Regions;

public class Region : Aggregate
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  public Region() : this(string.Empty)
  {
  }

  public Region(string uniqueName)
  {
    UniqueName = uniqueName;
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
