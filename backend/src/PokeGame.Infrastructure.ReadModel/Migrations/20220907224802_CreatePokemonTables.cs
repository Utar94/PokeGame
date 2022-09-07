using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class CreatePokemonTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Gender = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Nature = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IndividualValues = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EffortValues = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Statistics = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HeldItemId = table.Column<int>(type: "integer", nullable: true),
                    MetAtLevel = table.Column<byte>(type: "smallint", nullable: true),
                    MetLocation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MetOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrentTrainerId = table.Column<int>(type: "integer", nullable: true),
                    OriginalTrainerId = table.Column<int>(type: "integer", nullable: true),
                    Position = table.Column<byte>(type: "smallint", nullable: true),
                    Box = table.Column<byte>(type: "smallint", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_AbilityId",
                schema: "ReadModel",
                table: "Pokemon",
                column: "AbilityId");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokemonMoves",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "Pokemon",
                schema: "ReadModel");
        }
    }
}
