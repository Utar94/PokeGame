using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class RenamedToLegacyRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region",
                schema: "ReadModel",
                table: "Trainers",
                newName: "LegacyRegion");

            migrationBuilder.RenameIndex(
                name: "IX_Trainers_Region_Number_Name",
                schema: "ReadModel",
                table: "Trainers",
                newName: "IX_Trainers_LegacyRegion_Number_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Trainers_Region",
                schema: "ReadModel",
                table: "Trainers",
                newName: "IX_Trainers_LegacyRegion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LegacyRegion",
                schema: "ReadModel",
                table: "Trainers",
                newName: "Region");

            migrationBuilder.RenameIndex(
                name: "IX_Trainers_LegacyRegion_Number_Name",
                schema: "ReadModel",
                table: "Trainers",
                newName: "IX_Trainers_Region_Number_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Trainers_LegacyRegion",
                schema: "ReadModel",
                table: "Trainers",
                newName: "IX_Trainers_Region");
        }
    }
}
