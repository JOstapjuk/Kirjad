using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kirjad.Migrations
{
    /// <inheritdoc />
    public partial class addedClassesControllers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kirjad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pealkiri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sisu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnikaalsekKood = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoodudKuupaev = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kirjad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KiriKiri",
                columns: table => new
                {
                    SeotudKirjadId = table.Column<int>(type: "int", nullable: false),
                    TagasiViitedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KiriKiri", x => new { x.SeotudKirjadId, x.TagasiViitedId });
                    table.ForeignKey(
                        name: "FK_KiriKiri_Kirjad_SeotudKirjadId",
                        column: x => x.SeotudKirjadId,
                        principalTable: "Kirjad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KiriKiri_Kirjad_TagasiViitedId",
                        column: x => x.TagasiViitedId,
                        principalTable: "Kirjad",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KiriKiri_TagasiViitedId",
                table: "KiriKiri",
                column: "TagasiViitedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KiriKiri");

            migrationBuilder.DropTable(
                name: "Kirjad");
        }
    }
}
