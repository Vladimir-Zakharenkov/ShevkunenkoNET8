using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _040820242253 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PageInfo_BackgroundFileModelId",
                table: "PageInfo");

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_BackgroundFileModelId",
                table: "PageInfo",
                column: "BackgroundFileModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PageInfo_BackgroundFileModelId",
                table: "PageInfo");

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_BackgroundFileModelId",
                table: "PageInfo",
                column: "BackgroundFileModelId",
                unique: true);
        }
    }
}
