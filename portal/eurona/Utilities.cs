using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.Web.UI;
using System.Globalization;

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
    }
}
