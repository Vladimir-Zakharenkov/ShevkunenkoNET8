using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _220220252257 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BooksAndArticles",
                columns: table => new
                {
                    BooksArticlesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeOfText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorOfText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieCaption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TxtFileSize = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksAndArticles", x => x.BooksArticlesId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BooksAndArticles");
        }
    }
}
