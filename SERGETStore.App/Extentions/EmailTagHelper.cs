﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SERGETStore.App.Extentions
{
    public class EmailTagHelper : TagHelper
    {
        public string EmailDomain { get; set; } = "SERGET.COM.BR";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var content = await     output.GetChildContentAsync();
            var target = content.GetContent() + "@" + EmailDomain;
            output.Attributes.SetAttribute("href", "mailto:" + target);
            output.Content.SetContent(target);

        }

    }
}
