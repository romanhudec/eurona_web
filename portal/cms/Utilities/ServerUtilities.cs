using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.Web.UI;
using System.Globalization;

namespace CMS.Utilities
{
	public static class ServerUtilities
	{
		public static string Root(HttpRequest request)
		{
			string root;
			StringBuilder url = new StringBuilder();
			string port = request.ServerVariables["SERVER_PORT"];
			if (!String.IsNullOrEmpty(port) && port != "80" && port != "443") port = ":" + port; else port = String.Empty;
			string protocol = request.ServerVariables["SERVER_PORT_SECURE"];
			if (String.IsNullOrEmpty(protocol) || protocol == "0") protocol = "http://"; else protocol = "https://";
			url.Append(protocol);
			url.Append(request.ServerVariables["SERVER_NAME"]);
			url.Append(port);
			if (request.ApplicationPath != "/") url.Append(request.ApplicationPath);
			root = url.ToString();
			if (!root.EndsWith("/")) root = root + "/";
			return root;
		}

		private static string ResolveUrl(HttpContext context, string url)
		{
			System.Web.UI.Page p = context.Handler as System.Web.UI.Page;
			if (p != null) return p.ResolveUrl(url);
			else return ResolveUrl(context.Request, url);
		}

		public static string ResolveUrl(HttpRequest request, string url)
		{
			string absUrl = VirtualPathUtility.ToAbsolute(url);
			absUrl = absUrl.StartsWith("/") ? absUrl.Remove(0, 1) : absUrl;
			return CMS.Utilities.ServerUtilities.Root(request) + absUrl;
		}
	}
}
