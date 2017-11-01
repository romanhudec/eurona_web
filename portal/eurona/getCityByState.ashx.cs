using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eurona.Common;
using System.Data;
using System.Text;
using System.IO;

namespace Eurona {
    /// <summary>
    /// Summary description for getCityByState
    /// </summary>
    public class getCityByState : IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            string mesto = GetRequestData(context.Request);
            DataTable table = PSCHelper.GetTVDMestoBy(context.Request["stat"], mesto);
            StringBuilder sbJson = new StringBuilder();
            foreach (DataRow row in table.Rows) {
                if (sbJson.Length != 0) sbJson.Append(",");
                sbJson.AppendFormat("{{ \"Name\":\"{0}\", \"Psc\":\"{1}\" }}", row["Nazev"].ToString().Trim(), row["Psc"].ToString().Trim());
            }

            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write("{ \"d\":[" + sbJson.ToString() + "]}");
            context.Response.End();
        }

        /// <summary>
        /// Get request string from stream
        /// </summary>
        private string GetRequestData(HttpRequest request) {
            Stream stream = request.InputStream;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();
            return request.ContentEncoding.GetString(data);
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}