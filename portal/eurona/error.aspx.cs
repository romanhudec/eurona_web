using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona
{
	public partial class ErrorPage : System.Web.UI.Page
	{
        protected string locale = "cs";
        protected string errorCode = "500";
		protected void Page_Load(object sender, EventArgs e)
		{
            locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            string queryCode = Request.QueryString["code"];
            int code = 500;
            Int32.TryParse(queryCode, out code);
            if (code >= 400 && code < 500) {
                errorCode = "400";
            } else {
                errorCode = "500";
            }
		}
	}
}