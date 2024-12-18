﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PokeGame.Infrastructure;

#nullable disable

namespace PokeGame.Infrastructure.SqlServer.Migrations
{
    [DbContext(typeof(PokeGameContext))]
    [Migration("20241218000524_CreateMoveAndSpeciesTables")]
    partial class CreateMoveAndSpeciesTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PokeGame.Infrastructure.Entities.AbilityEntity", b =>
                {
                    b.Property<int>("AbilityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AbilityId"));

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Link")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreamId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UniqueNameNormalized")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("AbilityId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CreatedOn");

                    b.HasIndex("DisplayName");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("StreamId")
                        .IsUnique();

                    b.HasIndex("UniqueName");

                    b.HasIndex("UniqueNameNormalized")
                        .IsUnique();

                    b.HasIndex("UpdatedBy");

                    b.HasIndex("UpdatedOn");

                    b.HasIndex("Version");

                    b.ToTable("Abilities", (string)null);
                });

            modelBuilder.Entity("PokeGame.Infrastructure.Entities.MoveEntity", b =>
                {
                    b.Property<int>("MoveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MoveId"));

                    b.Property<byte?>("Accuracy")
                        .HasColumnType("tinyint");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Link")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte?>("Power")
                        .HasColumnType("tinyint");

                    b.Property<byte>("PowerPoints")
                        .HasColumnType("tinyint");

                    b.Property<string>("StatisticChanges")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StatusChance")
                        .HasColumnType("int");

                    b.Property<string>("StatusCondition")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("StreamId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UniqueNameNormalized")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.Property<string>("VolatileConditions")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MoveId");

                    b.HasIndex("Accuracy");

                    b.HasIndex("Category");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CreatedOn");

                    b.HasIndex("DisplayName");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Power");

                    b.HasIndex("PowerPoints");

                    b.HasIndex("StatusCondition");

                    b.HasIndex("StreamId")
                        .IsUnique();

                    b.HasIndex("Type");

                    b.HasIndex("UniqueName");

                    b.HasIndex("UniqueNameNormalized")
                        .IsUnique();

                    b.HasIndex("UpdatedBy");

                    b.HasIndex("UpdatedOn");

                    b.HasIndex("Version");

                    b.ToTable("Moves", (string)null);
                });

            modelBuilder.Entity("PokeGame.Infrastructure.Entities.RegionEntity", b =>
                {
                    b.Property<int>("RegionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegionId"));

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Link")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreamId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UniqueNameNormalized")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("RegionId");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CreatedOn");

                    b.HasIndex("DisplayName");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("StreamId")
                        .IsUnique();

                    b.HasIndex("UniqueName");

                    b.HasIndex("UniqueNameNormalized")
                        .IsUnique();

                    b.HasIndex("UpdatedBy");

                    b.HasIndex("UpdatedOn");

                    b.HasIndex("Version");

                    b.ToTable("Regions", (string)null);
                });

            modelBuilder.Entity("PokeGame.Infrastructure.Entities.RegionalSpeciesEntity", b =>
                {
                    b.Property<int>("SpeciesId")
                        .HasColumnType("int");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("SpeciesId", "RegionId");

                    b.HasIndex("RegionId", "Number")
                        .IsUnique();

                    b.ToTable("RegionalSpecies", (string)null);
                });

            modelBuilder.Entity("PokeGame.Infrastructure.Entities.SpeciesEntity", b =>
                {
                    b.Property<int>("SpeciesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SpeciesId"));

                    b.Property<byte>("BaseFriendship")
                        .HasColumnType("tinyint");

                    b.Property<byte>("CatchRate")
                        .HasColumnType("tinyint");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("GrowthRate")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Link")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("StreamId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UniqueNameNormalized")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("SpeciesId");

                    b.HasIndex("Category");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("CreatedOn");

                    b.HasIndex("DisplayName");

                    b.HasIndex("GrowthRate");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("StreamId")
                        .IsUnique();

                    b.HasIndex("UniqueName");

                    b.HasIndex("UniqueNameNormalized")
                        .IsUnique();

                    b.HasIndex("UpdatedBy");

                    b.HasIndex("UpdatedOn");

                    b.HasIndex("Version");

                    b.ToTable("Species", (string)null);
                });

            modelBuilder.Entity("PokeGame.Infrastructure.Entities.UserEntity", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("ActorId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PictureUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.HasKey("UserId");

                    b.HasIndex("ActorId")
                        .IsUnique();

                    b.HasIndex("DisplayName");

                    b.HasIndex("EmailAddress");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("IsDeleted");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("PokeGame.Infrastructure.Entities.RegionalSpeciesEntity", b =>
                {
                    b.HasOne("PokeGame.Infrastructure.Entities.RegionEntity", "Region")
                        .WithMany("RegionalSpecies")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PokeGame.Infrastructure.Entities.SpeciesEntity", "Species")
                        .WithMany("RegionalSpecies")
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");

                    b.Navigation("Species");
                });

            modelBuilder.Entity("PokeGame.Infrastructure.Entities.RegionEntity", b =>
                {
                    b.Navigation("RegionalSpecies");
                });

            modelBuilder.Entity("PokeGame.Infrastructure.Entities.SpeciesEntity", b =>
                {
                    b.Navigation("RegionalSpecies");
                });
#pragma warning restore 612, 618
        }
    }
}
