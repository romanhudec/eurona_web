using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eurona.Common;
using System.Data;
using System.Text;
using System.IO;
using Eurona.DAL.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona {
    /// <summary>
    /// Summary description for emailVerifycationService
    /// </summary>
    public class emailVerifycationService : IHttpHandler {

        public void ProcessRequest(HttpContext context) {
            string method = context.Request["method"];
            if (method == "checkEmail") {
                checkEmail(context);
            } else if (method == "sendEmail2EmailVerify") {
                sendEmail2EmailVerify(context);
            } else if (method == "verify") {
                verify(context);
            }
          
        }

        private void checkEmail(HttpContext context) {
            string email = GetRequestData(context.Request);
            Account account = Storage<Account>.ReadFirst(new Account.ReadByEmail { Email = email });
            StringBuilder sbJson = new StringBuilder();

            int status = 0;
            string message = "";
            if (account == null) {
                status = 1;
                message = Resources.Strings.EmailVerifyControl_EmailValidation_EmailNeexituje;
                //message = Resources.Strings.EmailVerifyControl_EmailValidation_EmailNeexituje;
            }

            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"Message\":\"{1}\" }}", status, message);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(sbJson.ToString());
            context.Response.End();
        }

        private void sendEmail2EmailVerify(HttpContext context) {
            string email = GetRequestData(context.Request);
            List<Error> account = Storage<Error>.Read();
            StringBuilder sbJson = new StringBuilder();

            int status = 0;
            string message = "";

            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"Message\":\"{1}\" }}", status, message);
            context.Response.ContentType = "application/json; charset=utf-8";            
            context.Response.Write(sbJson.ToString());
            context.Response.End();
        }


        private void verify(HttpContext context) {
            string email = GetRequestData(context.Request);
            List<Error> account = Storage<Error>.Read();
            StringBuilder sbJson = new StringBuilder();

            int status = 0;
            string message = "";

            sbJson.AppendFormat("{{ \"Status\":\"{0}\", \"Message\":\"{1}\" }}", status, message);
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Write(sbJson.ToString());
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