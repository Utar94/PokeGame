using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Infrastructure.Entities;

namespace PokeGame.Infrastructure.Configurations
{
  internal class EventConfiguration : IEntityTypeConfiguration<Event>
  {
    public virtual void Configure(EntityTypeBuilder<Event> builder)
    {
      builder.HasKey(x => x.Sid);
      builder.HasIndex(x => x.Id).IsUnique();
      builder.HasIndex(x => new { x.AggregateType, x.AggregateId });

      builder.Property(x => x.AggregateType).HasMaxLength(256);
      builder.Property(x => x.EventData).HasColumnType("jsonb");
      builder.Property(x => x.EventType).HasMaxLength(256);
      builder.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
      builder.Property(x => x.OccurredOn).HasDefaultValueSql("now()");
      builder.Property(x => x.UserId).HasDefaultValue(Guid.Empty);
      builder.Property(x => x.Version).HasDefaultValue(0);
    }
  }
}
