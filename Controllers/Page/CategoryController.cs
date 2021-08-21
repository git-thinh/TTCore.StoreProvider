using Microsoft.AspNetCore.Mvc;

namespace TTCore.StoreProvider.Areas.Controllers
{
    [Area("Page")]
    [MvcAreaRouteBase]
    public class CategoryController : Controller
    {
        [HttpGet("cat-1")]
        public IActionResult cat1()
        {
            return this.View2();
        }

        [HttpGet("cat-2")]
        public IActionResult cat2()
        {
            var area = ControllerContext.ActionDescriptor.RouteValues["area"];
            var controllerName = ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            var template = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Template;

            return Content($"area name:{area} controller:{controllerName}  action name: {actionName}  template:{template}");
        }
    }
}

