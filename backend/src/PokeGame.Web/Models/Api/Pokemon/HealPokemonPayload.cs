namespace PokeGame.Web.Models.Api.Pokemon
{
  public class HealPokemonPayload
  {
    public short RestoreHitPoints { get; set; }
    public bool RemoveCondition { get; set; }
  }
}
