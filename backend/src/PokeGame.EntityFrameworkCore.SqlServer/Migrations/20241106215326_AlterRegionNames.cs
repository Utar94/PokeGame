using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AlterRegionNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Regions_Name",
                table: "Regions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Regions",
                newName: "UniqueNameNormalized");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Regions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniqueName",
                table: "Regions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_DisplayName",
                table: "Regions",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_UniqueName",
                table: "Regions",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_UniqueNameNormalized",
                table: "Regions",
                column: "UniqueNameNormalized",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Regions_DisplayName",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_UniqueName",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_UniqueNameNormalized",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "UniqueName",
                table: "Regions");

            migrationBuilder.RenameColumn(
                name: "UniqueNameNormalized",
                table: "Regions",
                newName: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name",
                table: "Regions",
                column: "Name");
        }
    }
}
