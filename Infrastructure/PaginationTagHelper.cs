using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using YeetCarAccidents.Models.ViewModels;

namespace YeetCarAccidents.Infrastructure
{
	[HtmlTargetElement("div", Attributes = "page-info")]
	public class PaginationTagHelper : TagHelper
	{
		private IUrlHelperFactory urlHelperFactory;

		public PaginationTagHelper(IUrlHelperFactory factory)
		{
			urlHelperFactory = factory;
		}

		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext viewContext { get; set; }

		public PageInfo PageInfo { get; set; }
		public string PageAction { get; set; }
		public FilterInfo Filter { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
			IUrlHelper helper = urlHelperFactory.GetUrlHelper(viewContext);
			var filter = ObjectToDictionaryHelper.ToDictionary(Filter);

			//Use bootstrap styles for paginaiton
            StringBuilder _output = new StringBuilder(
				"<nav aria-label=\"Results Navigation\">" +
					"<ul class=\"pagination\">");

			for (int i = 1; i <= PageInfo.PageCount; i++)
            {
				//only bother to make a button if it is the current page, 1 on each side, min, max
				if (i == 1 || (i >= PageInfo.CurrentPage - 1 && i <= PageInfo.CurrentPage + 1) || i == PageInfo.PageCount)
                {
					var routeValues = new RouteValueDictionary
					{
						{ "pageNum", i }
					};
					foreach (var key in filter.Keys)
                    {
						if (filter[key].GetType().IsInstanceOfType(new {
						}))
							foreach (var innerkey in filter[key].ToDictionary().Keys)
								routeValues.Add(innerkey, filter[key].ToDictionary()[innerkey]);
						else
							routeValues.Add(key, filter[key]);
                    }


					if (i == 1 && PageInfo.CurrentPage > 1)
                    {
						_output.Append( //conditionally add active state
							i == PageInfo.CurrentPage ? "<li class=\"page-item active\" disabled>" : "<li class=\"page-item\">"
							);
						_output.Append(
                            $"<a class=\"page-link\" href={helper.Action(PageAction, routeValues)}>{i}</a ></li><li class=\"page-item\"><a class=\"page-link\">…</a></li>"
                        );
					}
					else if(i == PageInfo.PageCount && PageInfo.CurrentPage < PageInfo.PageCount - 1 )
                    {
						_output.Append("<li class=\"page-item\"><a class=\"page-link\">…</a></li>");
						_output.Append( //conditionally add active state
							i == PageInfo.CurrentPage ? "<li class=\"page-item active\" disabled>" : "<li class=\"page-item\">"
							);
						_output.Append(
							$"<a class=\"page-link\" href={helper.Action(PageAction, routeValues)}>{i}</a></li>"
							);
					}
                    else
                    {
						_output.Append( //conditionally add active state
							i == PageInfo.CurrentPage ? "<li class=\"page-item active\" disabled>" : "<li class=\"page-item\">"
							);
						_output.Append(
							$"<a class=\"page-link\" href={helper.Action(PageAction, routeValues)}>{i}</a></li>"
							);
					}

				}

            }
			_output.Append("</ul></nav>");

			output.Content.SetHtmlContent(new HtmlString(_output.ToString()));

		}
    }
}

