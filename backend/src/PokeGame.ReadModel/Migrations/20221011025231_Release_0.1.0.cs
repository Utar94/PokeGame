using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class Release_010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ReadModel");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Abilities",
                schema: "ReadModel",
                columns: table => new
                {
                    AbilityId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.AbilityId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "ReadModel",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    DefaultModifier = table.Column<double>(type: "double precision", nullable: true),
                    Price = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Picture = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Reference = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "Moves",
                schema: "ReadModel",
                columns: table => new
                {
                    MoveId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Category = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Accuracy = table.Column<byte>(type: "smallint", nullable: true),
                    Power = table.Column<byte>(type: "smallint", nullable: true),
                    PowerPoints = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    StatusCondition = table.Column<int>(type: "integer", nullable: true),
                    StatusChance = table.Column<byte>(type: "smallint", nullable: true),
                    StatisticStages = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AccuracyStage = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    EvasionStage = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    VolatileConditions = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.MoveId);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                schema: "ReadModel",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    PrimaryType = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    SecondaryType = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    GenderRatio = table.Column<double>(type: "double precision", nullable: true),
                    Height = table.Column<double>(type: "double precision", nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    BaseExperienceYield = table.Column<int>(type: "integer", nullable: true),
                    BaseFriendship = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    CatchRate = table.Column<byte>(type: "smallint", nullable: true),
                    LevelingRate = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    EggCycles = table.Column<byte>(type: "smallint", nullable: true),
                    BaseStatistics = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EvYield = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Picture = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Reference = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.SpeciesId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "ReadModel",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Locale = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Picture = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Evolutions",
                schema: "ReadModel",
                columns: table => new
                {
                    EvolvingSpeciesId = table.Column<int>(type: "integer", nullable: false),
                    EvolvedSpeciesId = table.Column<int>(type: "integer", nullable: false),
                    Method = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    HighFriendship = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ItemId = table.Column<int>(type: "integer", nullable: true),
                    Level = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MoveId = table.Column<int>(type: "integer", nullable: true),
                    Region = table.Column<int>(type: "integer", nullable: true),
                    TimeOfDay = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evolutions", x => new { x.EvolvingSpeciesId, x.EvolvedSpeciesId });
                    table.ForeignKey(
                        name: "FK_Evolutions_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "ReadModel",
                        principalTable: "Items",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_Evolutions_Moves_MoveId",
                        column: x => x.MoveId,
                        principalSchema: "ReadModel",
                        principalTable: "Moves",
                        principalColumn: "MoveId");
                    table.ForeignKey(
                        name: "FK_Evolutions_Species_EvolvedSpeciesId",
                        column: x => x.EvolvedSpeciesId,
                        principalSchema: "ReadModel",
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evolutions_Species_EvolvingSpeciesId",
                        column: x => x.EvolvingSpeciesId,
                        principalSchema: "ReadModel",
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegionalSpecies",
                schema: "ReadModel",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "integer", nullable: false),
                    Region = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalSpecies", x => new { x.SpeciesId, x.Region });
                    table.ForeignKey(
                        name: "FK_RegionalSpecies_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalSchema: "ReadModel",
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesAbilities",
                schema: "ReadModel",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "integer", nullable: false),
                    AbilityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesAbilities", x => new { x.SpeciesId, x.AbilityId });
                    table.ForeignKey(
                        name: "FK_SpeciesAbilities_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "ReadModel",
                        principalTable: "Abilities",
                        principalColumn: "AbilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeciesAbilities_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalSchema: "ReadModel",
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                schema: "ReadModel",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Region = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Checksum = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    Money = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PlayTime = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Gender = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Picture = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Reference = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    NationalPokedex = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PokedexCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.TrainerId);
                    table.ForeignKey(
                        name: "FK_Trainers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "ReadModel",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                schema: "ReadModel",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "integer", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => new { x.TrainerId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_Inventory_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "ReadModel",
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventory_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalSchema: "ReadModel",
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pokedex",
                schema: "ReadModel",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "integer", nullable: false),
                    SpeciesId = table.Column<int>(type: "integer", nullable: false),
                    HasCaught = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokedex", x => new { x.TrainerId, x.SpeciesId });
                    table.ForeignKey(
                        name: "FK_Pokedex_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalSchema: "ReadModel",
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pokedex_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalSchema: "ReadModel",
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pokemon",
                schema: "ReadModel",
                columns: table => new
                {
                    PokemonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SpeciesId = table.Column<int>(type: "integer", nullable: false),
                    AbilityId = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<byte>(type: "smallint", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Friendship = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    RemainingHatchSteps = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Gender = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Nature = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Characteristic = table.Column<int>(type: "integer", nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IndividualValues = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EffortValues = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Statistics = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CurrentHitPoints = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    StatusCondition = table.Column<int>(type: "integer", nullable: true),
                    HeldItemId = table.Column<int>(type: "integer", nullable: true),
                    BallId = table.Column<int>(type: "integer", nullable: true),
                    MetAtLevel = table.Column<byte>(type: "smallint", nullable: true),
                    MetLocation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MetOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrentTrainerId = table.Column<int>(type: "integer", nullable: true),
                    OriginalTrainerId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemon", x => x.PokemonId);
                    table.ForeignKey(
                        name: "FK_Pokemon_Abilities_AbilityId",
                        column: x => x.AbilityId,
                        principalSchema: "ReadModel",
                        principalTable: "Abilities",
                        principalColumn: "AbilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pokemon_Items_BallId",
                        column: x => x.BallId,
                        principalSchema: "ReadModel",
                        principalTable: "Items",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_Pokemon_Items_HeldItemId",
                        column: x => x.HeldItemId,
                        principalSchema: "ReadModel",
                        principalTable: "Items",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_Pokemon_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalSchema: "ReadModel",
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pokemon_Trainers_CurrentTrainerId",
                        column: x => x.CurrentTrainerId,
                        principalSchema: "ReadModel",
                        principalTable: "Trainers",
                        principalColumn: "TrainerId");
                    table.ForeignKey(
                        name: "FK_Pokemon_Trainers_OriginalTrainerId",
                        column: x => x.OriginalTrainerId,
                        principalSchema: "ReadModel",
                        principalTable: "Trainers",
                        principalColumn: "TrainerId");
                });

            migrationBuilder.CreateTable(
                name: "PokemonMoves",
                schema: "ReadModel",
                columns: table => new
                {
                    PokemonId = table.Column<int>(type: "integer", nullable: false),
                    MoveId = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<byte>(type: "smallint", nullable: false),
                    RemainingPowerPoints = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonMoves", x => new { x.PokemonId, x.MoveId });
                    table.ForeignKey(
                        name: "FK_PokemonMoves_Moves_MoveId",
                        column: x => x.MoveId,
                        principalSchema: "ReadModel",
                        principalTable: "Moves",
                        principalColumn: "MoveId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonMoves_Pokemon_PokemonId",
                        column: x => x.PokemonId,
                        principalSchema: "ReadModel",
                        principalTable: "Pokemon",
                        principalColumn: "PokemonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PokemonPositions",
                schema: "ReadModel",
                columns: table => new
                {
                    PokemonId = table.Column<int>(type: "integer", nullable: false),
                    TrainerId = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    Box = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonPositions", x => x.PokemonId);
                    table.ForeignKey(
                        name: "FK_PokemonPositions_Pokemon_PokemonId",
                        column: x => x.PokemonId,
                        principalSchema: "ReadModel",
                        principalTable: "Pokemon",
                        principalColumn: "PokemonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonPositions_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalSchema: "ReadModel",
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_Id",
                schema: "ReadModel",
                table: "Abilities",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_Name",
                schema: "ReadModel",
                table: "Abilities",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Evolutions_EvolvedSpeciesId",
                schema: "ReadModel",
                table: "Evolutions",
                column: "EvolvedSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolutions_ItemId",
                schema: "ReadModel",
                table: "Evolutions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolutions_MoveId",
                schema: "ReadModel",
                table: "Evolutions",
                column: "MoveId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ItemId",
                schema: "ReadModel",
                table: "Inventory",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Id",
                schema: "ReadModel",
                table: "Items",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                schema: "ReadModel",
                table: "Items",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Id",
                schema: "ReadModel",
                table: "Moves",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Name",
                schema: "ReadModel",
                table: "Moves",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Pokedex_HasCaught",
                schema: "ReadModel",
                table: "Pokedex",
                column: "HasCaught");

            migrationBuilder.CreateIndex(
                name: "IX_Pokedex_SpeciesId",
                schema: "ReadModel",
                table: "Pokedex",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_AbilityId",
                schema: "ReadModel",
                table: "Pokemon",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_BallId",
                schema: "ReadModel",
                table: "Pokemon",
                column: "BallId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_CurrentTrainerId",
                schema: "ReadModel",
                table: "Pokemon",
                column: "CurrentTrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_Gender",
                schema: "ReadModel",
                table: "Pokemon",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_HeldItemId",
                schema: "ReadModel",
                table: "Pokemon",
                column: "HeldItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_Id",
                schema: "ReadModel",
                table: "Pokemon",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_OriginalTrainerId",
                schema: "ReadModel",
                table: "Pokemon",
                column: "OriginalTrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_SpeciesId",
                schema: "ReadModel",
                table: "Pokemon",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_Surname",
                schema: "ReadModel",
                table: "Pokemon",
                column: "Surname");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonMoves_MoveId",
                schema: "ReadModel",
                table: "PokemonMoves",
                column: "MoveId");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonMoves_PokemonId_Position",
                schema: "ReadModel",
                table: "PokemonMoves",
                columns: new[] { "PokemonId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PokemonPositions_TrainerId_Position_Box",
                schema: "ReadModel",
                table: "PokemonPositions",
                columns: new[] { "TrainerId", "Position", "Box" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegionalSpecies_Region_Number",
                schema: "ReadModel",
                table: "RegionalSpecies",
                columns: new[] { "Region", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_Category",
                schema: "ReadModel",
                table: "Species",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Id",
                schema: "ReadModel",
                table: "Species",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_Name",
                schema: "ReadModel",
                table: "Species",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Number",
                schema: "ReadModel",
                table: "Species",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_PrimaryType",
                schema: "ReadModel",
                table: "Species",
                column: "PrimaryType");

            migrationBuilder.CreateIndex(
                name: "IX_Species_SecondaryType",
                schema: "ReadModel",
                table: "Species",
                column: "SecondaryType");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesAbilities_AbilityId",
                schema: "ReadModel",
                table: "SpeciesAbilities",
                column: "AbilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Gender",
                schema: "ReadModel",
                table: "Trainers",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Id",
                schema: "ReadModel",
                table: "Trainers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Name",
                schema: "ReadModel",
                table: "Trainers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Region",
                schema: "ReadModel",
                table: "Trainers",
                column: "Region");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_Region_Number_Name",
                schema: "ReadModel",
                table: "Trainers",
                columns: new[] { "Region", "Number", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_UserId",
                schema: "ReadModel",
                table: "Trainers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                schema: "ReadModel",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "ReadModel",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evolutions",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Inventory",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Pokedex",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "PokemonMoves",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "PokemonPositions",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "RegionalSpecies",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "SpeciesAbilities",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Moves",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Pokemon",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Abilities",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Items",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Species",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Trainers",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "ReadModel");
        }
    }
}
