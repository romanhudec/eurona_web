using Eurona.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User.Anonymous {
    public partial class RequestEmailVerifycation : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Request["status"]) && string.IsNullOrEmpty(Request["id"])) {
                Security.IsLogged(true);
            }
        }
    }
}
