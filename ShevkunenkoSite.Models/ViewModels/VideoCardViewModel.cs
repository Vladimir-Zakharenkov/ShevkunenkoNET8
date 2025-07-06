namespace ShevkunenkoSite.Models.ViewModels;

public class VideoCardViewModel
{
    #region Экземпляр видео

    public MovieFileModel MovieInfo { get; set; } = new();

    #endregion

    #region Путь к странице серий фильма (для многосерийного фильма)

    public string PathForSeries { get; set; } = string.Empty;

    #endregion

    #region Картинка для видео (название файла в базе данных)

    public string ImageForVideo { get; set; } = string.Empty;

    #endregion

    #region Картинка для видео (Guid в базе данных)

    public Guid? ImageForVideoGuid { get; set; }

    #endregion

    #region Размеры картинки (true -> иконка картинки (width 300 px), false -> картинка (width 720 px), null -> картинка HD)

    public bool? IsImageIcon { get; set; }

    #endregion

    #region Тип картинки ("hd", "icon300", "icon200", "icon100", "image", "webhd", "webicon300", "webicon200", "webicon100", "webimage")

    public string IconType { get; set; } = string.Empty;

    #endregion

    #region Ссылка на страницу информации о фильме (true) или ссылка на страницу фильма (false)

    public bool? LinkToInfoAboutMovie { get; set; }

    #endregion
}