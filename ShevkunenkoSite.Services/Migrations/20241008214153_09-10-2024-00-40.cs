using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _091020240040 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MovieFile_FullMovieID",
                table: "MovieFile",
                column: "FullMovieID");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieFile_MovieFile_FullMovieID",
                table: "MovieFile",
                column: "FullMovieID",
                principalTable: "MovieFile",
                principalColumn: "MovieFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieFile_MovieFile_FullMovieID",
                table: "MovieFile");

            migrationBuilder.DropIndex(
                name: "IX_MovieFile_FullMovieID",
                table: "MovieFile");
        }
    }
}
