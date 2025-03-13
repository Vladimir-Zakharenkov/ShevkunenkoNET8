using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _130320250024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ScanOfArticleId",
                table: "BooksAndArticles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BooksAndArticles_ScanOfArticleId",
                table: "BooksAndArticles",
                column: "ScanOfArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksAndArticles_ImageFile_ScanOfArticleId",
                table: "BooksAndArticles",
                column: "ScanOfArticleId",
                principalTable: "ImageFile",
                principalColumn: "ImageFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksAndArticles_ImageFile_ScanOfArticleId",
                table: "BooksAndArticles");

            migrationBuilder.DropIndex(
                name: "IX_BooksAndArticles_ScanOfArticleId",
                table: "BooksAndArticles");

            migrationBuilder.DropColumn(
                name: "ScanOfArticleId",
                table: "BooksAndArticles");
        }
    }
}
