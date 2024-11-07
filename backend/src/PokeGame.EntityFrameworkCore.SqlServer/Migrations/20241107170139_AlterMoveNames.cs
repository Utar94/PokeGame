using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AlterMoveNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Moves_Name",
                table: "Moves");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Moves",
                newName: "UniqueNameNormalized");

            migrationBuilder.RenameColumn(
                name: "Kind",
                table: "Moves",
                newName: "DisplayName");

            migrationBuilder.RenameIndex(
                name: "IX_Moves_Kind",
                table: "Moves",
                newName: "IX_Moves_DisplayName");

            migrationBuilder.AddColumn<string>(
                name: "UniqueName",
                table: "Moves",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_UniqueName",
                table: "Moves",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_UniqueNameNormalized",
                table: "Moves",
                column: "UniqueNameNormalized",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Moves_UniqueName",
                table: "Moves");

            migrationBuilder.DropIndex(
                name: "IX_Moves_UniqueNameNormalized",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "UniqueName",
                table: "Moves");

            migrationBuilder.RenameColumn(
                name: "UniqueNameNormalized",
                table: "Moves",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "Moves",
                newName: "Kind");

            migrationBuilder.RenameIndex(
                name: "IX_Moves_DisplayName",
                table: "Moves",
                newName: "IX_Moves_Kind");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Name",
                table: "Moves",
                column: "Name");
        }
    }
}
