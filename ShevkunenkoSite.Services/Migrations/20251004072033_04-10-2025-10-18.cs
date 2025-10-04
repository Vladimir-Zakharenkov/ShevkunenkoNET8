using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class _041020251018 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AudioInfoModel",
                columns: table => new
                {
                    AudioInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorOfText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaptionOfTextInAudioFile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TextInfoModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AudioFileDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioFileDuration = table.Column<long>(type: "bigint", nullable: false),
                    AudioFileBitRate = table.Column<int>(type: "int", nullable: false),
                    AudioFileFrequency = table.Column<int>(type: "int", nullable: false),
                    AudioFileSize = table.Column<int>(type: "int", nullable: false),
                    AudioFileMimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioFileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioFilePlaybackType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SortOfAudioFile = table.Column<int>(type: "int", nullable: false),
                    InternetRefToAudioFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AudioFileUploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AudioBookModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SequenceNumber = table.Column<int>(type: "int", nullable: true),
                    PageInfoModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioInfoModel", x => x.AudioInfoId);
                    table.ForeignKey(
                        name: "FK_AudioInfoModel_AudioBookModel_AudioBookModelId",
                        column: x => x.AudioBookModelId,
                        principalTable: "AudioBookModel",
                        principalColumn: "AudioBookId");
                    table.ForeignKey(
                        name: "FK_AudioInfoModel_PageInfo_PageInfoModelId",
                        column: x => x.PageInfoModelId,
                        principalTable: "PageInfo",
                        principalColumn: "PageInfoId");
                    table.ForeignKey(
                        name: "FK_AudioInfoModel_TextFile_TextInfoModelId",
                        column: x => x.TextInfoModelId,
                        principalTable: "TextFile",
                        principalColumn: "TextInfoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioInfoModel_AudioBookModelId",
                table: "AudioInfoModel",
                column: "AudioBookModelId");

            migrationBuilder.CreateIndex(
                name: "IX_AudioInfoModel_PageInfoModelId",
                table: "AudioInfoModel",
                column: "PageInfoModelId",
                unique: true,
                filter: "[PageInfoModelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AudioInfoModel_TextInfoModelId",
                table: "AudioInfoModel",
                column: "TextInfoModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioInfoModel");
        }
    }
}
