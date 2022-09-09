using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class AddPokemonStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CurrentHitPoints",
                schema: "ReadModel",
                table: "Pokemon",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<int>(
                name: "StatusCondition",
                schema: "ReadModel",
                table: "Pokemon",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentHitPoints",
                schema: "ReadModel",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "StatusCondition",
                schema: "ReadModel",
                table: "Pokemon");
        }
    }
}
