using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _09102024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MovieFile_MoviePosterId",
                table: "MovieFile",
                column: "MoviePosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieFile_ImageFile_MoviePosterId",
                table: "MovieFile",
                column: "MoviePosterId",
                principalTable: "ImageFile",
                principalColumn: "ImageFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieFile_ImageFile_MoviePosterId",
                table: "MovieFile");

            migrationBuilder.DropIndex(
                name: "IX_MovieFile_MoviePosterId",
                table: "MovieFile");
        }
    }
}
