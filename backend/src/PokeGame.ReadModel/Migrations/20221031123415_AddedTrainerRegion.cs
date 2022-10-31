using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class AddedTrainerRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                schema: "ReadModel",
                table: "Trainers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_RegionId",
                schema: "ReadModel",
                table: "Trainers",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Regions_RegionId",
                schema: "ReadModel",
                table: "Trainers",
                column: "RegionId",
                principalSchema: "ReadModel",
                principalTable: "Regions",
                principalColumn: "RegionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Regions_RegionId",
                schema: "ReadModel",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_RegionId",
                schema: "ReadModel",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "RegionId",
                schema: "ReadModel",
                table: "Trainers");
        }
    }
}
