using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Abilities;

public class Ability : Aggregate
{
  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  public Ability() : this(string.Empty)
  {
  }

  public Ability(string uniqueName)
  {
    UniqueName = uniqueName;
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
