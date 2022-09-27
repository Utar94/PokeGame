using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class CreateRegionalSpeciesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegionalSpecies",
                schema: "ReadModel",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "integer", nullable: false),
                    Region = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalSpecies", x => new { x.SpeciesId, x.Region });
                    table.ForeignKey(
                        name: "FK_RegionalSpecies_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalSchema: "ReadModel",
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegionalSpecies_Region_Number",
                schema: "ReadModel",
                table: "RegionalSpecies",
                columns: new[] { "Region", "Number" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegionalSpecies",
                schema: "ReadModel");
        }
    }
}
