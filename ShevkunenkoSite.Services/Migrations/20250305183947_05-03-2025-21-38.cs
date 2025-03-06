using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _050320252138 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LogoOfArticleId",
                table: "BooksAndArticles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BooksAndArticles_LogoOfArticleId",
                table: "BooksAndArticles",
                column: "LogoOfArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksAndArticles_ImageFile_LogoOfArticleId",
                table: "BooksAndArticles",
                column: "LogoOfArticleId",
                principalTable: "ImageFile",
                principalColumn: "ImageFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksAndArticles_ImageFile_LogoOfArticleId",
                table: "BooksAndArticles");

            migrationBuilder.DropIndex(
                name: "IX_BooksAndArticles_LogoOfArticleId",
                table: "BooksAndArticles");

            migrationBuilder.DropColumn(
                name: "LogoOfArticleId",
                table: "BooksAndArticles");
        }
    }
}
