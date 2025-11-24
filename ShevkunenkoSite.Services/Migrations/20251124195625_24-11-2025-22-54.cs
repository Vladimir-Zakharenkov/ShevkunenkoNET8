using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _241120252254 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AudioBookModel_PageInfo_PageInfoModelId",
                table: "AudioBookModel");

            migrationBuilder.DropIndex(
                name: "IX_AudioBookModel_PageInfoModelId",
                table: "AudioBookModel");

            migrationBuilder.DropColumn(
                name: "PageInfoModelId",
                table: "AudioBookModel");

            migrationBuilder.AddColumn<Guid>(
                name: "AudioBookModelId",
                table: "PageInfo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_AudioBookModelId",
                table: "PageInfo",
                column: "AudioBookModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PageInfo_AudioBookModel_AudioBookModelId",
                table: "PageInfo",
                column: "AudioBookModelId",
                principalTable: "AudioBookModel",
                principalColumn: "AudioBookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageInfo_AudioBookModel_AudioBookModelId",
                table: "PageInfo");

            migrationBuilder.DropIndex(
                name: "IX_PageInfo_AudioBookModelId",
                table: "PageInfo");

            migrationBuilder.DropColumn(
                name: "AudioBookModelId",
                table: "PageInfo");

            migrationBuilder.AddColumn<Guid>(
                name: "PageInfoModelId",
                table: "AudioBookModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AudioBookModel_PageInfoModelId",
                table: "AudioBookModel",
                column: "PageInfoModelId",
                unique: true,
                filter: "[PageInfoModelId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AudioBookModel_PageInfo_PageInfoModelId",
                table: "AudioBookModel",
                column: "PageInfoModelId",
                principalTable: "PageInfo",
                principalColumn: "PageInfoId");
        }
    }
}
