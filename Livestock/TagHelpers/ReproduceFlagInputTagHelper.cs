using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Website.TagHelpers
{
    /// <summary>
    /// For checkboxes. Sets the checkbox's value to the given `ReproduceType`, and ensures the checkbox is pre-checked
    /// if the `ReproduceInput` has the correct flag set for the given type.
    /// </summary>
    [HtmlTargetElement("input")]
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
