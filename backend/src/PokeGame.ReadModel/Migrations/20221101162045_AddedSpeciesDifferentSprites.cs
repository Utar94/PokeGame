using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class AddedSpeciesDifferentSprites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureFemale",
                schema: "ReadModel",
                table: "Species",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureShiny",
                schema: "ReadModel",
                table: "Species",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PictureShinyFemale",
                schema: "ReadModel",
                table: "Species",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureFemale",
                schema: "ReadModel",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "PictureShiny",
                schema: "ReadModel",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "PictureShinyFemale",
                schema: "ReadModel",
                table: "Species");
        }
    }
}
