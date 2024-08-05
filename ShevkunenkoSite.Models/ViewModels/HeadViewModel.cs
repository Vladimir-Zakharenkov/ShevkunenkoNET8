namespace ShevkunenkoSite.Models.ViewModels;

public class HeadViewModel
{
    public PageInfoModel PageInfo { get; set; } = null!;

    public List<IconFileModel> IconList { get; set; } = new List<IconFileModel>();

    public IconFileModel IconItem { get; set; } = null!;
}
