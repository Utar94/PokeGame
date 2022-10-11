using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Configurations
{
  internal class UserConfiguration : EntityConfiguration<UserEntity>, IEntityTypeConfiguration<UserEntity>
  {
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
      base.Configure(builder);

      builder.HasKey(x => x.Sid);
      builder.HasIndex(x => x.Id).IsUnique();
      builder.HasIndex(x => x.Username).IsUnique();

      builder.Property(x => x.Email).HasMaxLength(256);
      builder.Property(x => x.FullName).HasMaxLength(256);
      builder.Property(x => x.Id).HasDefaultValueSql("uuid_generate_v4()");
      builder.Property(x => x.Locale).HasMaxLength(16);
      builder.Property(x => x.Picture).HasMaxLength(2048);
      builder.Property(x => x.Sid).HasColumnName("UserId");
      builder.Property(x => x.Username).HasMaxLength(256);
    }
  }
}
