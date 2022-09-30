using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class AddedTrainerPokedexCountAndNational : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NationalPokedex",
                schema: "ReadModel",
                table: "Trainers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PokedexCount",
                schema: "ReadModel",
                table: "Trainers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalPokedex",
                schema: "ReadModel",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "PokedexCount",
                schema: "ReadModel",
                table: "Trainers");
        }
    }
}
