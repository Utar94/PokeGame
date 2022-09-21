using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class CreatePokemonPositionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Box",
                schema: "ReadModel",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "Position",
                schema: "ReadModel",
                table: "Pokemon");

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
                name: "IX_PokemonPositions_TrainerId_Position_Box",
                schema: "ReadModel",
                table: "PokemonPositions",
                columns: new[] { "TrainerId", "Position", "Box" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokemonPositions",
                schema: "ReadModel");

            migrationBuilder.AddColumn<byte>(
                name: "Box",
                schema: "ReadModel",
                table: "Pokemon",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Position",
                schema: "ReadModel",
                table: "Pokemon",
                type: "smallint",
                nullable: true);
        }
    }
}
