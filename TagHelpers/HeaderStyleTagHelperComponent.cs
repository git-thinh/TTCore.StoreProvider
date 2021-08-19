using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TTCore.StoreProvider.TagHelpers
{
    public class HeaderStyleTagHelperComponent : TagHelperComponent
    {
        private readonly string _style = @"<link rel=""stylesheet"" href=""HeaderStyleTagHelperComponent.css"" />";

        public override int Order => 1;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
            {
                output.PostContent.AppendHtml(_style);
            }

            return Task.CompletedTask;
        }
    }
}
