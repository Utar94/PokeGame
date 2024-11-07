using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AlterAbilityNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Abilities_Name",
                table: "Abilities");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Abilities",
                newName: "UniqueNameNormalized");

            migrationBuilder.RenameColumn(
                name: "Kind",
                table: "Abilities",
                newName: "DisplayName");

            migrationBuilder.RenameIndex(
                name: "IX_Abilities_Kind",
                table: "Abilities",
                newName: "IX_Abilities_DisplayName");

            migrationBuilder.AddColumn<string>(
                name: "UniqueName",
                table: "Abilities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_UniqueName",
                table: "Abilities",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_UniqueNameNormalized",
                table: "Abilities",
                column: "UniqueNameNormalized",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Abilities_UniqueName",
                table: "Abilities");

            migrationBuilder.DropIndex(
                name: "IX_Abilities_UniqueNameNormalized",
                table: "Abilities");

            migrationBuilder.DropColumn(
                name: "UniqueName",
                table: "Abilities");

            migrationBuilder.RenameColumn(
                name: "UniqueNameNormalized",
                table: "Abilities",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "Abilities",
                newName: "Kind");

            migrationBuilder.RenameIndex(
                name: "IX_Abilities_DisplayName",
                table: "Abilities",
                newName: "IX_Abilities_Kind");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_Name",
                table: "Abilities",
                column: "Name");
        }
    }
}
