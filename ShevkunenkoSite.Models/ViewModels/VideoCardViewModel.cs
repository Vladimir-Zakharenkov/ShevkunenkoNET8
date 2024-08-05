namespace ShevkunenkoSite.Models.ViewModels;

public class VideoCardViewModel
{
    public MovieFileModel MovieInfo { get; set; } = null!;

    public string PathForSeries { get; set; } = string.Empty; // путь к странице для многосерийного фильма

    public string ImageForVideo { get; set; } = string.Empty;

    public Guid? ImageForVideoGuid { get; set; }

    public bool? IsImageIcon { get; set; } // true -> иконка картинки (width 300 px), false -> картинка (width 720 px), null -> картинка HD

    // "hd", "icon300", "icon200", "icon100", "image", "webhd", "webicon300", "webicon200", "webicon100", "webimage" 
    public string IconType { get; set; } = string.Empty;
}