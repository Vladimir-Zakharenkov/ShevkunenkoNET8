using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _170920250017 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AudioBookModel",
                columns: table => new
                {
                    AudioBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaptionOfAudioBook = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioBookDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActorOfAudioBook = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfFiles = table.Column<int>(type: "int", nullable: false),
                    BookForAudioBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PageInfoModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioBookModel", x => x.AudioBookId);
                    table.ForeignKey(
                        name: "FK_AudioBookModel_BooksAndArticles_BookForAudioBookId",
                        column: x => x.BookForAudioBookId,
                        principalTable: "BooksAndArticles",
                        principalColumn: "BooksArticlesId");
                    table.ForeignKey(
                        name: "FK_AudioBookModel_PageInfo_PageInfoModelId",
                        column: x => x.PageInfoModelId,
                        principalTable: "PageInfo",
                        principalColumn: "PageInfoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioBookModel_BookForAudioBookId",
                table: "AudioBookModel",
                column: "BookForAudioBookId",
                unique: true,
                filter: "[BookForAudioBookId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AudioBookModel_PageInfoModelId",
                table: "AudioBookModel",
                column: "PageInfoModelId",
                unique: true,
                filter: "[PageInfoModelId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioBookModel");
        }
    }
}
