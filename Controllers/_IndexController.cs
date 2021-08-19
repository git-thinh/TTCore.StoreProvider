using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.Docs.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCore.StoreProvider.TagHelpers;

namespace TTCore.StoreProvider.Controllers
{
    public class _IndexController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return this.View2();
        }
    }
}
