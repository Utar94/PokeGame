using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class AddedEnumsToReadModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characteristics",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvolutionMethods",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolutionMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "LevelingRates",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelingRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoveCategories",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PokemonGenders",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PokemonTypes",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusConditions",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimesOfDay",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesOfDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainerGenders",
                schema: "ReadModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerGenders", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "Characteristics",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "LovesEat" },
                    { 1, "ProudPower" },
                    { 2, "SturdyBody" },
                    { 3, "HighlyCurious" },
                    { 4, "StrongWilled" },
                    { 5, "LikesRun" },
                    { 6, "TakesPlentySiestas" },
                    { 7, "LikesThrashAbout" },
                    { 8, "CapableTakingHits" },
                    { 9, "Mischievous" },
                    { 10, "SomewhatVain" },
                    { 11, "AlertSounds" },
                    { 12, "NodsOffLot" },
                    { 13, "LittleQuickTempered" },
                    { 14, "HighlyPersistent" },
                    { 15, "ThoroughlyCunning" },
                    { 16, "StronglyDefiant" },
                    { 17, "ImpetuousSilly" },
                    { 18, "ScattersThingsOften" },
                    { 19, "LikesFight" },
                    { 20, "GoodEndurance" },
                    { 21, "OftenLostThought" },
                    { 22, "HatesLose" },
                    { 23, "SomewhatClown" },
                    { 24, "LikesRelax" },
                    { 25, "QuickTempered" },
                    { 26, "GoodPerseverance" },
                    { 27, "VeryFinicky" },
                    { 28, "SomewhatStubborn" },
                    { 29, "QuickFlee" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "EvolutionMethods",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "LevelUp" },
                    { 1, "Item" },
                    { 2, "Trade" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "ItemCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Medicine" },
                    { 1, "PokeBall" },
                    { 2, "BattleItem" },
                    { 3, "Berry" },
                    { 4, "OtherItem" },
                    { 5, "TM" },
                    { 6, "Treasure" },
                    { 7, "KeyItem" }
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

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "LevelingRates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -3, "Fluctuating" },
                    { -2, "Slow" },
                    { -1, "MediumSlow" },
                    { 0, "MediumFast" },
                    { 1, "Fast" },
                    { 2, "Erratic" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "MoveCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Status" },
                    { 1, "Physical" },
                    { 2, "Special" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "PokemonGenders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Unknown" },
                    { 1, "Male" },
                    { 2, "Female" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "PokemonTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Normal" },
                    { 1, "Fighting" },
                    { 2, "Flying" },
                    { 3, "Poison" },
                    { 4, "Ground" },
                    { 5, "Rock" },
                    { 6, "Bug" },
                    { 7, "Ghost" },
                    { 8, "Steel" },
                    { 9, "Fire" },
                    { 10, "Water" },
                    { 11, "Grass" },
                    { 12, "Electric" },
                    { 13, "Psychic" },
                    { 14, "Ice" },
                    { 15, "Dragon" },
                    { 16, "Dark" },
                    { 17, "Fairy" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "StatusConditions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Burn" },
                    { 1, "Freeze" },
                    { 2, "Paralysis" },
                    { 3, "Poison" },
                    { 4, "Sleep" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "TimesOfDay",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Day" },
                    { 1, "Night" }
                });

            migrationBuilder.InsertData(
                schema: "ReadModel",
                table: "TrainerGenders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 0, "Other" },
                    { 1, "Male" },
                    { 2, "Female" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_Name",
                schema: "ReadModel",
                table: "Characteristics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvolutionMethods_Name",
                schema: "ReadModel",
                table: "EvolutionMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_Name",
                schema: "ReadModel",
                table: "ItemCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_Name",
                schema: "ReadModel",
                table: "ItemTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LevelingRates_Name",
                schema: "ReadModel",
                table: "LevelingRates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoveCategories_Name",
                schema: "ReadModel",
                table: "MoveCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PokemonGenders_Name",
                schema: "ReadModel",
                table: "PokemonGenders",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PokemonTypes_Name",
                schema: "ReadModel",
                table: "PokemonTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusConditions_Name",
                schema: "ReadModel",
                table: "StatusConditions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimesOfDay_Name",
                schema: "ReadModel",
                table: "TimesOfDay",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainerGenders_Name",
                schema: "ReadModel",
                table: "TrainerGenders",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characteristics",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "EvolutionMethods",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "ItemCategories",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "ItemTypes",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "LevelingRates",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "MoveCategories",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "PokemonGenders",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "PokemonTypes",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "StatusConditions",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "TimesOfDay",
                schema: "ReadModel");

            migrationBuilder.DropTable(
                name: "TrainerGenders",
                schema: "ReadModel");
        }
    }
}
