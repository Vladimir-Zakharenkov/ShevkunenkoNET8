//If you need to reset the database, then run this command in the ShevkunenkoSite.Services folder:
//dotnet ef database --startup-project ..\ShevkunenkoSite  drop --force --context SiteDbContext

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShevkunenkoSite.Models;

namespace ShevkunenkoSite;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        SiteDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<SiteDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        if (!context.BackgroundFile.Any())
        {
            context.BackgroundFile.AddRange(
                new BackgroundFileModel
                {
                    LeftBackground = "FotoPlenka.png",
                    RightBackground = "FotoPlenka.png",
                    WebLeftBackground = "FotoPlenka.webp",
                    WebRightBackground = "FotoPlenka.webp"
                },
                new BackgroundFileModel
                {
                    LeftBackground = "biografy-left.png",
                    RightBackground = "biografy-right.png",
                    WebLeftBackground = "biografy-left.webp",
                    WebRightBackground = "biografy-right.webp"
                });

            context.SaveChanges();
        }

        if (!context.IconFile.Any())
        {
            context.IconFile.AddRange(
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-57x57.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 5556,
                    IconWidth = 57,
                    IconHeight = 57,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-57x57-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 6139,
                    IconWidth = 57,
                    IconHeight = 57,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-60x60.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 5501,
                    IconWidth = 60,
                    IconHeight = 60,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-60x60-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 6340,
                    IconWidth = 60,
                    IconHeight = 60,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-72x72.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 6205,
                    IconWidth = 72,
                    IconHeight = 72,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-72x72-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 7213,
                    IconWidth = 72,
                    IconHeight = 72,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-76x76.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 5886,
                    IconWidth = 76,
                    IconHeight = 76,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-76x76-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 7273,
                    IconWidth = 76,
                    IconHeight = 76,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-114x114.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 8213,
                    IconWidth = 114,
                    IconHeight = 114,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-114x114-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 10086,
                    IconWidth = 114,
                    IconHeight = 114,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-120x120.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 7780,
                    IconWidth = 120,
                    IconHeight = 120,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-120x120-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 10556,
                    IconWidth = 120,
                    IconHeight = 120,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-144x144.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 8549,
                    IconWidth = 144,
                    IconHeight = 144,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-144x144-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 12159,
                    IconWidth = 144,
                    IconHeight = 144,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-152x152.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 9771,
                    IconWidth = 152,
                    IconHeight = 152,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-152x152-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 12587,
                    IconWidth = 152,
                    IconHeight = 152,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-167x167.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 10587,
                    IconWidth = 167,
                    IconHeight = 167,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-167x167-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 13651,
                    IconWidth = 167,
                    IconHeight = 167,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-180x180.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 11160,
                    IconWidth = 180,
                    IconHeight = 180,
                    IconRel = "apple-touch-icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "apple-touch-icon-180x180-precomposed.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 16342,
                    IconWidth = 180,
                    IconHeight = 180,
                    IconRel = "apple-touch-icon-precomposed",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "favicon.ico",
                    IconFileNameExtension = "ico",
                    IconMimeType = "image/x-icon",
                    IconFileSize = 64107,
                    IconWidth = 32,
                    IconHeight = 32,
                    IconRel = "shortcut icon",
                    IconType = "image/vnd.microsoft.icon"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "icon-48.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 4598,
                    IconWidth = 48,
                    IconHeight = 48,
                    IconRel = "icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "icon-96.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 6515,
                    IconWidth = 96,
                    IconHeight = 96,
                    IconRel = "icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "icon-144.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 8549,
                    IconWidth = 144,
                    IconHeight = 144,
                    IconRel = "icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "icon-192.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 10662,
                    IconWidth = 192,
                    IconHeight = 192,
                    IconRel = "icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "icon-256.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 13792,
                    IconWidth = 256,
                    IconHeight = 256,
                    IconRel = "icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "icon-384.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 22266,
                    IconWidth = 384,
                    IconHeight = 384,
                    IconRel = "icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "icon-512.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 26432,
                    IconWidth = 512,
                    IconHeight = 512,
                    IconRel = "icon",
                    IconType = "image/png"
                },
                new IconFileModel
                {
                    IconPath = "main",
                    IconFileName = "safari-pinned-tab.svg",
                    IconFileNameExtension = "svg",
                    IconMimeType = "image/svg+xml",
                    IconFileSize = 2062,
                    IconWidth = 2133,
                    IconHeight = 2133,
                    IconRel = "mask-icon",
                    IconType = "image/svg+xml"
                });

            context.SaveChanges();
        }

        if (!context.ImageFile.Any())
        {
            context.ImageFile.AddRange(
                new ImageFileModel
                {
                    ImageCaption = "Изображение не найдено",
                    ImageDescription = "Изображение не найдено в базе данных сайта.",
                    ImageAltTitle = "изображение не найдено",
                    ImagePath = "/images/common",
                    ImageFileName = "no-image.png",
                    ImageFileNameExtension = "png",
                    ImageMimeType = "image/png",
                    ImageFileSize = 232866,
                    ImageWidth = 720,
                    ImageHeight = 540,
                    IconFileName = "no-image-300.png",
                    IconFileNameExtension = "png",
                    IconMimeType = "image/png",
                    IconFileSize = 41667,
                    IconWidth = 300,
                    IconHeight = 225,
                    ImageHDFileName = null,
                    ImageHDFileSize = 0,
                    ImageHDHeight = 0,
                    ImageHDMimeType = null,
                    ImageHDNameExtension = null,
                    ImageHDWidth = 0,
                    Icon100FileName = "no-image-100.png",
                    Icon100FileNameExtension = "png",
                    Icon100FileSize = 9360,
                    Icon100Height = 75,
                    Icon100MimeType = "image/png",
                    Icon100Width = 100,
                    Icon200FileName = "no-image-200.png",
                    Icon200FileNameExtension = "png",
                    Icon200FileSize = 22526,
                    Icon200Height = 150,
                    Icon200MimeType = "image/png",
                    Icon200Width = 200,
                    SearchFilter = "Админ,",
                    WebImageFileName = "no-image.webp",
                    WebImageFileSize = 4990,
                    WebImageFileNameExtension = "webp",
                    WebImageMimeType = "image/webp",
                    WebIconFileName = "no-image-300.webp",
                    WebIconFileNameExtension = "webp",
                    WebIconFileSize = 2156,
                    WebIconMimeType = "image/webp",
                    WebIcon200FileName = "no-image-200.webp",
                    WebIcon200FileNameExtension = "webp",
                    WebIcon200FileSize = 1574,
                    WebIcon200MimeType = "image/webp",
                    WebIcon100FileName = "no-image-100.webp",
                    WebIcon100FileNameExtension = "webp",
                    WebIcon100FileSize = 922,
                    WebIcon100MimeType = "image/webp",
                    WebImageHDFileName = null,
                    WebImageHDFileSize = 0,
                    WebImageHDMimeType = "image/webp",
                    WebImageHDNameExtension = "webp",
                    WebImageHDHeight = 0,
                    WebImageHDWidth = 0,
                    WebImageHeight = 540,
                    WebImageWidth = 720,
                    WebIconHeight = 225,
                    WebIconWidth = 300,
                    WebIcon200Height = 150,
                    WebIcon200Width = 200,
                    WebIcon100Height = 75,
                    WebIcon100Width = 100
                });

            context.SaveChanges();
        }

        if (!context.PageInfo.Any())
        {
            context.PageInfo.AddRange(
            new PageInfoModel
            {
                PageTitle = "Сайт памяти Сергея Шевкуненко",
                PageDescription = "Эта история по-своему уникальна и практически не имеет " +
                    "аналогов в истории российского кинематографа. Подававший большие надежды " +
                    "актер волею судьбы угодил в тюрьму и довольно быстро добился славы и " +
                    "признания совсем в другой среде - уголовной. Последней ступенькой, " +
                    "на которую сумел забраться в преступной иерархии этот бывший актер, " +
                    "была должность «положенца», которая предшествует самому высокому " +
                    "титулу в уголовной среде - вора в законе. Имя этого человека - " +
                    "Сергей Шевкуненко.",
                PageKeyWords = "сергей шевкуненко,сергей шевкуненко фото,сергей " +
                    "шевкуненко криминальная,шевкуненко сергей юрьевич,сергей шевкуненко биография," +
                    "сергей шевкуненко криминальная биография,сергей шевкуненко убийство," +
                    "сергей шевкуненко фото убийство,сергей шевкуненко жена,сергей шевкуненко википедия," +
                    "фильм криминальная звезда сергей шевкуненко,сергей шевкуненко похороны," +
                    "актер сергей шевкуненко,памяти сергей шевкуненко,сергей шевкуненко документальный фильм," +
                    "сергей шевкуненко документальный фильм криминальная звезда,криминальная звезда " +
                    "документальный фильм памяти сергея шевкуненко,сергей шевкуненко видео," +
                    "сестра сергея шевкуненко,шевкуненко сергей юрьевич похороны,сергей шевкуненко кортик," +
                    "елена шевкуненко жена сергея шевкуненко,сергей шевкуненко могила," +
                    "сергей шевкуненко смерть,сергей шевкуненко похороны видео,сергей шевкуненко личная жизнь," +
                    "сергей шевкуненко фото смерть,шевкуненко сергей юрьевич вспоминают друзья," +
                    "актер сергей шевкуненко биография,судьба сергея шевкуненко," +
                    "сергей шевкуненко фото перед смертью,сергей шевкуненко криминал," +
                    "шевкуненко сергей юрьевич криминал",
                PageLastmod = DateTime.Today,
                PageLoc = "/index",
                BrowserConfig = "main.xml",
                Manifest = "main.json",
                OgType = "website",
                BackgroundFileModelId = new("E073BDDD694F4818A0C308DADB48DCD9"),
                ImageFileModelId = new("893CB76A-927A-432B-9E60-08DAF54685FF"),
                PageArea = string.Empty,
                PageCardText = string.Empty,
                Changefreq = "monthly",
                Priority = "0.9",
                PageIconPath = "/main",
                BrowserConfigFolder = "/main",
                Action = "/index",
                Controller = "/shevkunenko",
                RoutData = string.Empty,
                PageAsRazorPage = false,
                PagePathNickName = string.Empty,
                //PageFullPath = "/shevkunenko/index", вычисляемое свойство
                //PageFullPathWithData = "/shevkunenko/index", вычисляемое свойство
                //PagePathNickNameWithData = string.Empty, вычисляемое свойство
                PageLinks = true,
                RefPages = string.Empty,
                PageFilter = string.Empty,
                PageFilterOut = string.Empty
            }
            );

            context.SaveChanges();
        }

        if (!context.MovieFile.Any())
        {
            context.MovieFile.AddRange(
            new MovieFileModel
            {
                MovieCaption = "Кортик - 1",
                MovieDescriptionHtml = "<p>Гражданская война в России.</p>\r\n<p>В небольшом городке Ревске у бабушки с дедушкой гостит их внук, школьник из Москвы Миша Поляков. Ему и его другу Генке Петрову попадает в руки старинный кортик, принадлежавший офицеру с линкора «Императрица Мария».</p>\r\n<p>С кортиком связана какая-то тайна — его ищет бывший офицер с погибшего линкора — белогвардеец, а ныне главарь банды, Валерий Сигизмундович Никитский. Мишин друг — командир Красной Армии Полевой — пытается во что бы то ни стало сберечь кортик, в чём Миша ему и помогает.</p>\r\n<p>Миша и Генка едут в Москву…</p>",
                MovieDescriptionForSchemaOrg = "Гражданская война в России. В небольшом городке Ревске у бабушки с дедушкой гостит их внук, школьник из Москвы Миша Поляков. Ему и его другу Генке Петрову попадает в руки старинный кортик, принадлежавший офицеру с линкора «Императрица Мария». С кортиком связана какая-то тайна — его ищет бывший офицер с погибшего линкора — белогвардеец, а ныне главарь банды, Валерий Сигизмундович Никитский. Мишин друг — командир Красной Армии Полевой — пытается во что бы то ни стало сберечь кортик, в чём Миша ему и помогает. Миша и Генка едут в Москву…",
                MovieDuration = TimeSpan.Parse("01:08:39.9783333"),
                MovieWidth = 1560,
                MovieHeight = 1080,
                MovieScreenFormat = "4X3",
                MovieFileName = "kortik-1-seriya-(hd).mp4",
                MovieFileExtension = "mp4",
                MovieMimeType = "video/mp4",
                MovieFileSize = 1402897577,
                MovieDateCreated = DateTime.ParseExact("1973-01-01 00:00:00.000", "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture),
                MovieDatePublished = DateTime.ParseExact("1974-06-04 00:00:00.000", "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture),
                MovieUploadDate = DateTime.ParseExact("2023-02-04 00:00:00.000", "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture),
                MovieIsFamilyFriendly = true,
                MovieInLanguage2 = string.Empty,
                MovieРroductionCompany = "Беларусьфильм",
                MovieDirector1 = "Николай Калинин",
                MovieDirector2 = string.Empty,
                MovieMusicBy = "Станислав Пожлаков",
                MovieGenre = "экранизация, детский, приключения",
                MovieActor01 = "Сергей Шевкуненко",
                MovieActor02 = "Владимир Дичковский",
                MovieActor03 = "Игорь Шульженко",
                MovieActor04 = "Михаил Голубович",
                MovieActor05 = "Эммануил Виторган",
                MovieActor06 = "Зоя Фёдорова",
                MovieActor07 = "Леонид Кмит",
                MovieActor08 = "Наталья Чемодурова",
                MovieActor09 = "Виктор Сергачёв",
                MovieActor10 = "Александр Примако",
                MovieContentUrl = new("https://sergeyshef.ru/video/kortik-1-seriya-(hd).mp4"),
                MovieCaptionForOnline = "КОРТИК - 1 серия",
                MovieYouTube = new("https://www.youtube.com/embed/xJ4tfm9kdqA"),
                MovieVkVideo = new("https://vk.com/video_ext.php?oid=705530674&id=456239292&hash=a0df9c56c3e9bb50"),
                MovieMailRuVideo = new("https://my.mail.ru/video/embed/30262781949248138"),
                MovieOkVideo = new("https://ok.ru/videoembed/4326441355910"),
                MovieYandexDiskVideo = new("https://disk.yandex.ru/i/vNuICTfUCpbnPQ"),
                MovieKinoTeatrRu = new("https://www.kino-teatr.ru/kino/movie/sov/3199/annot/"),
                MovieKinoPoisk = new("https://www.kinopoisk.ru/series/79844/"),
                MovieImbd = new("https://www.imdb.com/title/tt0069600/?ref_=nv_sr_srsg_0"),
                MovieNote = string.Empty,
                MovieInLanguage1 = "ru",
                MovieSubtitles1 = string.Empty,
                MovieSubtitles2 = string.Empty,
                PageInfoModelId = new("F654CC14-FA40-4617-8844-08DB01E1D51B"),
                MovieAdult = false,
                MoviePart = 1,
                MovieTotalParts = 3,
                ImageFileModelId = new("ABE5FA25-C882-4CD7-C558-08DB10624E7D"),
                MoviePoster = null,
                MovieInMainList = true,
                PageInfoModelIdForSeries = new("6D727655-B263-406D-7865-08DB0178B531"),
                SearchFilter = "Шевкуненко, Кортик,",
                FullMovieID = null,
                SearchFilter1 = "Кортик,",
                SearchFilter2 = "Шевкуненко,",
                HeadTitleForVideoLinks1 = string.Empty,
                HeadTitleForVideoLinks2 = "Фильмы с участием Сергея Шевкуненко",
                HeadTitleForVideoLinks3 = "Сергей Шевкуненко - роли в кино",
                IconType1 = "webicon300",
                IconType2 = "webicon300",
                IconType3 = "webicon300",
                IsImage1 = true,
                IsImage2 = false,
                IsImage3 = false,
                MoviePosterGuid = new("962F0E6D - 2B5A - 48F7 - 8D8C - 08DB0A677DAF"),
                SearchFilter3 = "Шевкуненко,",
                IsPartsMoreOne1 = false,
                IsPartsMoreOne2 = true,
                IsPartsMoreOne3 = true,
                AllMoviesFromDB1 = true,
                AllMoviesFromDB2 = false,
                AllMoviesFromDB3 = false,
                Carousel = true,
                FramesAroundMovie = string.Empty,
                ImageForHeadSeriesImageFileModelId = null,
                SeriesSearchFilter = string.Empty,
                TopicGuidList = "59019054-7df0-42b7-7fa5-08dca3df7399"
            }
            );

            context.SaveChanges();
        }

        if (!context.TopicMovie.Any())
        {
            context.TopicMovie.AddRange(
                new TopicMovieModel
                {
                    TopicDescription = "Фильмы с участием Сергея Шевкунеко",
                    TopicHeadPage = "Фильмы с участием Сергея Шевкунеко",
                    ImageForRef = false,
                    IconTypeForRef = "webicon300",
                    GeneralPageForMovieEpisodes = true,
                    NumberOfLinksPerPage = 2
                }
                );

            context.SaveChanges();
        }

        if (!context.Access.Any())
        {
            context.Access.AddRange(
                new AccessModel
                {
                    Email = "krivosein623@gmail.com",
                    Password = "sergey623_"
                }
                );

            context.SaveChanges();
        }
    }
}