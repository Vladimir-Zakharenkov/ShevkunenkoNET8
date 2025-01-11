using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _050120252156 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TextInfoModelId",
                table: "MovieFile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieFile_TextInfoModelId",
                table: "MovieFile",
                column: "TextInfoModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieFile_TextFile_TextInfoModelId",
                table: "MovieFile",
                column: "TextInfoModelId",
                principalTable: "TextFile",
                principalColumn: "TextInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieFile_TextFile_TextInfoModelId",
                table: "MovieFile");

            migrationBuilder.DropIndex(
                name: "IX_MovieFile_TextInfoModelId",
                table: "MovieFile");

            migrationBuilder.DropColumn(
                name: "TextInfoModelId",
                table: "MovieFile");
        }
    }
}
