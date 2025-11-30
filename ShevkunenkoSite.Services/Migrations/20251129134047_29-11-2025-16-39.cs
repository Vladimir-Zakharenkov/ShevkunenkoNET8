using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _291120251639 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageFileModelId",
                table: "BooksAndArticles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BooksAndArticles_ImageFileModelId",
                table: "BooksAndArticles",
                column: "ImageFileModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksAndArticles_ImageFile_ImageFileModelId",
                table: "BooksAndArticles",
                column: "ImageFileModelId",
                principalTable: "ImageFile",
                principalColumn: "ImageFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksAndArticles_ImageFile_ImageFileModelId",
                table: "BooksAndArticles");

            migrationBuilder.DropIndex(
                name: "IX_BooksAndArticles_ImageFileModelId",
                table: "BooksAndArticles");

            migrationBuilder.DropColumn(
                name: "ImageFileModelId",
                table: "BooksAndArticles");
        }
    }
}
