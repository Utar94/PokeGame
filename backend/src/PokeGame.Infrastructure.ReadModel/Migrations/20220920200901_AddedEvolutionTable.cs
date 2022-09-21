using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class AddedEvolutionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evolutions",
                schema: "ReadModel",
                columns: table => new
                {
                    EvolvingSpeciesId = table.Column<int>(type: "integer", nullable: false),
                    EvolvedSpeciesId = table.Column<int>(type: "integer", nullable: false),
                    Method = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    HighFriendship = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ItemId = table.Column<int>(type: "integer", nullable: true),
                    Level = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MoveId = table.Column<int>(type: "integer", nullable: true),
                    Region = table.Column<int>(type: "integer", nullable: true),
                    TimeOfDay = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evolutions", x => new { x.EvolvingSpeciesId, x.EvolvedSpeciesId });
                    table.ForeignKey(
                        name: "FK_Evolutions_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "ReadModel",
                        principalTable: "Items",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK_Evolutions_Moves_MoveId",
                        column: x => x.MoveId,
                        principalSchema: "ReadModel",
                        principalTable: "Moves",
                        principalColumn: "MoveId");
                    table.ForeignKey(
                        name: "FK_Evolutions_Species_EvolvedSpeciesId",
                        column: x => x.EvolvedSpeciesId,
                        principalSchema: "ReadModel",
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Evolutions_Species_EvolvingSpeciesId",
                        column: x => x.EvolvingSpeciesId,
                        principalSchema: "ReadModel",
                        principalTable: "Species",
                        principalColumn: "SpeciesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evolutions_EvolvedSpeciesId",
                schema: "ReadModel",
                table: "Evolutions",
                column: "EvolvedSpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolutions_ItemId",
                schema: "ReadModel",
                table: "Evolutions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Evolutions_MoveId",
                schema: "ReadModel",
                table: "Evolutions",
                column: "MoveId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evolutions",
                schema: "ReadModel");
        }
    }
}
