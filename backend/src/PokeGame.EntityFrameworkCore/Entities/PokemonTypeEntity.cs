using PokeGame.Contracts;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class PokemonTypeEntity : EnumEntity
{
  public PokemonTypeEntity(PokemonType pokemonType) : base(pokemonType)
  {
  }

  private PokemonTypeEntity() : base()
  {
  }
}
