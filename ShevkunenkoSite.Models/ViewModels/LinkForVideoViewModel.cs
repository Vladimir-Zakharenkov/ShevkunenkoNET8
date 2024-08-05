namespace ShevkunenkoSite.Models.ViewModels;

public class LinkForVideoViewModel
{
    public MovieFileModel MovieInfo { get; set; } = new();

    public string? PathForSeries { get; set; } // путь к странице для многосерийного фильма

    public string ImageForVideo { get; set; } = string.Empty; // название файла картинки

    public string IconType { get; set; } = string.Empty; // "hd", "icon300", "icon200", "icon100" или картинка (width 720 px) 
}
