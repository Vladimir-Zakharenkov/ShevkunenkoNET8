namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class TextInfoController(ITextInfoRepository textContext) : Controller
{
    #region Список текстов

    public int textsPerPage = 16;

    public IActionResult Index(string? textSearchString, int pageNumber = 1)
    {
        var allTexts = textContext.Texts.TextSearch(textSearchString);

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
                 .AsNoTracking()
                 .FirstAsync(p => p.TextInfoModelId == textId);

            using StreamReader clearText = new(System.IO.Directory.GetCurrentDirectory() + DataConfig.TextsFolderPath + textItem.TxtFileName);

            using StreamReader htmlText = new(System.IO.Directory.GetCurrentDirectory() + DataConfig.TextsFolderPath + textItem.HtmlFileName);

            return View(new DetailsTextViewModel
            {
                TextInfoModelId = textItem.TextInfoModelId,
                TextDescription = textItem.TextDescription,
                TxtFileName = textItem.TxtFileName,
                HtmlFileName = textItem.HtmlFileName,
                TxtFileSize = textItem.TxtFileSize,
                HtmlFileSize = textItem.HtmlFileSize,
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

            addText.TextDescription = addText.TextDescription.Trim();

            if (await textContext.Texts.Where(file => file.TextDescription == addText.TextDescription).AnyAsync())
            {
                ModelState.AddModelError("TextDescription", $"Описание «{addText.TextDescription}» уже существует в БД");

                return View(addText);
            }

            #endregion

            #region TXT файл

            if (addText.TxtFileFormFile != null)
            {
                addText.TxtFileName = addText.TxtFileFormFile.FileName;
                addText.TxtFileSize = (int)addText.TxtFileFormFile.Length;
            }

            if (!addText.TxtFileName.EndsWith(".txt"))
            {
                ModelState.AddModelError("TxtFileFormFile", "Выберите файл формата «.txt»");

                return View(addText);
            }

            if (await textContext.Texts.Where(file => file.TxtFileName == addText.TxtFileName).AnyAsync())
            {
                ModelState.AddModelError("TxtFileFormFile", $"Файл с именем «{addText.TxtFileName}» уже существует в БД");

                return View(addText);
            }

            #endregion

            #region HTML файл

            if (addText.HtmlFileFormFile != null)
            {
                addText.HtmlFileName = addText.HtmlFileFormFile.FileName;
                addText.HtmlFileSize = (int)addText.HtmlFileFormFile.Length;
            }

            if (!addText.HtmlFileName.EndsWith(".html"))
            {
                ModelState.AddModelError("HtmlFileFormFile", "Выберите файл формата «.html»");

                return View(addText);
            }

            if (await textContext.Texts.Where(file => file.HtmlFileName == addText.HtmlFileName).AnyAsync())
            {
                ModelState.AddModelError("HtmlFileFormFile", $"Файл с именем «{addText.HtmlFileName}» уже существует в БД");

                return View(addText);
            }

            #endregion

            #region Добавить файл в БД

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

            using StreamReader clearText = new(System.IO.Directory.GetCurrentDirectory() + DataConfig.TextsFolderPath + editText.TxtFileName);

            using StreamReader htmlText = new(System.IO.Directory.GetCurrentDirectory() + DataConfig.TextsFolderPath + editText.HtmlFileName);

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
                var path = System.IO.Directory.GetCurrentDirectory() + DataConfig.TextsFolderPath + textItem.TxtFileName;

                // полная перезапись файла 
                using StreamWriter writer = new(path, false);

                await writer.WriteLineAsync(textItem.ClearText);
            }

            #endregion

            #region Текст с разметкой (html)

            if (!string.IsNullOrEmpty(textItem.HtmlText))
            {
                _ = textItem.HtmlText.Trim();

                // путь к файлу 
                var path = System.IO.Directory.GetCurrentDirectory() + DataConfig.TextsFolderPath + textItem.HtmlFileName;

                // полная перезапись файла 
                using StreamWriter writer = new(path, false);

                await writer.WriteLineAsync(textItem.HtmlText);
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
}