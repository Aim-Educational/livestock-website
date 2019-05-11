using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Website.TagHelpers
{
    /// <summary>
    /// For checkboxes. Pre-checks the checkbox if the given `ReproduceInput` contains the specified flag `ReproduceType`.
    /// </summary>
    [HtmlTargetElement("input", Attributes = "[type=checkbox]")]
    public class ReproduceFlagInputTagHelper : TagHelper
    {
        public ReproduceFlags ReproduceType { get; set; }
        public int ReproduceInput { get; set; } // This is from the Critter model, so it'll be an int.

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("value", (int)this.ReproduceType);

            if((this.ReproduceInput & (int)this.ReproduceType) > 0)
                output.Attributes.Add("checked", null);
        }
    }
}
