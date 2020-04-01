using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona {
    public partial class LogOut : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            Security.Logout();
           // CookiesUtils.CreateCookie(this, "cancel_q_id", "1");
            Response.Redirect("~/?cancel_q_id=1");
        }
    }
}
