namespace ShevkunenkoSite.Models.ViewModels;

public class DetailsImageIconViewModel
{
    public ImageFileModel ImageItem { get; set; } = new();

    public string? IconType { get; set; } = null;

   public bool FileExists { get; set; } = true;

    public string FileName { get; set;} = string.Empty;

    public string WebImageButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";

    public string ImageButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";

    public string WebhdButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";

    public string HdButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";

    public string Icon300ButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";

    public string WebIcon300ButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";

    public string Icon200ButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";

    public string WebIcon200ButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";

    public string Icon100ButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";

    public string WebIcon100ButtonInfostyle { get; set; } = "btn btn-outline-primary py-1 px-1 shadw";
}