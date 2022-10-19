using System.Text;

namespace PokeGame.Application.Pokemon
{
  public class PokemonCannotEvolveException : Exception
  {
    public PokemonCannotEvolveException(Domain.Pokemon.Pokemon pokemon, Guid speciesId, IEnumerable<string>? errors = null)
      : base(GetMessage(pokemon, speciesId, errors))
    {
      Data["PokemonId"] = pokemon.Id;
      Data["SpeciesId"] = speciesId;

      if (errors != null)
      {
        Data["Errors"] = errors;
      }
    }

    private static string GetMessage(Domain.Pokemon.Pokemon pokemon, Guid speciesId, IEnumerable<string>? errors)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified Pokémon cannot evolve into the specified species.");
      message.AppendLine($"Pokémon: {pokemon}");
      message.AppendLine($"SpeciesId: {speciesId}");

      if (errors?.Any() == true)
      {
        message.AppendLine("Errors:");
        foreach (string error in errors)
        {
          message.AppendLine($"- {error}");
        }
      }

      return message.ToString();
    }
  }
}
