using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Utilities;
using CMS.Entities;

namespace Eurona.User.Advisor {
    public partial class RegisterPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            registerControl.Continue += OnContinueClick;
        }

        protected void OnContinueClick(object sender, Account account) {
            string token = Cryptographer.Encrypt(account.Id.ToString());
            token = Server.UrlEncode(token);
            Response.Redirect(String.Format("~/user/advisor/registerUser.aspx?token={0}", token));
        }

    }
}
