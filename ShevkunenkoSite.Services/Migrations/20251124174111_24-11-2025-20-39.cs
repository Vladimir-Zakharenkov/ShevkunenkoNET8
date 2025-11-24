using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _241120252039 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PageInfo_AudioInfoId",
                table: "PageInfo");

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_AudioInfoId",
                table: "PageInfo",
                column: "AudioInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PageInfo_AudioInfoId",
                table: "PageInfo");

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_AudioInfoId",
                table: "PageInfo",
                column: "AudioInfoId",
                unique: true,
                filter: "[AudioInfoId] IS NOT NULL");
        }
    }
}
