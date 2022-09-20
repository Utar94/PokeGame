using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class AddedMoveVolatileConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VolatileConditions",
                schema: "ReadModel",
                table: "Moves",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VolatileConditions",
                schema: "ReadModel",
                table: "Moves");
        }
    }
}
