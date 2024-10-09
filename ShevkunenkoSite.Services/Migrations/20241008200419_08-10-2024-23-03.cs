using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _081020242303 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PageForMovieSeriesId",
                table: "MovieFile",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_MovieFile_PageForMovieSeriesId",
                table: "MovieFile",
                column: "PageForMovieSeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieFile_PageInfo_PageForMovieSeriesId",
                table: "MovieFile",
                column: "PageForMovieSeriesId",
                principalTable: "PageInfo",
                principalColumn: "PageInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieFile_PageInfo_PageForMovieSeriesId",
                table: "MovieFile");

            migrationBuilder.DropIndex(
                name: "IX_MovieFile_PageForMovieSeriesId",
                table: "MovieFile");

            migrationBuilder.AlterColumn<Guid>(
                name: "PageForMovieSeriesId",
                table: "MovieFile",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
