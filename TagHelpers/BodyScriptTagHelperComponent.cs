using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TTCore.StoreProvider.TagHelpers
{
    public class BodyScriptTagHelperComponent : TagHelperComponent
    {
        private readonly string _markup = "";
        public override int Order { get; } = 2;
        public BodyScriptTagHelperComponent(string markup = "", int order = 1)
        {
            _markup = markup;
            Order = order;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(context.TagName, "body", StringComparison.OrdinalIgnoreCase))
            {
                if (_markup.Length > 0)
                {
                    var script = await File.ReadAllTextAsync("TagHelpers/Templates/AddressToolTipScript.html");
                    output.PostContent.AppendHtml(script + _markup);
                }
            }
        }
    }
}
