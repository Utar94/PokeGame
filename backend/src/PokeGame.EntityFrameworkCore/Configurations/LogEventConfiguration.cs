using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class LogEventConfiguration : IEntityTypeConfiguration<LogEventEntity>
{
  public void Configure(EntityTypeBuilder<LogEventEntity> builder)
  {
    builder.ToTable(nameof(PokemonContext.LogEvents));
    builder.HasKey(x => x.LogEventId);

    builder.HasIndex(x => x.LogId);
    builder.HasIndex(x => x.EventId).IsUnique();

    builder.Property(x => x.EventId).HasMaxLength(EventId.MaximumLength);

    builder.HasOne(x => x.Log).WithMany(x => x.Events).OnDelete(DeleteBehavior.Cascade);
  }
}
