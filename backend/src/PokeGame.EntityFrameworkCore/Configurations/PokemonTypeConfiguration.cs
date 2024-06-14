using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Contracts;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class PokemonTypeConfiguration : EnumConfiguration<PokemonTypeEntity>, IEntityTypeConfiguration<PokemonTypeEntity>
{
  public override void Configure(EntityTypeBuilder<PokemonTypeEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(nameof(PokemonContext.PokemonTypes));

    builder.Property(x => x.Id).HasColumnName(PokemonDb.PokemonTypes.PokemonTypeId.Name);
  }

  protected override IEnumerable<PokemonTypeEntity> GetData()
  {
    return Enum.GetValues<PokemonType>().Select(value => new PokemonTypeEntity(value));
  }
}
