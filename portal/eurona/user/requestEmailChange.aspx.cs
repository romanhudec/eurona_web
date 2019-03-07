using Eurona.Controls;
using Eurona.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User {
    public partial class RequestEmailChange : WebPage {
        protected void Page_Load(object sender, EventArgs e) {

            if (string.IsNullOrEmpty(Request["code"])) {
                Security.IsLogged(true);
            } else {
                string[] data = Eurona.User.emailVerifycationService.decodeVerifyCode(Request["code"]);
                string emailFromCode = data[0];
                int accountFromCode = Convert.ToInt32(data[1]);
                Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = accountFromCode });
                if (account != null) {
                    Security.Login(account, false);
                }
            }
        }
    }
}
