using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokeGame.ReadModel.Migrations
{
    public partial class RemovedTrainerChecksum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checksum",
                schema: "ReadModel",
                table: "Trainers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Checksum",
                schema: "ReadModel",
                table: "Trainers",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
