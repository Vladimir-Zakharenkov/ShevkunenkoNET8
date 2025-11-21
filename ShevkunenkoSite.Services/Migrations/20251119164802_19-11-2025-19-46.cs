using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _191120251946 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AudioInfoModel_PageInfo_PageInfoModelId",
                table: "AudioInfoModel");

            migrationBuilder.DropIndex(
                name: "IX_AudioInfoModel_PageInfoModelId",
                table: "AudioInfoModel");

            migrationBuilder.DropColumn(
                name: "PageInfoModelId",
                table: "AudioInfoModel");

            migrationBuilder.AddColumn<Guid>(
                name: "AudioInfoId",
                table: "PageInfo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_AudioInfoId",
                table: "PageInfo",
                column: "AudioInfoId",
                unique: true,
                filter: "[AudioInfoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PageInfo_AudioInfoModel_AudioInfoId",
                table: "PageInfo",
                column: "AudioInfoId",
                principalTable: "AudioInfoModel",
                principalColumn: "AudioInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageInfo_AudioInfoModel_AudioInfoId",
                table: "PageInfo");

            migrationBuilder.DropIndex(
                name: "IX_PageInfo_AudioInfoId",
                table: "PageInfo");

            migrationBuilder.DropColumn(
                name: "AudioInfoId",
                table: "PageInfo");

            migrationBuilder.AddColumn<Guid>(
                name: "PageInfoModelId",
                table: "AudioInfoModel",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AudioInfoModel_PageInfoModelId",
                table: "AudioInfoModel",
                column: "PageInfoModelId",
                unique: true,
                filter: "[PageInfoModelId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AudioInfoModel_PageInfo_PageInfoModelId",
                table: "AudioInfoModel",
                column: "PageInfoModelId",
                principalTable: "PageInfo",
                principalColumn: "PageInfoId");
        }
    }
}
