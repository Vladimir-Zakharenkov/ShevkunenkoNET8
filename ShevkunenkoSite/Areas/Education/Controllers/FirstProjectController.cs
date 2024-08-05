using Microsoft.AspNetCore.Authorization;

namespace ShevkunenkoSite.Areas.Education.Controllers
{
    [Area("Education")]
    [Authorize]

    public class FirstProjectController : Controller
    {
        #region ReturnString

        [Route("[area]")]
        [Route("[area]/[controller]")]
        [Route("[area]/[controller]/[action]")]
        public string Index()
        {
            return "Hello World";
        }

        #endregion
    }
}
