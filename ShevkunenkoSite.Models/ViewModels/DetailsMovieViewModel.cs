namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsMovieViewModel
{
    // видео
    public MovieFileModel MovieItem { get; set; } = null!;

    // список ссылок на группы видео по фильтру SearchFilter1
    // отображение иконок
    public List<VideoLinksViewModel> LinksToVideosByFilter1 { get; set; } = [];

    // список списков видео по фильтру SearchFilter1
    // данные для таблицы
    public List<List<MovieFileModel>> ListsMoviesFileModel1 { get; set; } = [];

    // список ссылок на группы видео по фильтру SearchFilter2
    // отображение иконок
    public List<VideoLinksViewModel> LinksToVideosByFilter2 { get; set; } = [];

    // список списков видео по фильтру SearchFilter2
    // данные для таблицы
    public List<List<MovieFileModel>> ListsMoviesFileModel2 { get; set; } = [];

    // список ссылок на группы видео по фильтру SearchFilter3
    // отображение иконок
    public List<VideoLinksViewModel> LinksToVideosByFilter3 { get; set; } = [];

    // список списков видео по фильтру SearchFilter3
    // данные для таблицы
    public List<List<MovieFileModel>> ListsMoviesFileModel3 { get; set; } = [];

    public PageInfoModel? PageForSeries { get; set; }

    public MovieFileModel? FullMovie { get; set; }

    public ImageFileModel? PosterForMovie { get; set; }

    public string[] SearchFilters { get; set; } = [];

    public string[] TopicFilters { get; set; } = [];

    // статья о фильме (1)
    public string? ArticleAboutMovie1 { get; set; }
}