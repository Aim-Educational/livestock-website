using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Website.TagHelpers
{
    /// <summary>
    /// For checkboxes. Pre-checks the checkbox if the given `FlagInput` contains the specified flag `FlagToUser`.
    /// </summary>
    [HtmlTargetElement("input", Attributes = "[reproduce-type]")]
    public class CritterFlagInputTagHelper : TagHelper
    {
        public CritterFlags FlagToUse { get; set; }
        public int? FlagInput { get; set; } // This is from the Critter model, so it'll be an int.

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("value", (int)this.FlagToUse);

            if(this.FlagInput != null && (this.FlagInput & (int)this.FlagToUse) > 0)
                output.Attributes.Add("checked", null);
        }
    }
}
