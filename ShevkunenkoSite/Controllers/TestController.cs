using Microsoft.Docs.Samples;

namespace ShevkunenkoSite.Controllers;

// Тестовый контроллер
public class TestController : Controller
{
    public IActionResult Test(int id)
    {
        return ControllerContext.MyDisplayRouteInfo(id);
    }
}
