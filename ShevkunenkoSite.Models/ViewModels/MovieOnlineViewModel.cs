// Ignore Spelling: Sergeyshef Youtube Mailru

namespace ShevkunenkoSite.Models.ViewModels;

public class MovieOnlineViewModel
{
    public PageInfoModel PageInfo { get; set; } = null!;

    public Uri VideoRef { get; set; } = null!;

    public string? YoutubeImageBorder { get; set; }

    public string? VkImageBorder { get; set; }

    public string? OkImageBorder { get; set; }

    public string? MailruImageBorder { get; set; }

    public string? SergeyshefBorder { get; set; }

    public bool SergeyshefRu { get; set; }

    public string SitePath { get; set; } = string.Empty;
}