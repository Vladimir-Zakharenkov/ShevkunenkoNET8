namespace ShevkunenkoSite.Models.ViewModels;

public class ViewImageViewModel
{
    // Экземпляр класса картинки
    public ImageFileModel ImageItem { get; set; } = new();

    // CSS для картинки
    public string? CssClass { get; set; }

    // Параметры файла картинки "hd", "image", "icon300", "icon200", "icon100", "webhd", "webimage", "webicon300", "webicon200", "webicon100"
    public string? IconType { get; set; }
}