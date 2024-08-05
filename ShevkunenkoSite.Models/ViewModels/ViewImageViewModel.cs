namespace ShevkunenkoSite.Models.ViewModels;

public class ViewImageViewModel
{
    public ImageFileModel ImageItem { get; set; } = new();

    public string? CssClass { get; set; }

    // "hd", "image", "icon300", "icon200", "icon100", "webhd", "webimage", "webicon300", "webicon200", "webicon100"
    public string? IconType { get; set; }
}