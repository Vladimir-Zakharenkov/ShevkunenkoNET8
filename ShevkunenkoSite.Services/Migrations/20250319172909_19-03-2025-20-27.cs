using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _190320252027 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VideoForBookOrArticleId",
                table: "BooksAndArticles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BooksAndArticles_VideoForBookOrArticleId",
                table: "BooksAndArticles",
                column: "VideoForBookOrArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksAndArticles_MovieFile_VideoForBookOrArticleId",
                table: "BooksAndArticles",
                column: "VideoForBookOrArticleId",
                principalTable: "MovieFile",
                principalColumn: "MovieFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksAndArticles_MovieFile_VideoForBookOrArticleId",
                table: "BooksAndArticles");

            migrationBuilder.DropIndex(
                name: "IX_BooksAndArticles_VideoForBookOrArticleId",
                table: "BooksAndArticles");

            migrationBuilder.DropColumn(
                name: "VideoForBookOrArticleId",
                table: "BooksAndArticles");
        }
    }
}
