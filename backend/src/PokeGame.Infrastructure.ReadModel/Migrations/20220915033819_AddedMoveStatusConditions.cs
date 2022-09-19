using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class AddedMoveStatusConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "AccuracyStage",
                schema: "ReadModel",
                table: "Moves",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "EvasionStage",
                schema: "ReadModel",
                table: "Moves",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "StatisticStages",
                schema: "ReadModel",
                table: "Moves",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "StatusChance",
                schema: "ReadModel",
                table: "Moves",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusCondition",
                schema: "ReadModel",
                table: "Moves",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccuracyStage",
                schema: "ReadModel",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "EvasionStage",
                schema: "ReadModel",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "StatisticStages",
                schema: "ReadModel",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "StatusChance",
                schema: "ReadModel",
                table: "Moves");

            migrationBuilder.DropColumn(
                name: "StatusCondition",
                schema: "ReadModel",
                table: "Moves");
        }
    }
}
