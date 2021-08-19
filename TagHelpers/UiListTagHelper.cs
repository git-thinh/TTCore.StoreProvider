using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCore.StoreProvider.TagHelpers
{
    // <ui-list list-title="Danh sách sản phẩm" list-items="@productlist"></ui-list>
    [HtmlTargetElement("ui-list", Attributes = "list-items")]
    public class UiListTagHelper : TagHelper
    {
        // Thuộc tính sẽ là list-title
        public string ListTitle { get; set; }

        // Thuộc tính sẽ là list-items
        public List<string> ListItems { set; get; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //if (string.Equals(context.TagName, "ui-list", StringComparison.OrdinalIgnoreCase))
            //{
                output.TagName = "ul";    // ul sẽ thay cho myul
                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.SetAttribute("class", "list-group");
                output.PreElement.AppendHtml($"<h2>{ListTitle}</h2>");

                StringBuilder content = new StringBuilder();
                foreach (var item in ListItems)
                {
                    content.Append($@"<li class=""list-group-item"">{item}</li>");
                }
                output.Content.SetHtmlContent(content.ToString());
                //output.PostContent.AppendHtml(script);
            //}
        }

    }
}
