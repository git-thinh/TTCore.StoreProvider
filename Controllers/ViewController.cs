using Microsoft.AspNetCore.Mvc;

namespace TTCore.StoreProvider.Controllers
{
    public class ViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult About()
        {
            var url = Url.Action("AddUser", "Users", new { Area = "Zebra" });
            return Content($"URL: {url}");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
    }
}
