using Microsoft.AspNetCore.Mvc;

namespace TTCore.StoreProvider.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Article()
        {
            var area = ControllerContext.ActionDescriptor.RouteValues["area"];
            var controllerName = ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            var template = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Template;

            return Content($"area name:{area} controller:{controllerName}  action name: {actionName}  template:{template}");

        }

        public IActionResult Index()
        {
            var area = ControllerContext.ActionDescriptor.RouteValues["area"];
            var controllerName = ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            var template = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Template;

            return Content($"area name:{area} controller:{controllerName}  action name: {actionName}  template:{template}");

        }
    }
}