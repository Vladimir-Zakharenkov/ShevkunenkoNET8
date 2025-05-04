using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _300420250726 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PageInfoModelId",
                table: "BooksAndArticles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BooksAndArticles_PageInfoModelId",
                table: "BooksAndArticles",
                column: "PageInfoModelId",
                unique: true,
                filter: "[PageInfoModelId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksAndArticles_PageInfo_PageInfoModelId",
                table: "BooksAndArticles",
                column: "PageInfoModelId",
                principalTable: "PageInfo",
                principalColumn: "PageInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksAndArticles_PageInfo_PageInfoModelId",
                table: "BooksAndArticles");

            migrationBuilder.DropIndex(
                name: "IX_BooksAndArticles_PageInfoModelId",
                table: "BooksAndArticles");

            migrationBuilder.DropColumn(
                name: "PageInfoModelId",
                table: "BooksAndArticles");
        }
    }
}
