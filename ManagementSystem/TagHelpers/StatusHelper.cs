using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ManagementSystem.TagHelpers
{
    [HtmlTargetElement("status")]
    public class StatusHelper : TagHelper
    {
        public bool HasStatus { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.AddClass("status", HtmlEncoder.Default);

            var builder = new StringBuilder();

            var icons = new List<string>
                {
                    "/img/Online.png",
                    "/img/Offline.png",
                };

            if (HasStatus)
                builder.Append($"<img src='{icons[0]}' width='30px';height='30px';>");

            else
                builder.Append($"<img src='{icons[1]}' width='30px';height='30px';>");

            output.Content.SetHtmlContent(builder.ToString());
        }
    }
}
