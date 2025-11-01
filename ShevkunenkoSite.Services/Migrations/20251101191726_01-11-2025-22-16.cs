using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _011120252216 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AudioFileForTextAudioInfoModelId",
                table: "TextFile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AudioFileForTextId",
                table: "TextFile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TextFile_AudioFileForTextAudioInfoModelId",
                table: "TextFile",
                column: "AudioFileForTextAudioInfoModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextFile_AudioInfoModel_AudioFileForTextAudioInfoModelId",
                table: "TextFile",
                column: "AudioFileForTextAudioInfoModelId",
                principalTable: "AudioInfoModel",
                principalColumn: "AudioInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextFile_AudioInfoModel_AudioFileForTextAudioInfoModelId",
                table: "TextFile");

            migrationBuilder.DropIndex(
                name: "IX_TextFile_AudioFileForTextAudioInfoModelId",
                table: "TextFile");

            migrationBuilder.DropColumn(
                name: "AudioFileForTextAudioInfoModelId",
                table: "TextFile");

            migrationBuilder.DropColumn(
                name: "AudioFileForTextId",
                table: "TextFile");
        }
    }
}
