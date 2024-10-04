using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _190920240121 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieFile_ImageFile_ImageForHeadSeriesImageFileModelId",
                table: "MovieFile");

            migrationBuilder.RenameColumn(
                name: "ImageForHeadSeriesImageFileModelId",
                table: "MovieFile",
                newName: "ImageForHeadSeriesId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieFile_ImageForHeadSeriesImageFileModelId",
                table: "MovieFile",
                newName: "IX_MovieFile_ImageForHeadSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieFile_ImageFile_ImageForHeadSeriesId",
                table: "MovieFile",
                column: "ImageForHeadSeriesId",
                principalTable: "ImageFile",
                principalColumn: "ImageFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieFile_ImageFile_ImageForHeadSeriesId",
                table: "MovieFile");

            migrationBuilder.RenameColumn(
                name: "ImageForHeadSeriesId",
                table: "MovieFile",
                newName: "ImageForHeadSeriesImageFileModelId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieFile_ImageForHeadSeriesId",
                table: "MovieFile",
                newName: "IX_MovieFile_ImageForHeadSeriesImageFileModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieFile_ImageFile_ImageForHeadSeriesImageFileModelId",
                table: "MovieFile",
                column: "ImageForHeadSeriesImageFileModelId",
                principalTable: "ImageFile",
                principalColumn: "ImageFileId");
        }
    }
}
