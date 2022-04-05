using System;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using YeetCarAccidents.Models.ViewModels;

namespace bookstore.Infrastructure
{
	[HtmlTargetElement("div", Attributes = "page-info")] //@todo: read up on this
	public class PaginationTagHelper : TagHelper
	{
		private IUrlHelperFactory urlHelperFactory;

		public PaginationTagHelper(IUrlHelperFactory factory)
		{
			urlHelperFactory = factory;
		}

		//@todo: learn about ViewContext
		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext viewContext { get; set; }

		public PageInfo PageInfo { get; set; }
		public string PageAction { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
			//base.Process(context, output);
			IUrlHelper helper = urlHelperFactory.GetUrlHelper(viewContext); //@todo: Figure out what this does and why we shouldn\"t call the base method

			//TODO: Consider using the normal tag builder objects and applying properties.
			//TODO: Add 'Prev' and 'Next' buttons to be flashy...

			//Use bootstrap styles for paginaiton (thus the stringbuilder)
            StringBuilder _output = new StringBuilder(
				"<nav aria-label=\"Results Navigation\">" +
					"<ul class=\"pagination\">");

			for (int i = 1; i <= PageInfo.PageCount; i++)
            {
				_output.Append( //conditionally add active state
					i == PageInfo.CurrentPage ? "<li class=\"page-item active\">" : "<li class=\"page-item\">"
					);
				_output.Append(
					$"<a class=\"page-link\" href={helper.Action(PageAction, new { pageNum = i })}>{i}</a></li>"
					);
            }
			_output.Append("</ul></nav>");
			//This was tricky. I had to convert the string to an HtmlString, else it would render as plain text.
			output.Content.SetHtmlContent(new HtmlString(_output.ToString()));

		}
    }
}

