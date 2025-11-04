using Microsoft.AspNetCore.Hosting;
using ShevkunenkoSite.Models;

namespace ShevkunenkoSite.Services.Extensions;

public static class TextInfoModelExtension
{
    public static IEnumerable<TextInfoModel> TextSearch(this IEnumerable<TextInfoModel> textInfoModel, string? searchString, IWebHostEnvironment hostEnvironment)
    {
        var rootPath = hostEnvironment.WebRootPath;

        foreach (var foundText in textInfoModel)
        {
            using StreamReader clearText = new(rootPath + DataConfig.TextsFolderPath + foundText.FolderForText + foundText.TxtFileName);

            var clearString = clearText.ReadToEnd();

            if (clearString.Contains((searchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase)
                || foundText.TextDescription.Contains((searchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
            {
                yield return foundText;
            }

            clearText.Close();
        }
    }
}