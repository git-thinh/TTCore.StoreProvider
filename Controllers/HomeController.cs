﻿using Microsoft.AspNetCore.Mvc;

namespace TTCore.StoreProvider.Controllers
{
    [Area("Public")]
    [MvcAreaRouteBase]
    public class HomeController : Controller
    {
        [HttpGet("view-1")]
        public IActionResult view1(int id)
        {
            return this.View();
        }
    }

    //[Route("Home")]
    //public class HomeController : Controller
    //{
    //    [Route("")]
    //    [Route("Index")]
    //    [Route("/")]
    //    public IActionResult Index()
    //    {
    //        return ControllerContext.MyDisplayRouteInfo();
    //    }

    //    [Route("About")]
    //    public IActionResult About()
    //    {
    //        return ControllerContext.MyDisplayRouteInfo();
    //    }
    //}

    //public class HomeController : Controller
    //{
    //    [Route("")]
    //    [Route("Home")]
    //    [Route("Home/Index")]
    //    [Route("Home/Index/{id?}")]
    //    public IActionResult Index(int? id)
    //    {
    //        return ControllerContext.MyDisplayRouteInfo(id);
    //    }

    //    [Route("Home/About")]
    //    [Route("Home/About/{id?}")]
    //    public IActionResult About(int? id)
    //    {
    //        return ControllerContext.MyDisplayRouteInfo(id);
    //    }
    //}

    //public class HomeController : Controller
    //{
    //    [Route("")]
    //    [Route("Home")]
    //    [Route("[controller]/[action]")]
    //    public IActionResult Index()
    //    {
    //        return ControllerContext.MyDisplayRouteInfo();
    //    }

    //    [Route("[controller]/[action]")]
    //    public IActionResult About()
    //    {
    //        return ControllerContext.MyDisplayRouteInfo();
    //    }
    //}

    //[Route("[controller]/[action]")]
    //public class HomeController : Controller
    //{
    //    [Route("~/")]
    //    [Route("/Home")]
    //    [Route("~/Home/Index")]
    //    public IActionResult Index()
    //    {
    //        return ControllerContext.MyDisplayRouteInfo();
    //    }

    //    public IActionResult About()
    //    {
    //        return ControllerContext.MyDisplayRouteInfo();
    //    }
    //}
}

