using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class RegionalSpeciesRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trainers_LegacyRegion",
                schema: "ReadModel",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_LegacyRegion_Number_Name",
                schema: "ReadModel",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_RegionId",
                schema: "ReadModel",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "LegacyRegion",
                schema: "ReadModel",
                table: "Trainers");

            migrationBuilder.RenameColumn(
                name: "Region",
                schema: "ReadModel",
                table: "RegionalSpecies",
                newName: "RegionId");

            migrationBuilder.RenameIndex(
                name: "IX_RegionalSpecies_Region_Number",
                schema: "ReadModel",
                table: "RegionalSpecies",
                newName: "IX_RegionalSpecies_RegionId_Number");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_RegionId_Number_Name",
                schema: "ReadModel",
                table: "Trainers",
                columns: new[] { "RegionId", "Number", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RegionalSpecies_Regions_RegionId",
                schema: "ReadModel",
                table: "RegionalSpecies",
                column: "RegionId",
                principalSchema: "ReadModel",
                principalTable: "Regions",
                principalColumn: "RegionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegionalSpecies_Regions_RegionId",
                schema: "ReadModel",
                table: "RegionalSpecies");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_RegionId_Number_Name",
                schema: "ReadModel",
                table: "Trainers");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                schema: "ReadModel",
                table: "RegionalSpecies",
                newName: "Region");

            migrationBuilder.RenameIndex(
                name: "IX_RegionalSpecies_RegionId_Number",
                schema: "ReadModel",
                table: "RegionalSpecies",
                newName: "IX_RegionalSpecies_Region_Number");

            migrationBuilder.AddColumn<int>(
                name: "LegacyRegion",
                schema: "ReadModel",
                table: "Trainers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_LegacyRegion",
                schema: "ReadModel",
                table: "Trainers",
                column: "LegacyRegion");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_LegacyRegion_Number_Name",
                schema: "ReadModel",
                table: "Trainers",
                columns: new[] { "LegacyRegion", "Number", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_RegionId",
                schema: "ReadModel",
                table: "Trainers",
                column: "RegionId");
        }
    }
}
