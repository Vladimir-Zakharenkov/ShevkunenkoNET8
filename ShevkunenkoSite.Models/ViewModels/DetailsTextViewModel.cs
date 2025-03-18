namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsTextViewModel : TextInfoModel
{
    #region Текст без разметки

    public string? ClearText { get; set; }

    #endregion

    #region Текст с разметкой

    public string? HtmlText { get; set; }

    #endregion

    #region Связанная книга (статья)

    [DataType(DataType.Text)]
    [Display(Name = "Связанная книга (статья) :")]
    public string? RefForBookOrArticle { get; set; }

    #endregion

    #region Сообщение о наличии ссылки в базе данных фильмов (при попытке удалить текст)

    public string? RefInMovies { get; set; }

    #endregion
}