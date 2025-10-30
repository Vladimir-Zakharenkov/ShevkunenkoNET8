namespace ShevkunenkoSite.Models.ViewModels;

public class ItemsListViewModel : PagingInfoViewModel
{
    #region Аудиокниги

    public AudioBookModel[]? AllAudioBooks { get; set; }

    #endregion

    #region Аудиофайлы

    public AudioInfoModel[]? AllAudioFiles { get; set; }

    #endregion
}