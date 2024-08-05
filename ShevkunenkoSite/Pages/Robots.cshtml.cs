using System.Text;

namespace ShevkunenkoSite.Pages;

public class RobotsModel : PageModel
{
    public List<string> disallowFolders = new()
    {
        "/Admin/DBCRUD",
        "/browserconfigs",
        "/css",
        "/js",
        "/lib",
        "/manifests",
        "/sass",
        "/images/admin",
        "/images/backgrounds",
        "/images/browserconfig",
        "/images/manifest",
        "/images/pageicons",
        "/video"
    };

    public IActionResult OnGet()
    {
        StringBuilder sb = new();

        sb.AppendLine("User-agent: *");

        foreach (var folder in disallowFolders)
        {
            sb.AppendLine($"Disallow: {folder}");
        }

        sb.Append("Sitemap: ")
        .Append(this.Request.Scheme)
        .Append("://")
        .Append(this.Request.Host)
        .AppendLine("/sitemap.xml");

        return new ContentResult
        {
            ContentType = "text/plain",
            Content = sb.ToString(),
            StatusCode = 200
        };
    }
}
