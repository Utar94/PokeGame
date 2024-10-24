using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Abilities;

public class AbilityModel : Aggregate
{
  public AbilityKind? Kind { get; set; }

  public string Name { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public AbilityModel() : this(string.Empty)
  {
  }

  public AbilityModel(string name)
  {
    Name = name;
  }
}
