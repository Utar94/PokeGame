using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.Domain;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
  public void Configure(EntityTypeBuilder<UserEntity> builder)
  {
    builder.ToTable(PokeGameDb.Users.Table.Table ?? string.Empty, PokeGameDb.Users.Table.Schema);
    builder.HasKey(x => x.UserId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.ActorId).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.EmailAddress);

    builder.Property(x => x.ActorId).HasMaxLength(ActorId.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.EmailAddress).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.PictureUrl).HasMaxLength(Url.MaximumLength);
  }
}
