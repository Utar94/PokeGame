using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class PokemonTypeEntity : EnumEntity
  {
    public PokemonTypeEntity(PokemonType pokemonType) : base(pokemonType)
    {
    }
    private PokemonTypeEntity() : base()
    {
    }
  }

  internal class PokemonTypeEntityConfiguration : EnumEntityConfiguration<PokemonType, PokemonTypeEntity>
  {
    public override void Configure(EntityTypeBuilder<PokemonTypeEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("PokemonTypes");
    }

    protected override IEnumerable<PokemonTypeEntity> GetData()
      => Enum.GetValues<PokemonType>().Select(value => new PokemonTypeEntity(value));
  }
}
