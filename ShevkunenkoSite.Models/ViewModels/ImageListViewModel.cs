namespace ShevkunenkoSite.Models.ViewModels;

public class ImageListViewModel
{
    #region Список картинок

    public IEnumerable<ImageFileModel> AllImages { get; set; } = [];

    #endregion

    #region Постраничная информация

    public PagingInfoViewModel PagingInfo { get; set; } = new();

    #endregion

    #region Строка поиска картинки

    public string? ImageSearchString { get; set; }

    #endregion

    #region Id (Guid) текущей картинки

    public Guid? CurrentImageId { get; set; }

    #endregion

    public bool? IconList { get; set; } = false;
}