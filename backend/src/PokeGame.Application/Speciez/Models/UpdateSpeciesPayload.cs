using Logitar.Portal.Contracts;
using PokeGame.Domain;

namespace PokeGame.Application.Speciez.Models;

public record UpdateSpeciesPayload
{
  public string? UniqueName { get; set; } = string.Empty;
  public ChangeModel<string>? DisplayName { get; set; }

  public GrowthRate? GrowthRate { get; set; }
  public int? BaseFriendship { get; set; }
  public int? CatchRate { get; set; }

  public List<RegionalNumberUpdate> RegionalNumbers { get; set; } = [];

  public ChangeModel<string>? Link { get; set; }
  public ChangeModel<string>? Notes { get; set; }
}
