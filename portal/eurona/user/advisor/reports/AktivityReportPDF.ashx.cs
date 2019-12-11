using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;

namespace Eurona.User.Advisor.Reports {
    /// <summary>
    /// Summary description for AktivityReportPDF
    /// </summary>
    public class AktivityReportPDF : IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            // Sending headers
            string obdobi = context.Request.QueryString["obdobi"];
            string id = context.Request.QueryString["id"];

            byte[] pdfData = this.GetReportData(obdobi, id);
            if (pdfData == null || pdfData.Length == 0) context.Response.Redirect("~/user/advisor/reports/aktivityReportPoradce.aspx");

            string fileName = string.Format("{0}.pdf", obdobi);
            //string filePath = Path.Combine( Path.GetTempPath(), fileName );
            //using ( FileStream fs = new FileStream( filePath, FileMode.Create, FileAccess.Write ) )
            //{
            //    fs.Write( pdfData, 0, pdfData.Length );
            //}

            context.Response.Clear();
            context.Response.ContentType = "application/pdf";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Charset = "utf-8";
            context.Response.AppendHeader("content-disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            context.Response.BinaryWrite(pdfData);
            context.Response.End();
        }

        private byte[] GetReportData(string obdobi, string id) {
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(this.ConnectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = string.Empty;
                sql = @" SELECT arfile FROM provize_finalni_reporty WHERE RRRRMM=@RRRRMM AND Id_odberatele=@Id_odberatele";

                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@RRRRMM", obdobi), new SqlParameter("@Id_odberatele", id));
                if (dt.Rows.Count == 0) return null;
                else return (byte[])dt.Rows[0]["arfile"];
            }
        }

        /// <summary>
        /// Vrati connection string do TVD
        /// </summary>
        public string ConnectionString {
            get {
                return ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            }
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}