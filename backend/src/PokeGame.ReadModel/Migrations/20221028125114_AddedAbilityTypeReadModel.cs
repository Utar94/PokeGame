using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class AddedAbilityTypeReadModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "ReadModel",
                table: "Abilities",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AbilityTypes",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "AbilityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Adaptability" },
                    { 1, "AirLock" },
                    { 2, "CloudNine" },
                    { 3, "Guts" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbilityTypes_Name",
                schema: "ReadModel",
                table: "AbilityTypes",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityTypes",
                schema: "ReadModel");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "ReadModel",
                table: "Abilities");
        }
    }
}
