using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _261120252212 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TextInfoId",
                table: "PageInfo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_TextInfoId",
                table: "PageInfo",
                column: "TextInfoId",
                unique: true,
                filter: "[TextInfoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PageInfo_TextFile_TextInfoId",
                table: "PageInfo",
                column: "TextInfoId",
                principalTable: "TextFile",
                principalColumn: "TextInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageInfo_TextFile_TextInfoId",
                table: "PageInfo");

            migrationBuilder.DropIndex(
                name: "IX_PageInfo_TextInfoId",
                table: "PageInfo");

            migrationBuilder.DropColumn(
                name: "TextInfoId",
                table: "PageInfo");
        }
    }
}
