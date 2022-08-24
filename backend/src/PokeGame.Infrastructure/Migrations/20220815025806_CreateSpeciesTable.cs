using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PokeGame.Infrastructure.Migrations
{
    public partial class CreateSpeciesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Sid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    PrimaryType = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    SecondaryType = table.Column<int>(type: "integer", nullable: true),
                    AbilitySid = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Reference = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Sid);
                    table.ForeignKey(
                        name: "FK_Species_Abilities_AbilitySid",
                        column: x => x.AbilitySid,
                        principalTable: "Abilities",
                        principalColumn: "Sid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Species_AbilitySid",
                table: "Species",
                column: "AbilitySid");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Category",
                table: "Species",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Id",
                table: "Species",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_Name",
                table: "Species",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Species_Number",
                table: "Species",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_PrimaryType",
                table: "Species",
                column: "PrimaryType");

            migrationBuilder.CreateIndex(
                name: "IX_Species_SecondaryType",
                table: "Species",
                column: "SecondaryType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Species");
        }
    }
}
