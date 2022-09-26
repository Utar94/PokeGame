using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class AddPokemonBallCaughtWith : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BallId",
                schema: "ReadModel",
                table: "Pokemon",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pokemon_BallId",
                schema: "ReadModel",
                table: "Pokemon",
                column: "BallId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pokemon_Items_BallId",
                schema: "ReadModel",
                table: "Pokemon",
                column: "BallId",
                principalSchema: "ReadModel",
                principalTable: "Items",
                principalColumn: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pokemon_Items_BallId",
                schema: "ReadModel",
                table: "Pokemon");

            migrationBuilder.DropIndex(
                name: "IX_Pokemon_BallId",
                schema: "ReadModel",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "BallId",
                schema: "ReadModel",
                table: "Pokemon");
        }
    }
}
