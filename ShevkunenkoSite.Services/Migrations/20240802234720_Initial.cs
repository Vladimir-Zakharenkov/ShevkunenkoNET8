using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShevkunenkoSite.Services.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    AccessModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access", x => x.AccessModelId);
                });

            migrationBuilder.CreateTable(
                name: "BackgroundFile",
                columns: table => new
                {
                    BackgroundFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeftBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RightBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebLeftBackground = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebRightBackground = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundFile", x => x.BackgroundFileId);
                });

            migrationBuilder.CreateTable(
                name: "IconFile",
                columns: table => new
                {
                    IconFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IconPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconFileNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconMimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconFileSize = table.Column<long>(type: "bigint", nullable: false),
                    IconWidth = table.Column<long>(type: "bigint", nullable: false),
                    IconHeight = table.Column<long>(type: "bigint", nullable: false),
                    IconRel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IconFile", x => x.IconFileId);
                });

            migrationBuilder.CreateTable(
                name: "ImageFile",
                columns: table => new
                {
                    ImageFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageCaption = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ImageDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ImageAltTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchFilter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFileNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageMimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageFileSize = table.Column<long>(type: "bigint", nullable: false),
                    ImageWidth = table.Column<long>(type: "bigint", nullable: false),
                    ImageHeight = table.Column<long>(type: "bigint", nullable: false),
                    WebImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebImageFileNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebImageMimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebImageFileSize = table.Column<long>(type: "bigint", nullable: false),
                    WebImageWidth = table.Column<long>(type: "bigint", nullable: false),
                    WebImageHeight = table.Column<long>(type: "bigint", nullable: false),
                    IconFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconFileNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconMimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconFileSize = table.Column<long>(type: "bigint", nullable: true),
                    IconWidth = table.Column<long>(type: "bigint", nullable: true),
                    IconHeight = table.Column<long>(type: "bigint", nullable: true),
                    WebIconFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebIconFileNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebIconMimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebIconFileSize = table.Column<long>(type: "bigint", nullable: false),
                    WebIconWidth = table.Column<long>(type: "bigint", nullable: false),
                    WebIconHeight = table.Column<long>(type: "bigint", nullable: false),
                    Icon200FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon200FileNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon200MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon200FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Icon200Width = table.Column<long>(type: "bigint", nullable: true),
                    Icon200Height = table.Column<long>(type: "bigint", nullable: true),
                    WebIcon200FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebIcon200FileNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebIcon200MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebIcon200FileSize = table.Column<long>(type: "bigint", nullable: false),
                    WebIcon200Width = table.Column<long>(type: "bigint", nullable: false),
                    WebIcon200Height = table.Column<long>(type: "bigint", nullable: false),
                    Icon100FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon100FileNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon100MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon100FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Icon100Width = table.Column<long>(type: "bigint", nullable: true),
                    Icon100Height = table.Column<long>(type: "bigint", nullable: true),
                    WebIcon100FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebIcon100FileNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebIcon100MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebIcon100FileSize = table.Column<long>(type: "bigint", nullable: false),
                    WebIcon100Width = table.Column<long>(type: "bigint", nullable: false),
                    WebIcon100Height = table.Column<long>(type: "bigint", nullable: false),
                    ImageHDFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageHDNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageHDMimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageHDFileSize = table.Column<long>(type: "bigint", nullable: true),
                    ImageHDWidth = table.Column<long>(type: "bigint", nullable: true),
                    ImageHDHeight = table.Column<long>(type: "bigint", nullable: true),
                    WebImageHDFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebImageHDNameExtension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebImageHDMimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebImageHDFileSize = table.Column<long>(type: "bigint", nullable: false),
                    WebImageHDWidth = table.Column<long>(type: "bigint", nullable: false),
                    WebImageHDHeight = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageFile", x => x.ImageFileId);
                });

            migrationBuilder.CreateTable(
                name: "TopicMovie",
                columns: table => new
                {
                    TopicMovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopicDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopicHeadPage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageForRef = table.Column<bool>(type: "bit", nullable: true),
                    IconTypeForRef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneralPageForMovieEpisodes = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfLinksPerPage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicMovie", x => x.TopicMovieId);
                });

            migrationBuilder.CreateTable(
                name: "PageInfo",
                columns: table => new
                {
                    PageInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageAsRazorPage = table.Column<bool>(type: "bit", nullable: false),
                    PageArea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoutData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageLoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PagePathNickName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PagePathNickNameWithData = table.Column<string>(type: "nvarchar(max)", nullable: false, computedColumnSql: "[PagePathNickName] + [RoutData]"),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageKeyWords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageLastmod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PageFullPath = table.Column<string>(type: "nvarchar(max)", nullable: false, computedColumnSql: "[PageArea] + [Controller] + [PageLoc]"),
                    PageFullPathWithData = table.Column<string>(type: "nvarchar(max)", nullable: false, computedColumnSql: "[PageArea] + [Controller] + [PageLoc] + [RoutData]"),
                    BrowserConfig = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrowserConfigFolder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Manifest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OgType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageCardText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Changefreq = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PageIconPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageFilter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageFilterOut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackgroundFileModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageFileModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageLinks = table.Column<bool>(type: "bit", nullable: false),
                    RefPages = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageInfo", x => x.PageInfoId);
                    table.ForeignKey(
                        name: "FK_PageInfo_BackgroundFile_BackgroundFileModelId",
                        column: x => x.BackgroundFileModelId,
                        principalTable: "BackgroundFile",
                        principalColumn: "BackgroundFileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageInfo_ImageFile_ImageFileModelId",
                        column: x => x.ImageFileModelId,
                        principalTable: "ImageFile",
                        principalColumn: "ImageFileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieFile",
                columns: table => new
                {
                    MovieFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieCaption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieInMainList = table.Column<bool>(type: "bit", nullable: false),
                    MovieCaptionForOnline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieDescriptionForSchemaOrg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieDescriptionHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    MovieWidth = table.Column<long>(type: "bigint", nullable: false),
                    MovieHeight = table.Column<long>(type: "bigint", nullable: false),
                    MovieFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieFileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieMimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieFileSize = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    MovieScreenFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieGenre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchFilter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TopicGuidList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieIsFamilyFriendly = table.Column<bool>(type: "bit", nullable: false),
                    MovieAdult = table.Column<bool>(type: "bit", nullable: false),
                    FullMovieID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MovieDateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovieDatePublished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovieUploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovieInLanguage1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieInLanguage2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieSubtitles1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieSubtitles2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieРroductionCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieDirector1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieDirector2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieMusicBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor01 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor02 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor03 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor04 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor05 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor06 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor07 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor08 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor09 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieActor10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MovieContentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieYouTube = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieVkVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieMailRuVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieOkVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieYandexDiskVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieKinoTeatrRu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieKinoPoisk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieImbd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieTotalParts = table.Column<long>(type: "bigint", nullable: false),
                    MoviePart = table.Column<long>(type: "bigint", nullable: false),
                    SeriesSearchFilter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageInfoModelIdForSeries = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageForHeadSeriesImageFileModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PageInfoModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageFileModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MoviePoster = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoviePosterGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Carousel = table.Column<bool>(type: "bit", nullable: false),
                    FramesAroundMovie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadTitleForVideoLinks1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchFilter1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsImage1 = table.Column<bool>(type: "bit", nullable: true),
                    IconType1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPartsMoreOne1 = table.Column<bool>(type: "bit", nullable: false),
                    AllMoviesFromDB1 = table.Column<bool>(type: "bit", nullable: false),
                    HeadTitleForVideoLinks2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchFilter2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsImage2 = table.Column<bool>(type: "bit", nullable: true),
                    IconType2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPartsMoreOne2 = table.Column<bool>(type: "bit", nullable: false),
                    AllMoviesFromDB2 = table.Column<bool>(type: "bit", nullable: false),
                    HeadTitleForVideoLinks3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchFilter3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsImage3 = table.Column<bool>(type: "bit", nullable: true),
                    IconType3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPartsMoreOne3 = table.Column<bool>(type: "bit", nullable: false),
                    AllMoviesFromDB3 = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieFile", x => x.MovieFileId);
                    table.ForeignKey(
                        name: "FK_MovieFile_ImageFile_ImageFileModelId",
                        column: x => x.ImageFileModelId,
                        principalTable: "ImageFile",
                        principalColumn: "ImageFileId");
                    table.ForeignKey(
                        name: "FK_MovieFile_ImageFile_ImageForHeadSeriesImageFileModelId",
                        column: x => x.ImageForHeadSeriesImageFileModelId,
                        principalTable: "ImageFile",
                        principalColumn: "ImageFileId");
                    table.ForeignKey(
                        name: "FK_MovieFile_PageInfo_PageInfoModelId",
                        column: x => x.PageInfoModelId,
                        principalTable: "PageInfo",
                        principalColumn: "PageInfoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieFile_ImageFileModelId",
                table: "MovieFile",
                column: "ImageFileModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieFile_ImageForHeadSeriesImageFileModelId",
                table: "MovieFile",
                column: "ImageForHeadSeriesImageFileModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieFile_PageInfoModelId",
                table: "MovieFile",
                column: "PageInfoModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_BackgroundFileModelId",
                table: "PageInfo",
                column: "BackgroundFileModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PageInfo_ImageFileModelId",
                table: "PageInfo",
                column: "ImageFileModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Access");

            migrationBuilder.DropTable(
                name: "IconFile");

            migrationBuilder.DropTable(
                name: "MovieFile");

            migrationBuilder.DropTable(
                name: "TopicMovie");

            migrationBuilder.DropTable(
                name: "PageInfo");

            migrationBuilder.DropTable(
                name: "BackgroundFile");

            migrationBuilder.DropTable(
                name: "ImageFile");
        }
    }
}
