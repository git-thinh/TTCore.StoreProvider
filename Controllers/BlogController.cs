using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;

namespace TTCore.StoreProvider.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Article()
        {
            return ControllerContext.MyDisplayRouteInfo();
        }

        public IActionResult Index()
        {
            return ControllerContext.MyDisplayRouteInfo();
        }
    }
}