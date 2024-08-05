namespace ShevkunenkoSite.Areas.Videos.Controllers;

[Area("Videos")]
[Route("[area]")]
public class BBC_Top_100_US_MoviesController : Controller
{
    [Route("bbc-top100-us-movies/{id:int:range(1, 100)?}")]
    [Route("[controller]/[action]/{id:int:range(1, 100)?}")]
    public IActionResult Index(int id)
    {
        return View();
    }
}
