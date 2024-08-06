namespace ShevkunenkoSite.Areas.Admin.Pages.DBCRUD;

public class Icon_AddModel : PageModel
{
    private readonly IIconFileRepository _iconContext;
    public Icon_AddModel(IIconFileRepository iconContext) => _iconContext = iconContext;

    [BindProperty]
    [Required(ErrorMessage = "Укажите каталог")]
    [DataType(DataType.Text)]
    [Display(Name = "Каталог иконок images/pageicons/ :")]
    public string PathForIcon { get; set; } = string.Empty;

    [DataType(DataType.Upload)]
    [Display(Name = "Иконки для загрузки :")]
    public List<IFormFile>? IconFilesUpload { get; set; }

    //IconFileModel IconItem { get; set; } = null!;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            PathForIcon = PathForIcon.Trim().ToLower();

            string[] AllDirInPageiconsFolder = System.IO.Directory.GetDirectories(System.IO.Directory.GetCurrentDirectory() + nameof(DataConfig.IconFoldersPath));

            foreach (string dir in AllDirInPageiconsFolder)
            {
                if (dir.EndsWith(PathForIcon))
                {
                    ModelState.AddModelError("PathForIcon", $"Каталог « {PathForIcon} » уже существует");

                    return Page();
                }
            }

            if (IconFilesUpload == null || IconFilesUpload.Count == 0)
            {
                ModelState.AddModelError("IconFilesUpload", "Выберите файлы иконок");

                return Page();
            }
            else if (IconFilesUpload.Count != 29)
            {
                ModelState.AddModelError("IconFilesUpload", "Количество файлов должно быть - 29");

                return Page();
            }

            List<string> fileNames = new();

            foreach (var item in IconFilesUpload)
            {
                fileNames.Add(item.FileName);
            }

            string[] files = fileNames.ToArray();

            string[] partternFiles = new[]
            {
            "icon-48.png",
            "icon-96.png",
            "icon-144.png",
            "icon-192.png",
            "icon-256.png",
            "icon-384.png",
            "icon-512.png",
            "apple-touch-icon-57x57-precomposed.png",
            "apple-touch-icon-57x57.png",
            "apple-touch-icon-60x60-precomposed.png",
            "apple-touch-icon-60x60.png",
            "apple-touch-icon-72x72-precomposed.png",
            "apple-touch-icon-72x72.png",
            "apple-touch-icon-76x76-precomposed.png",
            "apple-touch-icon-76x76.png",
            "apple-touch-icon-114x114-precomposed.png",
            "apple-touch-icon-114x114.png",
            "apple-touch-icon-120x120-precomposed.png",
            "apple-touch-icon-120x120.png",
            "apple-touch-icon-144x144-precomposed.png",
            "apple-touch-icon-144x144.png",
            "apple-touch-icon-152x152-precomposed.png",
            "apple-touch-icon-152x152.png",
            "apple-touch-icon-167x167-precomposed.png",
            "apple-touch-icon-167x167.png",
            "apple-touch-icon-180x180-precomposed.png",
            "apple-touch-icon-180x180.png",
            "safari-pinned-tab.svg",
            "favicon.ico"
            };

            for (int i = 0; i < files.Length; i++)
            {
                if (!partternFiles.Contains(files[i]))
                {
                    ModelState.AddModelError("IconFilesUpload", $"Имя файла « {files[i]} » не соответствует шаблону");

                    return Page();
                }

            }

            string path = Path.Join(System.IO.Directory.GetCurrentDirectory(), nameof(DataConfig.IconFoldersPath), PathForIcon);

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            foreach (var icon in IconFilesUpload)
            {
                string fileNameWithPath = Path.Combine(path, icon.FileName);

                using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                await icon.CopyToAsync(stream);
            }

            foreach (var item in files)
            {
                IconFileModel IconItem = new()
                {
                    IconPath = "/" + PathForIcon
                };

                if (item.EndsWith(".png"))
                {

                    IReadOnlyList<MetadataExtractor.Directory> imageDirectories = ImageMetadataReader.ReadMetadata(Path.Combine(path, item));

                    foreach (var imageDirectory in imageDirectories)
                    {
                        foreach (var tag in imageDirectory.Tags)
                        {
                            if (tag.Name == "File Name") IconItem.IconFileName = tag.Description!;

                            if (tag.Name == "Expected File Name Extension") IconItem.IconFileNameExtension = tag.Description!;

                            if (tag.Name == "Detected MIME Type") IconItem.IconMimeType = tag.Description!;

                            if (tag.Name == "File Size") IconItem.IconFileSize = Convert.ToInt32(tag.Description![..tag.Description!.IndexOf(" ")]);

                            if (tag.Name == "Image Width") IconItem.IconWidth = Convert.ToInt32(tag.Description);

                            if (tag.Name == "Image Height") IconItem.IconHeight = Convert.ToInt32(tag.Description);
                        }
                    }

                }
                else if (item.EndsWith(".ico"))
                {
                    IReadOnlyList<MetadataExtractor.Directory> imageDirectories = ImageMetadataReader.ReadMetadata(Path.Combine(path, item));

                    foreach (var imageDirectory in imageDirectories)
                    {
                        foreach (var tag in imageDirectory.Tags)
                        {
                            if (tag.Name == "File Name") IconItem.IconFileName = tag.Description!;

                            if (tag.Name == "Expected File Name Extension") IconItem.IconFileNameExtension = tag.Description!;

                            if (tag.Name == "Detected MIME Type") IconItem.IconMimeType = tag.Description!;

                            if (tag.Name == "File Size") IconItem.IconFileSize = Convert.ToInt32(tag.Description![..tag.Description!.IndexOf(" ")]);
                        }
                    }

                    IconItem.IconWidth = 32;
                    IconItem.IconHeight = 32;
                }
                else
                {
                    var formFileform = IconFilesUpload.Where(f => f.FileName.EndsWith(".svg"));

                    IconItem.IconFileName = "safari-pinned-tab.svg";
                    IconItem.IconFileNameExtension = "svg";
                    IconItem.IconMimeType = "image/svg+xml";
                    IconItem.IconWidth = 2133;
                    IconItem.IconHeight = 2133;
                    IconItem.IconFileSize = Convert.ToInt32(IconFilesUpload.Find(f => f.FileName.EndsWith(".svg"))!.Length);
                }

                if (item.EndsWith(".ico"))
                {
                    IconItem.IconRel = "shortcut icon";
                    IconItem.IconType = "image/vnd.microsoft.icon";
                }
                else if (item.EndsWith(".svg"))
                {
                    IconItem.IconRel = "mask-icon";
                    IconItem.IconType = "image/svg+xml";
                }
                else if (item.Contains("apple") & item.Contains("precomposed"))
                {
                    IconItem.IconRel = "apple-touch-icon-precomposed";
                    IconItem.IconType = "image/png";
                }
                else if (item.Contains("apple") & !item.Contains("precomposed"))
                {
                    IconItem.IconRel = "apple-touch-icon";
                    IconItem.IconType = "image/png";
                }
                else
                {
                    IconItem.IconRel = "icon";
                    IconItem.IconType = "image/png";
                }

                await _iconContext.AddNewIconAsync(IconItem);
            }

            return RedirectToPage("/DBCRUD/Icon-List", new { area = "Admin" });
        }
        else
        {
            return Page();
        }
    }
}