using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _050120251451 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextFile",
                columns: table => new
                {
                    TextInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClearText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HtmlText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextFile", x => x.TextInfoId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextFile");
        }
    }
}
