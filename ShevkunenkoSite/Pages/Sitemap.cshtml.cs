using System.Text;

namespace ShevkunenkoSite.Pages;

public class SitemapModel : PageModel
{
    public readonly IPageInfoRepository _pageinfoContext;

    public SitemapModel(IPageInfoRepository pageinfoContext) => _pageinfoContext = pageinfoContext;

    public async Task<IActionResult> OnGetAsync()
    {
        var pages = await _pageinfoContext.PagesInfo.ToListAsync();

        StringBuilder sb = new();

        sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.google.com/schemas/sitemap/0.84 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\">");

        foreach (var page in pages)
        {
            if (!page.PageLoc.ToLower().Contains("dbcrud") || !page.PageLoc.ToLower().Contains("pageinfo"))
            {
                string pageLoc = (this.Request.Scheme + "://" + this.Request.Host + page.PageFullPathWithData).Replace("&", "&amp;");
                string pageLastmode = page.PageLastmod.ToString("yyyy-MM-ddTHH:mm:sszzz");

                sb.Append($"<url><loc>{pageLoc}</loc><lastmod>{pageLastmode}</lastmod><changefreq>{page.Changefreq}</changefreq><priority>{page.Priority}</priority></url>");

                if (!string.IsNullOrEmpty(page.PagePathNickName))
                {
                    string pagePathNickName = (this.Request.Scheme + "://" + this.Request.Host + page.PagePathNickName).Replace("&", "&amp;");

                    sb.Append($"<url><loc>{pagePathNickName}</loc><lastmod>{pageLastmode}</lastmod><changefreq>{page.Changefreq}</changefreq><priority>{page.Priority}</priority></url>");
                }
            }
        }

        sb.Append("</urlset>");

        return new ContentResult
        {
            ContentType = "application/xml",
            Content = sb.ToString(),
            StatusCode = 200
        };
    }
}