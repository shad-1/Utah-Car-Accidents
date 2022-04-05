using System;
using Microsoft.AspNetCore.Http;

namespace bookstore.Infrastructure
{
	public static class UrlExtensions
	{
		/** 
		 * For parsing a URL to determine the page which was navigated from
		 */
		public static string PathAndQuery(this HttpRequest request)
        {
			return request.QueryString.HasValue ? $"{request.Path}{request.QueryString}" : request.Path.ToString();
        }
	}
}

