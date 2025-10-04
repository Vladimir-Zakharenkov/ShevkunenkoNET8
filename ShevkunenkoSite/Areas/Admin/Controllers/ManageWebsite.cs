namespace ShevkunenkoSite.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ManageWebsite : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}