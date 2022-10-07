using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class AddSpeciesEggCycles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "EggCycles",
                schema: "ReadModel",
                table: "Species",
                type: "smallint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EggCycles",
                schema: "ReadModel",
                table: "Species");
        }
    }
}
