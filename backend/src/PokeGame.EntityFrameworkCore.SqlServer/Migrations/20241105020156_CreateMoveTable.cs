using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.EntityFrameworkCore.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateMoveTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    MoveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Kind = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accuracy = table.Column<int>(type: "int", nullable: true),
                    Power = table.Column<int>(type: "int", nullable: true),
                    PowerPoints = table.Column<int>(type: "int", nullable: false),
                    AttackStages = table.Column<int>(type: "int", nullable: false),
                    DefenseStages = table.Column<int>(type: "int", nullable: false),
                    SpecialAttackStages = table.Column<int>(type: "int", nullable: false),
                    SpecialDefenseStages = table.Column<int>(type: "int", nullable: false),
                    SpeedStages = table.Column<int>(type: "int", nullable: false),
                    AccuracyStages = table.Column<int>(type: "int", nullable: false),
                    EvasionStages = table.Column<int>(type: "int", nullable: false),
                    InflictedStatusCondition = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    InflictedStatusChance = table.Column<int>(type: "int", nullable: true),
                    VolatileConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Moves", x => x.MoveId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Accuracy",
                table: "Moves",
                column: "Accuracy");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_AggregateId",
                table: "Moves",
                column: "AggregateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Category",
                table: "Moves",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_CreatedBy",
                table: "Moves",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_CreatedOn",
                table: "Moves",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Id",
                table: "Moves",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Kind",
                table: "Moves",
                column: "Kind");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Name",
                table: "Moves",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Power",
                table: "Moves",
                column: "Power");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_PowerPoints",
                table: "Moves",
                column: "PowerPoints");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Type",
                table: "Moves",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_UpdatedBy",
                table: "Moves",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_UpdatedOn",
                table: "Moves",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Version",
                table: "Moves",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moves");
        }
    }
}
