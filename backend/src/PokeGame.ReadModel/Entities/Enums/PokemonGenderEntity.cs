using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Pokemon;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class PokemonGenderEntity : EnumEntity
  {
    public PokemonGenderEntity(PokemonGender pokemonGender) : base(pokemonGender)
    {
    }
    private PokemonGenderEntity() : base()
    {
    }
  }

  internal class PokemonGenderEntityConfiguration : EnumEntityConfiguration<PokemonGender, PokemonGenderEntity>
  {
    public override void Configure(EntityTypeBuilder<PokemonGenderEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("PokemonGenders");
    }

    protected override IEnumerable<PokemonGenderEntity> GetData()
      => Enum.GetValues<PokemonGender>().Select(value => new PokemonGenderEntity(value));
  }
}
