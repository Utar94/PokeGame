using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Abilities;

public class AbilityModel : Aggregate
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public AbilityModel() : this(string.Empty)
  {
  }

  public AbilityModel(string uniqueName)
  {
    UniqueName = uniqueName;
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
