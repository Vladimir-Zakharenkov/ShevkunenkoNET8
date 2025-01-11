namespace ShevkunenkoSite.Models.ViewModels;

public class TopicMovieViewModel
{
    public TopicMovieModel[] AllTopics { get; set; } = [];

    public PagingInfoViewModel PagingInfo { get; set; } = new();

    public string? TopicMovieSearchString { get; set; }
}