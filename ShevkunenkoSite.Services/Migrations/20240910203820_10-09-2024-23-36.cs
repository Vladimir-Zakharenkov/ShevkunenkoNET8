using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _100920242336 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePageHeading",
                table: "PageInfo",
                newName: "ImagePageHeadingId");

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_ImagePageHeadingId",
                table: "PageInfo",
                column: "ImagePageHeadingId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageInfo_ImageFile_ImagePageHeadingId",
                table: "PageInfo",
                column: "ImagePageHeadingId",
                principalTable: "ImageFile",
                principalColumn: "ImageFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageInfo_ImageFile_ImagePageHeadingId",
                table: "PageInfo");

            migrationBuilder.DropIndex(
                name: "IX_PageInfo_ImagePageHeadingId",
                table: "PageInfo");

            migrationBuilder.RenameColumn(
                name: "ImagePageHeadingId",
                table: "PageInfo",
                newName: "ImagePageHeading");
        }
    }
}
