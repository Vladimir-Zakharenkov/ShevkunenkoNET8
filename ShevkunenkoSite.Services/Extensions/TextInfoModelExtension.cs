using ShevkunenkoSite.Models;

namespace ShevkunenkoSite.Services.Extensions;

public static class TextInfoModelExtension
{
    public static IEnumerable<TextInfoModel> TextSearch(this IEnumerable<TextInfoModel> textInfoModel, string? textSearchString)
    {
        foreach (var foundText in textInfoModel)
        {
            using StreamReader clearText = new(System.IO.Directory.GetCurrentDirectory() + DataConfig.TextsFolderPath + foundText.TxtFileName);

            var clearString = clearText.ReadToEnd();

            if (clearString.Contains((textSearchString ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
            {
                yield return foundText;
            }

            clearText.Close();
        }
    }
}