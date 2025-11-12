using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kirjad.Migrations
{
    /// <inheritdoc />
    public partial class rewriteDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KiriKiri_Kirjad_SeotudKirjadId",
                table: "KiriKiri");

            migrationBuilder.DropForeignKey(
                name: "FK_KiriKiri_Kirjad_TagasiViitedId",
                table: "KiriKiri");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KiriKiri",
                table: "KiriKiri");

            migrationBuilder.RenameTable(
                name: "KiriKiri",
                newName: "KirjadViited");

            migrationBuilder.RenameIndex(
                name: "IX_KiriKiri_TagasiViitedId",
                table: "KirjadViited",
                newName: "IX_KirjadViited_TagasiViitedId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KirjadViited",
                table: "KirjadViited",
                columns: new[] { "SeotudKirjadId", "TagasiViitedId" });

            migrationBuilder.AddForeignKey(
                name: "FK_KirjadViited_Kirjad_SeotudKirjadId",
                table: "KirjadViited",
                column: "SeotudKirjadId",
                principalTable: "Kirjad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KirjadViited_Kirjad_TagasiViitedId",
                table: "KirjadViited",
                column: "TagasiViitedId",
                principalTable: "Kirjad",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KirjadViited_Kirjad_SeotudKirjadId",
                table: "KirjadViited");

            migrationBuilder.DropForeignKey(
                name: "FK_KirjadViited_Kirjad_TagasiViitedId",
                table: "KirjadViited");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KirjadViited",
                table: "KirjadViited");

            migrationBuilder.RenameTable(
                name: "KirjadViited",
                newName: "KiriKiri");

            migrationBuilder.RenameIndex(
                name: "IX_KirjadViited_TagasiViitedId",
                table: "KiriKiri",
                newName: "IX_KiriKiri_TagasiViitedId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KiriKiri",
                table: "KiriKiri",
                columns: new[] { "SeotudKirjadId", "TagasiViitedId" });

            migrationBuilder.AddForeignKey(
                name: "FK_KiriKiri_Kirjad_SeotudKirjadId",
                table: "KiriKiri",
                column: "SeotudKirjadId",
                principalTable: "Kirjad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KiriKiri_Kirjad_TagasiViitedId",
                table: "KiriKiri",
                column: "TagasiViitedId",
                principalTable: "Kirjad",
                principalColumn: "Id");
        }
    }
}
