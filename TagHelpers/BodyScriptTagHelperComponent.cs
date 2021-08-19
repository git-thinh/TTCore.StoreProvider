using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TTCore.StoreProvider.TagHelpers
{
    public class BodyScriptTagHelperComponent : TagHelperComponent
    {
        public override int Order => 2;
        
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(context.TagName, "body", StringComparison.OrdinalIgnoreCase))
            {
                var script = await File.ReadAllTextAsync("TagHelpers/Templates/AddressToolTipScript.html");
                output.PostContent.AppendHtml(script);
            }
        }
    }
}
