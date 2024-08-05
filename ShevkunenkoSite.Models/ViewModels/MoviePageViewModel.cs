// Ignore Spelling: Sergeyshef

namespace ShevkunenkoSite.Models.ViewModels;

public class MoviePageViewModel
{
    public MovieFileModel MovieFile { get; set; } = new();

    public MovieFileModel? FullMovie { get; set; }

    public bool SergeyshefRu {  get; set; } = false;

    public Uri VideoRef { get; set; } = null!;

    public string? YoutubeImageBorder { get; set; }

    public string? VkImageBorder { get; set; }

    public string? OkImageBorder { get; set; }

    public string? MailruImageBorder { get; set; }

    public string? SergeyshefBorder { get; set; }
}