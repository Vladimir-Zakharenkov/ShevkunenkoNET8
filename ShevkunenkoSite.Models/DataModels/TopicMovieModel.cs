namespace ShevkunenkoSite.Models.DataModels;

public class TopicMovieModel
{
    [Display(Name = "TopicMovieId:")]
    [Column("TopicMovieId")]
    public Guid TopicMovieModelId { get; set; }

    [Required(ErrorMessage = "Добавьте описание")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Описание :")]
    public string TopicDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Добавьте заголовок страницы")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Заголовок страницы :")]
    public string TopicHeadPage { get; set; } = string.Empty;

    // true -> картинку для видео, false -> постер для видео, null -> картинка для страницы видео
    [Display(Name = "Тип картинки для ссылки :")]
    public bool? ImageForRef { get; set; }

    // "hd", "image", "icon300", "icon200", "icon100", "webhd", "webimage", "webicon300", "webicon200", "webicon100" 
    [Display(Name = "Формат картинки для ссылки :")]
    [DataType(DataType.Text)]
    public string IconTypeForRef { get; set; } = string.Empty;

    // true - выбираем главную страницу многосерийного фильма
    [Display(Name = "Ссылка на страницу серий :")]
    public bool GeneralPageForMovieEpisodes { get; set; } = true;

    [Display(Name = "Количество ссылок на странице :")]
    [Range(1, 24, ErrorMessage = "{0} не должно быть меньше {1} и больше {2}")]
    public int NumberOfLinksPerPage { get; set; } = 1;
}