using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
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
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActorId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
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
                name: "IX_Abilities_Id",
                table: "Abilities",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_Kind",
                table: "Abilities",
                column: "Kind");

            migrationBuilder.CreateIndex(
                name: "IX_Abilities_Name",
                table: "Abilities",
                column: "Name");

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
                name: "IX_Users_ActorId",
                table: "Users",
                column: "ActorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_DisplayName",
                table: "Users",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                table: "Users",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
