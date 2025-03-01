using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _010320251533 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BooksAndArticlesModelId",
                table: "TextFile",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "TextFile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TextFile_BooksAndArticlesModelId",
                table: "TextFile",
                column: "BooksAndArticlesModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextFile_BooksAndArticles_BooksAndArticlesModelId",
                table: "TextFile",
                column: "BooksAndArticlesModelId",
                principalTable: "BooksAndArticles",
                principalColumn: "BooksArticlesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextFile_BooksAndArticles_BooksAndArticlesModelId",
                table: "TextFile");

            migrationBuilder.DropIndex(
                name: "IX_TextFile_BooksAndArticlesModelId",
                table: "TextFile");

            migrationBuilder.DropColumn(
                name: "BooksAndArticlesModelId",
                table: "TextFile");

            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "TextFile");
        }
    }
}
