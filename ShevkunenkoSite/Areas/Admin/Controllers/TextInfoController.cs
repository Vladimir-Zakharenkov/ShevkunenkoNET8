using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Configuration;
using ShevkunenkoSite.Models.DataModels;

namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class TextInfoController(ITextInfoRepository textContext) : Controller
{
    #region Список текстов

    public int textsPerPage = 16;

    public async Task<IActionResult> Index(string? textSearchString,
                                                                int pageNumber = 1)
    {
        var allTexts = from m in textContext.Texts
            .Where
            (
                s => s.ClearText.Contains((textSearchString ?? string.Empty).Trim())
            )
                       select m;

        return View(new TextInfoViewModel
        {
            AllTexts = await allTexts
                     .Skip((pageNumber - 1) * textsPerPage)
                     .Take(textsPerPage)
                     .ToArrayAsync(),

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
        if (textId.HasValue)
        {
            TextInfoModel textItem;

            #region Инициализация textItem

            if (await textContext.Texts.Where(p => p.TextInfoModelId == textId).AnyAsync())
            {
                textItem = await textContext.Texts
                    .AsNoTracking()
                    .FirstAsync(p => p.TextInfoModelId == textId);

                return View(textItem);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            #endregion
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
        TextInfoModel newText = new();

        return View(newText);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddText(TextInfoModel addText)
    {
        if (ModelState.IsValid)
        {
            #region Описание текста

            addText.TextDescription = addText.TextDescription.Trim();

            #endregion

            #region Текст без разметки

            addText.ClearText = addText.ClearText.Trim();

            #endregion

            #region Текст с разметкой (html)

            addText.HtmlText = addText.HtmlText.Trim();

            #endregion

            await textContext.AddNewTextAsync(addText);

            #region Открытие списка текстов

            return RedirectToAction("Index", new { Area = "Admin" });

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

            return View(editText);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditText(TextInfoModel textItem)
    {
        if (ModelState.IsValid)
        {
            TextInfoModel textUpdate = await textContext.Texts.FirstAsync(txt =>txt.TextInfoModelId == textItem.TextInfoModelId);

            #region Описание текста

            textUpdate.TextDescription = textItem.TextDescription.Trim();

            #endregion

            #region Текст без разметки (txt)

            textUpdate.ClearText = textItem.ClearText;

            #endregion

            #region Текст с разметкой (html)

            textUpdate.HtmlText = textItem.HtmlText;

            #endregion

            await textContext.SaveChangesInTextAsync();

            return RedirectToAction(nameof(DetailsText), new { textId = textUpdate.TextInfoModelId });
        }
        else
        {
            return View(textItem);
        }
    }
    #endregion
}