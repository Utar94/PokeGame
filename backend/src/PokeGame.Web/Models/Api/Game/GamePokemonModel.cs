using PokeGame.Application.Pokemon.Models;
using PokeGame.Application.Species.Models;
using PokeGame.Domain.Pokemon;

namespace PokeGame.Web.Models.Api.Game
{
  /// <summary>
  /// TODO(fpion): handle Egg Pokémon
  /// </summary>
  public class GamePokemonModel
  {
    public GamePokemonModel(PokemonModel pokemon)
    {
      Id = pokemon.Id;

      SpeciesModel species = pokemon.Species ?? throw new ArgumentException($"The {nameof(pokemon.Species)} is required.", nameof(pokemon));

      Picture = species.Picture;

      Name = pokemon.Surname ?? species.Name;
      Gender = pokemon.Gender;

      Level = pokemon.Level;
      CurrentHitPoints = pokemon.CurrentHitPoints;
      MaximumHitPoints = pokemon.MaximumHitPoints;

      Box = pokemon.Box;
      Position = pokemon.Position ?? throw new ArgumentException($"The {nameof(pokemon.Position)} is required.", nameof(pokemon));
    }

    public Guid Id { get; set; }

    public string? Picture { get; set; }

    public string Name { get; set; }
    public PokemonGender Gender { get; set; }

    public byte Level { get; set; }
    public ushort CurrentHitPoints { get; set; }
    public ushort MaximumHitPoints { get; set; }

    public byte? Box { get; set; }
    public byte Position { get; set; }
  }
}
