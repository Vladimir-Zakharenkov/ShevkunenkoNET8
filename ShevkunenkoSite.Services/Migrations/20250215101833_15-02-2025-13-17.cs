using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _150220251317 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadingOfArticle1",
                table: "MovieFile");

            migrationBuilder.DropColumn(
                name: "TextOfArticle1",
                table: "MovieFile");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeadingOfArticle1",
                table: "MovieFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TextOfArticle1",
                table: "MovieFile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
