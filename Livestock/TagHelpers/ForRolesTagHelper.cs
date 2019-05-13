using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Website.Other;

namespace Website.TagHelpers
{
    public enum ForRolesAction
    {
        Show,
        Hide
    }

    /// <summary>
    /// Performs a certain action on the child content depending on what roles the user is in.
    /// </summary>
    [HtmlTargetElement("for-roles")]
    public class ForRoles : TagHelper
    {
        readonly IHttpContextAccessor _http;

        /// <summary>
        /// Comma seperated list of roles to hide the content for.
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// The action to perform if the user matches any of the given roles.
        /// </summary>
        public ForRolesAction Action { get; set; }

        public ForRoles(IHttpContextAccessor http)
        {
            this._http = http;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var roles = this.Roles.Split(",").Select(str => str.ToLower());
            if (roles.Any(r => this._http.HttpContext.User.IsInRole(r)))
            {
                switch(this.Action)
                {
                    case ForRolesAction.Hide: this.HideContent(output); break;
                    case ForRolesAction.Show: this.ShowContent(output); break;
                }
            }
            else
            {
                // Do the opposite.
                switch (this.Action)
                {
                    case ForRolesAction.Hide: this.ShowContent(output); break;
                    case ForRolesAction.Show: this.HideContent(output); break;
                }
            }
        }

        private void ShowContent(TagHelperOutput output)
        {
            // Remove the tag we're using, since it can mess up styling in certain cases.
            // This will still preserve the child content though.
            output.TagName = null;
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        private void HideContent(TagHelperOutput output)
        {
            output.SuppressOutput();
        }
    }
}
