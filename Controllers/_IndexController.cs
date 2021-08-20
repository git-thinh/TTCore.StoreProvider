using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using TTCore.StoreProvider.TagHelpers;
using System.Linq;
using TTCore.StoreProvider.Models;
using Microsoft.Extensions.Options;
using TTCore.StoreProvider.ServiceBackground;
using System;

namespace TTCore.StoreProvider.Controllers
{
    public class _IndexController : Controller
    {
        readonly UserLogin[] _users;
        readonly Article[] _articles;
        readonly ITagHelperComponentManager _tagHelperManager;
        public _IndexController(ITagHelperComponentManager tagHelper,
            IOptions<CollectionItems<UserLogin>> userOptions,
            IOptions<CollectionItems<Article>> articleOptions,
            RedisService redis)
        {
            _tagHelperManager = tagHelper;
            _users = userOptions.Value.Items;
            _articles = articleOptions.Value.Items;
            //var _redis = redis.GetDB(1);
            //var keys = redis.GetServer().Keys(1).ToArray();
            //var _redis.StringSet("k1", DateTime.Now.ToString());
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            string markup = "<h1>_tagHelperComponentManager UiFooterTagHelper</h1><em class='text-warning'> _tagHelperComponentManager Office closed today!</em>";
            var footer = new BodyScriptTagHelperComponent(markup, 1);
            var cs = _tagHelperManager.Components.ToArray();
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
                _tagHelperManager.Components.Remove(cs[index]);
                _tagHelperManager.Components.Add(footer);
            }

            return this.View2();
        }
    }
}
