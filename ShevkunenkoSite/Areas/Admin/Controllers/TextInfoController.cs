using System.Text;
using System.Xml;

namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class TextInfoController(
    ITextInfoRepository textContext,
    IBooksAndArticlesRepository bookContext,
    IMovieFileRepository movieContext,
    IWebHostEnvironment hostEnvironment) : Controller
{
    private readonly string rootPath = hostEnvironment.WebRootPath;

    #region Список текстов

    public int textsPerPage = 16;

    public IActionResult Index(string? textSearchString, int pageNumber = 1)
    {
        var allTexts = textContext.Texts.TextSearch(textSearchString, hostEnvironment);

        return View(new TextInfoViewModel
        {
            AllTexts = [.. allTexts
                     .Skip((pageNumber - 1) * textsPerPage)
                     .Take(textsPerPage)],

            PagingInfo = new PagingInfoViewModel
            {
                CurrentPage = pageNumber,
                ItemsPerPage = textsPerPage,
                TotalItems = allTexts.Count()
            },

            TextSearchString = textSearchString ?? string.Empty
        });
    }

    #endregion

    #region Информация о тексте

    public async Task<IActionResult> DetailsText(Guid? textId)
    {
        if (textId.HasValue & await textContext.Texts.Where(p => p.TextInfoModelId == textId).AnyAsync())
        {
            var textItem = await textContext.Texts
                .Include(book => book.BooksAndArticlesModel)
                .AsNoTracking()
                .FirstAsync(p => p.TextInfoModelId == textId);

            using StreamReader clearText = new(rootPath + DataConfig.TextsFolderPath + textItem.TxtFileName);

            using StreamReader htmlText = new(rootPath + DataConfig.TextsFolderPath + textItem.HtmlFileName);

            return View(new DetailsTextViewModel
            {
                TextInfoModelId = textItem.TextInfoModelId,
                TextDescription = textItem.TextDescription,
                TxtFileName = textItem.TxtFileName,
                HtmlFileName = textItem.HtmlFileName,
                TxtFileSize = textItem.TxtFileSize,
                HtmlFileSize = textItem.HtmlFileSize,
                BooksAndArticlesModelId = textItem.BooksAndArticlesModelId,
                BooksAndArticlesModel = textItem.BooksAndArticlesModel,
                SequenceNumber = textItem.SequenceNumber,
                ClearText = clearText.ReadToEnd(),
                HtmlText = htmlText.ReadToEnd()
            });
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion

    #region Добавить текст

    [HttpGet]
    public ViewResult AddText()
    {
        AddTextInfoViewModel newText = new();

        return View(newText);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddText(AddTextInfoViewModel addText)
    {
        if (ModelState.IsValid)
        {
            #region Описание текста

            if (await textContext.Texts.Where(file => file.TextDescription == addText.TextDescription).AnyAsync())
            {
                ModelState.AddModelError("TextDescription", $"Описание «{addText.TextDescription}» уже существует в БД");

                return View(addText);
            }

            addText.TextDescription = addText.TextDescription.Trim();

            #endregion

            #region TXT файл

            if (addText.TxtFileFormFile != null)
            {
                if (!addText.TxtFileFormFile.FileName.EndsWith(".txt"))
                {
                    ModelState.AddModelError("TxtFileFormFile", "Выберите файл формата «.txt»");

                    return View(addText);
                }

                if (await textContext.Texts.Where(file => file.TxtFileName == addText.TxtFileFormFile.FileName).AnyAsync())
                {
                    ModelState.AddModelError("TxtFileFormFile", $"Файл с именем «{addText.TxtFileFormFile.FileName}» уже существует в БД");

                    return View(addText);
                }

                addText.TxtFileName = addText.TxtFileFormFile.FileName;
                addText.TxtFileSize = (int)addText.TxtFileFormFile.Length;
            }

            #endregion

            #region HTML файл

            if (addText.HtmlFileFormFile != null)
            {
                if (!addText.HtmlFileFormFile.FileName.EndsWith(".html"))
                {
                    ModelState.AddModelError("HtmlFileFormFile", "Выберите файл формата «.html»");

                    return View(addText);
                }

                if (await textContext.Texts.Where(file => file.HtmlFileName == addText.HtmlFileFormFile.FileName).AnyAsync())
                {
                    ModelState.AddModelError("HtmlFileFormFile", $"Файл с именем «{addText.HtmlFileFormFile.FileName}» уже существует в БД");

                    return View(addText);
                }

                addText.HtmlFileName = addText.HtmlFileFormFile.FileName;
                addText.HtmlFileSize = (int)addText.HtmlFileFormFile.Length;
            }

            #endregion

            #region Связанная книга (статья)

            if (addText.RefForBookOrArticle != null)
            {
                _ = addText.RefForBookOrArticle.Trim();

                if (await bookContext.BooksAndArticles.Where(book => book.CaptionOfText.ToLower() == addText.RefForBookOrArticle.ToLower()).AnyAsync())
                {
                    var book = await bookContext.BooksAndArticles.FirstAsync(book => book.CaptionOfText == addText.RefForBookOrArticle);

                    addText.BooksAndArticlesModelId = book.BooksAndArticlesModelId;
                }
                else
                {
                    ModelState.AddModelError("RefForBookOrArticle", $"Книги (статьи) «{addText.RefForBookOrArticle}» не найдено в БД");

                    return View(addText);
                }
            }
            else
            {
                addText.BooksAndArticlesModelId = null;
            }

            #endregion

            #region Номер страницы

            if (addText.BooksAndArticlesModelId != null & addText.SequenceNumber == null)
            {
                ModelState.AddModelError("SequenceNumber", $"Если указана книга (статья), нужно указать страницу");

                return View(addText);
            }
            else if (await textContext.Texts.Where(text => text.BooksAndArticlesModelId == addText.BooksAndArticlesModelId & text.SequenceNumber == addText.SequenceNumber).AnyAsync())
            {
                ModelState.AddModelError("SequenceNumber", $"Для книги (статьи) «{addText.RefForBookOrArticle}», страница «{addText.SequenceNumber}» уже существует");

                return View(addText);
            }
            else
            {
                _ = addText.SequenceNumber;
            }

            #endregion

            #region Копировать файлы и добавить в БД

            if (addText.TxtFileFormFile != null)
            {
                using FileStream txtStream = new(rootPath + DataConfig.TextsFolderPath + addText.TxtFileFormFile.FileName, FileMode.Create);

                await addText.TxtFileFormFile.CopyToAsync(txtStream);
            }

            if (addText.HtmlFileFormFile != null)
            {
                using FileStream htmlStream = new(rootPath + DataConfig.TextsFolderPath + addText.HtmlFileFormFile.FileName, FileMode.Create);

                await addText.HtmlFileFormFile.CopyToAsync(htmlStream);
            }

            await textContext.AddNewTextAsync(addText);

            #endregion

            #region Открытие параметров добавленного файла

            var newText = await textContext.Texts.FirstAsync(file => file.TxtFileName == addText.TxtFileName);

            return RedirectToAction(nameof(DetailsText), new { textId = newText.TextInfoModelId });

            #endregion
        }
        else
        {
            return View();
        }
    }

    #endregion

    #region Изменить текст

    [HttpGet]
    public async Task<IActionResult> EditText(Guid? textId)
    {
        TextInfoModel editText = new();

        if (textId.HasValue & await textContext.Texts.Where(i => i.TextInfoModelId == textId).AnyAsync())
        {
            editText = await textContext.Texts.FirstAsync(i => i.TextInfoModelId == textId);

            using StreamReader clearText = new(rootPath + DataConfig.TextsFolderPath + editText.TxtFileName);

            using StreamReader htmlText = new(rootPath + DataConfig.TextsFolderPath + editText.HtmlFileName);

            return View(new DetailsTextViewModel
            {
                TextInfoModelId = editText.TextInfoModelId,
                TextDescription = editText.TextDescription,
                TxtFileName = editText.TxtFileName,
                HtmlFileName = editText.HtmlFileName,
                TxtFileSize = editText.TxtFileSize,
                HtmlFileSize = editText.HtmlFileSize,
                ClearText = clearText.ReadToEnd(),
                HtmlText = htmlText.ReadToEnd()
            });
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditText(DetailsTextViewModel textItem)
    {
        if (ModelState.IsValid)
        {
            TextInfoModel textUpdate = await textContext.Texts.FirstAsync(txt => txt.TextInfoModelId == textItem.TextInfoModelId);

            #region Описание текста

            textUpdate.TextDescription = textItem.TextDescription.Trim();

            #endregion

            #region Текст без разметки (txt)

            if (!string.IsNullOrEmpty(textItem.ClearText))
            {
                _ = textItem.ClearText.Trim();

                // путь к файлу 
                var path = rootPath + DataConfig.TextsFolderPath + textItem.TxtFileName;

                // полная перезапись файла 
                using StreamWriter writer = new(path, false, new UTF8Encoding(true));

                await writer.WriteLineAsync(textItem.ClearText);

                writer.Close();

                // новый размер файла txt
                textUpdate.TxtFileSize = (int)new System.IO.FileInfo(path).Length;
            }

            #endregion

            #region Текст с разметкой (html)

            if (!string.IsNullOrEmpty(textItem.HtmlText))
            {
                _ = textItem.HtmlText.Trim();

                // путь к файлу 
                var path = rootPath + DataConfig.TextsFolderPath + textItem.HtmlFileName;

                // полная перезапись файла 
                using StreamWriter writer = new(path, false, new UTF8Encoding(true));

                await writer.WriteLineAsync(textItem.HtmlText);

                writer.Close();

                // новый размер файла html
                textUpdate.HtmlFileSize = (int)new System.IO.FileInfo(path).Length;
            }

            #endregion

            #region Сохранить и перейти к DetailsText

            await textContext.SaveChangesInTextAsync();

            return RedirectToAction(nameof(DetailsText), new { textId = textUpdate.TextInfoModelId });

            #endregion
        }
        else
        {
            return View(textItem);
        }
    }

    #endregion

    #region Удалить текст

    [HttpGet]
    public async Task<IActionResult> DeleteText(Guid? textId)
    {
        TextInfoModel deleteText = new();

        if (textId.HasValue & await textContext.Texts.Where(i => i.TextInfoModelId == textId).AnyAsync())
        {
            deleteText = await textContext.Texts.FirstAsync(i => i.TextInfoModelId == textId);

            using StreamReader clearText = new(rootPath + DataConfig.TextsFolderPath + deleteText.TxtFileName);

            using StreamReader htmlText = new(rootPath + DataConfig.TextsFolderPath + deleteText.HtmlFileName);

            return View(new DetailsTextViewModel
            {
                TextInfoModelId = deleteText.TextInfoModelId,
                TextDescription = deleteText.TextDescription,
                TxtFileName = deleteText.TxtFileName,
                HtmlFileName = deleteText.HtmlFileName,
                TxtFileSize = deleteText.TxtFileSize,
                HtmlFileSize = deleteText.HtmlFileSize,
                ClearText = clearText.ReadToEnd(),
                HtmlText = htmlText.ReadToEnd()
            });
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteText(TextInfoModel? deleteText)
    {
        if (deleteText != null)
        {
            if (await movieContext.MovieFiles.Where(i => i.TextInfoModelId == deleteText.TextInfoModelId).AnyAsync())
            {
                deleteText = await textContext.Texts.FirstAsync(i => i.TextInfoModelId == deleteText.TextInfoModelId);

                using StreamReader clearText = new(rootPath + DataConfig.TextsFolderPath + deleteText.TxtFileName);

                using StreamReader htmlText = new(rootPath + DataConfig.TextsFolderPath + deleteText.HtmlFileName);

                return View(new DetailsTextViewModel
                {
                    TextInfoModelId = deleteText.TextInfoModelId,
                    TextDescription = deleteText.TextDescription,
                    TxtFileName = deleteText.TxtFileName,
                    HtmlFileName = deleteText.HtmlFileName,
                    TxtFileSize = deleteText.TxtFileSize,
                    HtmlFileSize = deleteText.HtmlFileSize,
                    ClearText = clearText.ReadToEnd(),
                    HtmlText = htmlText.ReadToEnd(),
                    RefInMovies = "Ссылка на файл в базе фильмов сайта!"
                });

            }

            if (await textContext.Texts.Where(i => i.TextInfoModelId == deleteText.TextInfoModelId).AnyAsync())
            {
                deleteText = await textContext.Texts.FirstAsync(i => i.TextInfoModelId == deleteText.TextInfoModelId);

                var txtPath = rootPath + DataConfig.TextsFolderPath + deleteText.TxtFileName;

                FileInfo txtFile = new(txtPath);

                if (txtFile.Exists)
                {
                    var txtMoveToPath = rootPath + DataConfig.ArchiveTextsFolderPath + deleteText.TxtFileName;

                    txtFile.MoveTo(txtMoveToPath, true);
                }

                var htmlPath = rootPath + DataConfig.TextsFolderPath + deleteText.HtmlFileName;

                FileInfo htmlFile = new(htmlPath);

                if (htmlFile.Exists)
                {
                    var htmlMoveToPath = rootPath + DataConfig.ArchiveTextsFolderPath + deleteText.HtmlFileName;

                    htmlFile.MoveTo(htmlMoveToPath, true);
                }

                await textContext.DeleteTextAsync(deleteText.TextInfoModelId);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    #endregion
}