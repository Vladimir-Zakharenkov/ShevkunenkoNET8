namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsTextViewModel : TextInfoModel
{
    public string? ClearText { get; set; }

    public string? HtmlText { get; set; }

    // сообщение о наличии ссылки в базе данных фильмов (при попытке удалить текст)
    public string? RefInMovies { get; set; }
}