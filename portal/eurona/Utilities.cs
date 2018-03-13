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

        public static void RedirectWithPost(Page page, string url, NameValueCollection args, bool newPage) {
            /*
            page.Response.Clear();
            if (url.StartsWith("/")) url = url.Remove(0, 1);
            url = Utilities.Root(page.Request) + url;

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");           
            sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
            sb.AppendFormat(@"<form name='form' action='{0}' method='post'>", url);           
            foreach(string key  in args.Keys){
                sb.AppendFormat("<input type='hidden' name='{0}' value='{1}'>", key, args.Get(key));
            }
            // Other params go here
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");

            page.Response.Write(sb.ToString());
            page.Response.End();
            */

            if (url.StartsWith("/")) url = url.Remove(0, 1);
            url = Utilities.Root(page.Request) + url;

            string js = @"
            var form = document.createElement(""form"");
            form.setAttribute(""method"", ""post"");
            form.setAttribute(""action"", """+url+@""");

            // setting form target to a window named 'formresult'
            form.setAttribute(""target"", ""_self"");";

            foreach(string key  in args.Keys){
                js+=@"var hiddenField_"+key+@" = document.createElement(""input"");";
                js += @"hiddenField_" + key + @".setAttribute(""name"", """ + key + @""");";
                js += @"hiddenField_" + key + @".setAttribute(""value"", """ + args.Get(key) + @""");";
                js += "form.appendChild(hiddenField_" + key + @");";
            }
            js+=@"            
            document.body.appendChild(form);

            // creating the 'formresult' window with custom features prior to submitting the form
            window.open('" + url + @"', '_blank', 'height=600,width=800,resizable=yes,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no');
            form.submit();";

            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "showOrder", js, true);

        }

         public static void RegisterShowOrderPOSTFunction(Page page, string url) {
            if (url.StartsWith("/")) url = url.Remove(0, 1);
            url = Utilities.Root(page.Request) + url;

            string formName = Guid.NewGuid().ToString();
            string js = @"
            function showOrder(orderNumber){
            var form = document.createElement(""form"");
            form.setAttribute(""method"", ""post"");
            form.setAttribute(""action"", """ + url + @""");

            // setting form target to a window named 'formresult'
            form.setAttribute(""target"", """ + formName + @""");";
            js += @"var hiddenField = document.createElement(""input"");";
            js += @"hiddenField.setAttribute(""name"", ""orderNumber"");";
            js += @"hiddenField.setAttribute(""value"", orderNumber);";
            js += "form.appendChild(hiddenField);";
            js += @"            
            document.body.appendChild(form);

            // creating the 'formresult' window with custom features prior to submitting the form
            window.open('" + url + @"', '" + formName + @"', 'height=700,width=800,resizable=yes,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,copyhistory=no');
            form.submit();
            }";

            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "showOrder", js, true);
        }
    }
}
