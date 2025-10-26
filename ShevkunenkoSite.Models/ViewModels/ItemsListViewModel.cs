namespace ShevkunenkoSite.Models.ViewModels;

public class ItemsListViewModel : PagingInfoViewModel
{
    public AudioBookModel[]? AllAudioBooks { get; set; }

    public AudioInfoModel[]? AllAudioFiles { get; set; }
}