using Logitar.Portal.Contracts;

namespace PokeGame.Contracts.Items;

public class Item : Aggregate
{
  public ItemCategory Category { get; set; }
  public int? Price { get; set; }

  public string UniqueName { get; set; }
  public string? DisplayName { get; set; }
  public string? Description { get; set; }
  public string? Picture { get; set; }

  public string? Reference { get; set; }
  public string? Notes { get; set; }

  /* Category: Medicine
   * - HP restore, but lowers friendship, fully
   * - StatusConditions: Burn, Freeze, Paralysis, Poison, Sleep (5/5), any non-volatile status conditions and confusion
   * - PP restore to all moves, to one move, fully
   * - Revive one or all fainted Pokémon, 50% HP or full HP
   */

  /* Category: PokeBall
   * - Catch rate modifier
   */

  public Item() : this(string.Empty)
  {
  }

  public Item(string uniqueName)
  {
    UniqueName = uniqueName;
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
