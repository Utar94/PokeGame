using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CreateMoveAndSpeciesTables : Migration
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
                    UniqueName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueNameNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accuracy = table.Column<byte>(type: "tinyint", nullable: true),
                    Power = table.Column<byte>(type: "tinyint", nullable: true),
                    PowerPoints = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCondition = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StatusChance = table.Column<int>(type: "int", nullable: true),
                    StatisticChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolatileConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.MoveId);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UniqueNameNormalized = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GrowthRate = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BaseFriendship = table.Column<byte>(type: "tinyint", nullable: false),
                    CatchRate = table.Column<byte>(type: "tinyint", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreamId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.SpeciesId);
                });

            migrationBuilder.CreateTable(
                name: "RegionalSpecies",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionalSpecies", x => new { x.SpeciesId, x.RegionId });
                    table.ForeignKey(
                        name: "FK_RegionalSpecies_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegionalSpecies_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Accuracy",
                table: "Moves",
                column: "Accuracy");

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
                name: "IX_Moves_DisplayName",
                table: "Moves",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Id",
                table: "Moves",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Power",
                table: "Moves",
                column: "Power");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_PowerPoints",
                table: "Moves",
                column: "PowerPoints");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_StatusCondition",
                table: "Moves",
                column: "StatusCondition");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_StreamId",
                table: "Moves",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Moves_Type",
                table: "Moves",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_UniqueName",
                table: "Moves",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Moves_UniqueNameNormalized",
                table: "Moves",
                column: "UniqueNameNormalized",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_RegionalSpecies_RegionId_Number",
                table: "RegionalSpecies",
                columns: new[] { "RegionId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_Category",
                table: "Species",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Species_CreatedBy",
                table: "Species",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Species_CreatedOn",
                table: "Species",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Species_DisplayName",
                table: "Species",
                column: "DisplayName");

            migrationBuilder.CreateIndex(
                name: "IX_Species_GrowthRate",
                table: "Species",
                column: "GrowthRate");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Id",
                table: "Species",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_Number",
                table: "Species",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_StreamId",
                table: "Species",
                column: "StreamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_UniqueName",
                table: "Species",
                column: "UniqueName");

            migrationBuilder.CreateIndex(
                name: "IX_Species_UniqueNameNormalized",
                table: "Species",
                column: "UniqueNameNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_UpdatedBy",
                table: "Species",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Species_UpdatedOn",
                table: "Species",
                column: "UpdatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Version",
                table: "Species",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropTable(
                name: "RegionalSpecies");

            migrationBuilder.DropTable(
                name: "Species");
        }
    }
}
