using Logitar.Portal.Contracts;

namespace PokeGame.Application.Abilities.Models;

public class AbilityModel : AggregateModel
{
  public string UniqueName { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? Link { get; set; }
  public string? Notes { get; set; }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
