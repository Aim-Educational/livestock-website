using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Website.TagHelpers
{
    public enum MultiSelectType
    {
        Critter,
        User
    }

    [HtmlTargetElement("aim-multi-select")]
    public class FilteredMultiSelectTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public MultiSelectType Type { get; set; }

        public IDictionary<int, string> Descriptions { get; set; }

        public string Label { get; set; }

        public string FormId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(this.For.Model != null && this.Descriptions == null)
                throw new InvalidOperationException($"Please pass the 'Descriptions' attribute for non-null models.");

            if (this.For.Model as IEnumerable<int> == null && this.For.Model != null)
                throw new InvalidOperationException($"Attribute asp-for must be of type `IEnumerable<int>`, not type `{this.For.ModelExplorer.ModelType}`");

            var options         = (this.For.Model as IEnumerable<int>)?.Select(i => new { value = i, inner = this.Descriptions[i] });
            var idName          = this.For.Name.Replace('.', '_');
            var filterId        = $"filter{idName}";
            var addId           = $"selectAdd{idName}";
            var selectedId      = $"selectSelected{idName}";
            var leftToRightId   = $"addToSelected{idName}";
            var rightToLeftId   = $"selectedToAdd{idName}";
            var builder         = new StringBuilder(); 

            if(options?.Count() == 0)
                options = null;
            builder.Append(
                // Filter box
                $"<div class='form-group'>\n" +
                $"  <label class='control-label'>{this.Label ?? this.For.Name}</label>\n" +
                $"  <input class='form-control' id='{filterId}' type='text' placeholder='Filter'/>\n" +
                $"</div>\n" +
                $"<div class='form-group row'>\n" +
                // Add box
                $"  <label class='col-sm-2 col-md-1 col-form-label'>Add:</label>" +
                $"  <div class='col-sm-10 col-md-11'>" +
                $"      <select multiple class='form-control resize-vert' id='{addId}'>\n" +
                $"      </select>\n" +
                $"  </div>" +
                // Selected box
                $"  <label class='col-sm-2 col-md-1 col-form-label'>Selected:</label>" +
                $"  <div class='col-sm-10 col-md-11'>" +
                $"      <select multiple class='form-control resize-vert' id='{selectedId}' name='{this.For.Name}'>\n" +
                $"          {options?.Select(o => $"<option value='{o.value}'>{o.inner}</option>").Aggregate((o1, o2) => $"{o1}\n{o2}") ?? ""}" +
                $"      </select>\n" +
                $"      <span class='text-danger' asp-validation-for='{this.For.Name}'></span>\n" +
                $"  </div>" +
                // Left to right, and right to left buttons.
                $"  <div class='col-md-4'>" +
                $"      <button class='btn btn-primary' id='{leftToRightId}' type='button'>&gt;&gt;&gt;</button>" +
                $"      <button class='btn btn-primary' id='{rightToLeftId}' type='button'>&lt;&lt;&lt;</button>" +
                $"  </div>" +
                $"</div>\n"
            );

            string multiSelectType;
            switch(this.Type)
            {
                case MultiSelectType.Critter:
                    multiSelectType = "critter";
                    break;

                case MultiSelectType.User:
                    multiSelectType = "user";
                    break;

                default:
                    throw new NotImplementedException($"{this.Type}");
            }

            builder.Append(
                $"<script>\n" +
                $"  $(function () {{\n" +
                $"      registerMultiSelect(document.getElementById('{filterId}'), " +
                $"                          document.getElementById('{addId}'), " +
                $"                          document.getElementById('{selectedId}'), " +
                $"                          document.getElementById('{leftToRightId}')," +
                $"                          document.getElementById('{rightToLeftId}')," +
                $"                          document.getElementById('{FormId}')," +
                $"                          '{multiSelectType}')\n" +
                $"  }});\n" +
                $"</script>\n"
            );

            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(builder.ToString());

            base.Process(context, output);
        }
    }
}
