using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _291120251610 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AudioBookModel_BookForAudioBookId",
                table: "AudioBookModel");

            migrationBuilder.CreateIndex(
                name: "IX_AudioBookModel_BookForAudioBookId",
                table: "AudioBookModel",
                column: "BookForAudioBookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AudioBookModel_BookForAudioBookId",
                table: "AudioBookModel");

            migrationBuilder.CreateIndex(
                name: "IX_AudioBookModel_BookForAudioBookId",
                table: "AudioBookModel",
                column: "BookForAudioBookId",
                unique: true,
                filter: "[BookForAudioBookId] IS NOT NULL");
        }
    }
}
