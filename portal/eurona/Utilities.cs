using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.Web.UI;
using System.Globalization;
using System.Collections.Specialized;

namespace Eurona {
    public static class Utilities {
        public static string Root(HttpRequest request) {
            return CMS.Utilities.ServerUtilities.Root(request);
        }

        public static string GetUserIP(HttpRequest request) {
            string ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList)) {
                return ipList.Split(',')[0];
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }

        public static void RedirectWithPost(Page page, string url, NameValueCollection args) {

            page.Response.Clear();
            if (url.StartsWith("/")) url = url.Remove(0, 1);
            url = Utilities.Root(page.Request) + url;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", url);
            foreach(string key  in args.Keys){
                sb.AppendFormat("<input type='hidden' name='{0}' value='{1}'>", key, args.Get(key));
            }
            // Other params go here
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");

            page.Response.Write(sb.ToString());
            page.Response.End();
        }
    }
}
