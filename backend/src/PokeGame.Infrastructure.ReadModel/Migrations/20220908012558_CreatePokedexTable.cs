using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class CreatePokedexTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pokedex",
                schema: "ReadModel",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "integer", nullable: false),
                    SpeciesId = table.Column<int>(type: "integer", nullable: false),
                    HasCaught = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pokedex",
                schema: "ReadModel");
        }
    }
}
