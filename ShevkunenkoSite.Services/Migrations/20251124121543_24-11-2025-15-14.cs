using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _241120251514 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TextFile_AudioInfoModelId",
                table: "TextFile");

            migrationBuilder.CreateIndex(
                name: "IX_TextFile_AudioInfoModelId",
                table: "TextFile",
                column: "AudioInfoModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TextFile_AudioInfoModelId",
                table: "TextFile");

            migrationBuilder.CreateIndex(
                name: "IX_TextFile_AudioInfoModelId",
                table: "TextFile",
                column: "AudioInfoModelId",
                unique: true,
                filter: "[AudioInfoModelId] IS NOT NULL");
        }
    }
}
