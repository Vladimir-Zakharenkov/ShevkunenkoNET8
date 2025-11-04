namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsTextViewModel : TextInfoModel
{
    #region Каталоги в каталоге /wwwroot/texts/

    public List<SelectListItem> TextDirectories { get; set; } = new([.. Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\wwwroot\\texts", "*", SearchOption.AllDirectories)
        .Select(a => new SelectListItem
        {
            Value = a[(Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\wwwroot\\texts")[0].IndexOf("texts") + 6)..].Replace('\\', '/'),
            Text = a[(Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\wwwroot\\texts")[0].IndexOf("texts") +6)..].Replace('\\', '/')
        })]);

    #endregion

    #region Связанная книга (статья)

    [DataType(DataType.Text)]
    [Display(Name = "Связанная книга (статья) :")]
    public string? RefForBookOrArticle { get; set; }

    #endregion

    #region Задать новый каталог

    [Required(AllowEmptyStrings = true)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [DataType(DataType.Text)]
    [Display(Name = "Новый каталог :")]
    public string NewFolderForText { get; set; } = string.Empty;

    #endregion

    #region Загрузить TXT файл

    [DataType(DataType.Upload)]
    [Display(Name = "Файл (txt) :")]
    public IFormFile? TxtFileFormFile { get; set; }

    #endregion

    #region Загрузить HTML файл

    [DataType(DataType.Upload)]
    [Display(Name = "Файл (html) :")]
    public IFormFile? HtmlFileFormFile { get; set; }

    #endregion

    #region Сообщение о наличии ссылки в базе данных (при попытке удалить текст)

    public string? RefInMovies { get; set; }

    #endregion
}