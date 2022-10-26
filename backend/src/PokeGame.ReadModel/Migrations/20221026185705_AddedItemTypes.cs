using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class AddedItemTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "ReadModel",
                table: "Items",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                schema: "ReadModel",
                table: "Items");
        }
    }
}
