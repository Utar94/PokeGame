using System.Text;

namespace PokeGame.Application.Pokemon
{
  public class PokemonNotFoundException : Exception
  {
    public PokemonNotFoundException(IEnumerable<Guid> ids)
      : base(GetMessage(ids))
    {
      Data["Ids"] = ids;
    }

    private static string GetMessage(IEnumerable<Guid> ids)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified Pokémon could not be found.");
      message.AppendLine($"Ids: {string.Join(", ", ids)}");

      return message.ToString();
    }
  }
}
