using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class AbilityItemMoveKinds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityTypes",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "ItemTypes",
                schema: "ReadModel");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "ReadModel",
                table: "Items",
                newName: "Kind");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "ReadModel",
                table: "Abilities",
                newName: "Kind");

            migrationBuilder.AddColumn<int>(
                name: "Kind",
                schema: "ReadModel",
                table: "Moves",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AbilityKinds",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityKinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemKinds",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemKinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoveKinds",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveKinds", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "AbilityKinds",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Adaptability" },
                    { 1, "AirLock" },
                    { 2, "CloudNine" },
                    { 3, "Guts" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "ItemKinds",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "ExpShare" },
                    { 1, "LuckyEgg" },
                    { 2, "FriendBall" },
                    { 3, "LuxuryBall" },
                    { 4, "HealBall" },
                    { 5, "SootheBell" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "MoveKinds",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "Facade" });

            migrationBuilder.CreateIndex(
                name: "IX_AbilityKinds_Name",
                schema: "ReadModel",
                table: "AbilityKinds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemKinds_Name",
                schema: "ReadModel",
                table: "ItemKinds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoveKinds_Name",
                schema: "ReadModel",
                table: "MoveKinds",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityKinds",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "ItemKinds",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "MoveKinds",
                schema: "ReadModel");

            migrationBuilder.DropColumn(
                name: "Kind",
                schema: "ReadModel",
                table: "Moves");

            migrationBuilder.RenameColumn(
                name: "Kind",
                schema: "ReadModel",
                table: "Items",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Kind",
                schema: "ReadModel",
                table: "Abilities",
                newName: "Type");

            migrationBuilder.CreateTable(
                name: "AbilityTypes",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "AbilityTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Adaptability" },
                    { 1, "AirLock" },
                    { 2, "CloudNine" },
                    { 3, "Guts" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "ItemTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "ExpShare" },
                    { 1, "LuckyEgg" },
                    { 2, "FriendBall" },
                    { 3, "LuxuryBall" },
                    { 4, "HealBall" },
                    { 5, "SootheBell" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbilityTypes_Name",
                schema: "ReadModel",
                table: "AbilityTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_Name",
                schema: "ReadModel",
                table: "ItemTypes",
                column: "Name",
                unique: true);
        }
    }
}
