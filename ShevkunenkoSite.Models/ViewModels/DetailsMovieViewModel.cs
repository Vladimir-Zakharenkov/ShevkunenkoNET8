namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsMovieViewModel
{
    public MovieFileModel MovieItem { get; set; } = null!;

    public PageInfoModel? PageForSeries { get; set; }

    public MovieFileModel? FullMovie { get; set; }

    public ImageFileModel? PosterForMovie { get; set; }

    public string[] SearchFilters { get; set; } = Array.Empty<string>();

    public string[] TopicFilters { get; set; } = Array.Empty<string>();
}