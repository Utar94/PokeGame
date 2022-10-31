using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class AddedEvolutionRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region",
                schema: "ReadModel",
                table: "Evolutions",
                newName: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolutions_RegionId",
                schema: "ReadModel",
                table: "Evolutions",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evolutions_Regions_RegionId",
                schema: "ReadModel",
                table: "Evolutions",
                column: "RegionId",
                principalSchema: "ReadModel",
                principalTable: "Regions",
                principalColumn: "RegionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evolutions_Regions_RegionId",
                schema: "ReadModel",
                table: "Evolutions");

            migrationBuilder.DropIndex(
                name: "IX_Evolutions_RegionId",
                schema: "ReadModel",
                table: "Evolutions");

            migrationBuilder.RenameColumn(
                name: "RegionId",
                schema: "ReadModel",
                table: "Evolutions",
                newName: "Region");
        }
    }
}
