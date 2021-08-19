using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TTCore.StoreProvider.Helpers
{
    [HtmlTargetElement("a", Attributes = "href-key")]
    public class HrefKeyTagHelper : TagHelper
    {
        public string HrefKey { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (HrefKey == null)
                return;

            var href = ViewContext.ViewData[HrefKey] as string;

            //if (string.IsNullOrWhiteSpace(href))
            //    return;

            output.Attributes.SetAttribute("href", href);

            string text = href;
            if (string.IsNullOrEmpty(href)) text = HrefKey;

            output.Content.SetContent(text);
        }
    }
}