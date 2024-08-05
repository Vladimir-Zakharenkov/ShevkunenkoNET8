namespace ShevkunenkoSite.Models.ViewModels;

public class FramesViewModel
{
    public MovieFileModel MovieFile { get; set; } = new();

    public ImageFileModel ImageFile { get; set; } = new();

    public ImageFileModel[]? ImageArray { get; set; }
}