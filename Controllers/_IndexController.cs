using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using TTCore.StoreProvider.TagHelpers;
using System.Linq;

namespace TTCore.StoreProvider.Controllers
{
    public class _IndexController : Controller
    {
        private readonly ITagHelperComponentManager _tagHelperComponentManager;

        public _IndexController(ITagHelperComponentManager tagHelperComponentManager)
        {
            _tagHelperComponentManager = tagHelperComponentManager;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            string markup = "<h1>_tagHelperComponentManager UiFooterTagHelper</h1><em class='text-warning'> _tagHelperComponentManager Office closed today!</em>";
            var footer = new BodyScriptTagHelperComponent(markup, 1);
            var cs = _tagHelperComponentManager.Components.ToArray();
            var index = -1;
            for (int i = 0; i < cs.Length; i++) {
                string name = cs[i].GetType().Name;
                if (name == "BodyScriptTagHelperComponent") {
                    index = i;
                    break;
                }
            }
            
            if (index != -1)
            {
                _tagHelperComponentManager.Components.Remove(cs[index]);
                _tagHelperComponentManager.Components.Add(footer);
            }

            return this.View2();
        }
    }
}
