using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateActorAndAbilityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    AbilityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueNameNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AggregateId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.AbilityId);
                });

            migrationBuilder.CreateTable(
                name: "Actors",
                columns: table => new
                {
                    ActorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actors", x => x.ActorId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_AggregateId",
                table: "Abilities",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_CreatedBy",
                table: "Abilities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_CreatedOn",
                table: "Abilities",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_DisplayName",
                table: "Abilities",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_UniqueName",
                table: "Abilities",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_UniqueNameNormalized",
                table: "Abilities",
                column: "UniqueNameNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_UpdatedBy",
                table: "Abilities",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_UpdatedOn",
                table: "Abilities",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_Version",
                table: "Abilities",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_DisplayName",
                table: "Actors",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_EmailAddress",
                table: "Actors",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_Id",
                table: "Actors",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Actors_IsDeleted",
                table: "Actors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Actors_Type",
                table: "Actors",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "Actors");
        }
    }
}
