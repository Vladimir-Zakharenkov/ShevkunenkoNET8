using Microsoft.AspNetCore.Hosting;
using ShevkunenkoSite.Models;

namespace ShevkunenkoSite.Services.Extensions;

public static class TextInfoModelExtension
{
    public static IEnumerable<TextInfoModel> TextSearch(this IEnumerable<TextInfoModel> textInfoModel, string? textSearchString, IWebHostEnvironment hostEnvironment)
    {
        var rootPath = hostEnvironment.WebRootPath;

        foreach (var foundText in textInfoModel)
        {
            using StreamReader clearText = new(rootPath + DataConfig.TextsFolderPath + foundText.TxtFileName);

            var clearString = clearText.ReadToEnd();

            if (clearString.Contains((textSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                || foundText.TextDescription.Contains((textSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
            {
                yield return foundText;
            }

            clearText.Close();
        }
    }
}