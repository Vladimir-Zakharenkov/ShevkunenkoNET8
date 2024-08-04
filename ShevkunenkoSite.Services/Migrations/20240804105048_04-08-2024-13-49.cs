using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _040820241349 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PageInfo_BackgroundFileModelId",
                table: "PageInfo");

            migrationBuilder.DropIndex(
                name: "IX_MovieFile_PageInfoModelId",
                table: "MovieFile");

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_BackgroundFileModelId",
                table: "PageInfo",
                column: "BackgroundFileModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieFile_PageInfoModelId",
                table: "MovieFile",
                column: "PageInfoModelId",
                unique: true,
                filter: "[PageInfoModelId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PageInfo_BackgroundFileModelId",
                table: "PageInfo");

            migrationBuilder.DropIndex(
                name: "IX_MovieFile_PageInfoModelId",
                table: "MovieFile");

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_BackgroundFileModelId",
                table: "PageInfo",
                column: "BackgroundFileModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieFile_PageInfoModelId",
                table: "MovieFile",
                column: "PageInfoModelId");
        }
    }
}
