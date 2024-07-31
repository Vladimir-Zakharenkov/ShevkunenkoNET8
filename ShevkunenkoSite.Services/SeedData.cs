//If you need to reset the database, then run this command in the ShevkunenkoSite.Services folder:
//dotnet ef database --startup-project ..\ShevkunenkoSite  drop --force --context SiteDbContext

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

                PageLoc = "index",

                BrowserConfig = "main.xml",

                Manifest = "main.json",

                OgType = "website",

                BackgroundFileModelId = new("E073BDDD694F4818A0C308DADB48DCD9"),

                ImageFileModelId = new("F718A63571D24BC21F4908DAE2B985EA")
            }
            );

            context.SaveChanges();
        }
    }
}