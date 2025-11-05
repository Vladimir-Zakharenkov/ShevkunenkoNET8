using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _051120252134 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AudioInfoModelId",
                table: "TextFile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TextFile_AudioInfoModelId",
                table: "TextFile",
                column: "AudioInfoModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextFile_AudioInfoModel_AudioInfoModelId",
                table: "TextFile",
                column: "AudioInfoModelId",
                principalTable: "AudioInfoModel",
                principalColumn: "AudioInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextFile_AudioInfoModel_AudioInfoModelId",
                table: "TextFile");

            migrationBuilder.DropIndex(
                name: "IX_TextFile_AudioInfoModelId",
                table: "TextFile");

            migrationBuilder.DropColumn(
                name: "AudioInfoModelId",
                table: "TextFile");
        }
    }
}
