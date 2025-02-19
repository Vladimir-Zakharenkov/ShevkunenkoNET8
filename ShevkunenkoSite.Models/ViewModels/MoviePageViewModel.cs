// Ignore Spelling: Sergeyshef

namespace ShevkunenkoSite.Models.ViewModels;

public class MoviePageViewModel
{
    public MovieFileModel MovieFile { get; set; } = new();

    public MovieFileModel? FullMovie { get; set; }

    public bool SergeyshefRu {  get; set; } = false;

    public Uri VideoRef { get; set; } = null!;

    public string? ArticleAboutMovie1 { get; set; }
}