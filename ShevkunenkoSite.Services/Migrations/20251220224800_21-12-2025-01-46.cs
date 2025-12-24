using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _211220250146 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "BookCaptionForURL",
                columns: table => new
                {
                    BookCaptionForURLId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookCaptionForURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BooksAndArticlesModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCaptionForURL", x => x.BookCaptionForURLId);
                    table.ForeignKey(
                        name: "FK_BookCaptionForURL_BooksAndArticles_BooksAndArticlesModelId",
                        column: x => x.BooksAndArticlesModelId,
                        principalTable: "BooksAndArticles",
                        principalColumn: "BooksArticlesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCaptionForURL_BooksAndArticlesModelId",
                table: "BookCaptionForURL",
                column: "BooksAndArticlesModelId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCaptionForURL");

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
    }
}
