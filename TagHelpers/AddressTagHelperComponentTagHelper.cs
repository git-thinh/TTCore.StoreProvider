using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

namespace TTCore.StoreProvider.TagHelpers
{
    [HtmlTargetElement("address")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class AddressTagHelperComponentTagHelper : TagHelperComponentTagHelper
    {
        readonly ITagHelperComponentManager manager;
        public AddressTagHelperComponentTagHelper(
            ITagHelperComponentManager componentManager,
            ILoggerFactory loggerFactory) : base(componentManager, loggerFactory)
        {
            manager = componentManager;
        }

        private readonly string _printableButton =
            "<button type='button' class='btn btn-info' onclick=\"window.open(" +
            "'https://binged.it/2AXRRYw')\">" +
            "PRINT !!! <BR><span class='glyphicon glyphicon-road' aria-hidden='true'></span>" +
            "</button>";
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.Equals(context.TagName, "address", StringComparison.OrdinalIgnoreCase) &&
                output.Attributes.ContainsName("printable"))
            {
                string h3 = "<br><h3>99999999</h3><em class='text-warning'>Office closed today!</em>";
                TagHelperContent childContent = await output.GetChildContentAsync();
                string content = childContent.GetContent();
                output.Content.SetHtmlContent($"<div><h1>{h3}</h1>{content}</div>{_printableButton}");
            }
        }
    }
}
