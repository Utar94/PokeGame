using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.Infrastructure.ReadModel.Migrations
{
    public partial class AddedPokemonFriendship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Friendship",
                schema: "ReadModel",
                table: "Pokemon",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Friendship",
                schema: "ReadModel",
                table: "Pokemon");
        }
    }
}
