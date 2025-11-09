namespace ShevkunenkoSite.Models.ViewModels;

public class FramesAroundMainContentModel
{
    [Display(Name = "Кадры слева :")]
    public ImageFileModel[]? FramesOnTheLeft { get; set; }

    [Display(Name = "Кадры справа :")]
    public ImageFileModel[]? FramesOnTheRight { get; set; }
}