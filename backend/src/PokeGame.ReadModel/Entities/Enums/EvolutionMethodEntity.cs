using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain.Species;
using PokeGame.ReadModel.Configurations;

namespace PokeGame.ReadModel.Entities.Enums
{
  internal class EvolutionMethodEntity : EnumEntity
  {
    public EvolutionMethodEntity(EvolutionMethod evolutionMethod) : base(evolutionMethod)
    {
    }
    private EvolutionMethodEntity() : base()
    {
    }
  }

  internal class EvolutionMethodEntityConfiguration : EnumEntityConfiguration<EvolutionMethod, EvolutionMethodEntity>
  {
    public override void Configure(EntityTypeBuilder<EvolutionMethodEntity> builder)
    {
      base.Configure(builder);

      builder.ToTable("EvolutionMethods");
    }

    protected override IEnumerable<EvolutionMethodEntity> GetData()
      => Enum.GetValues<EvolutionMethod>().Select(value => new EvolutionMethodEntity(value));
  }
}
