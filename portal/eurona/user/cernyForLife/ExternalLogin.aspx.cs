using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User.CernyForLifeUser
{
    public partial class ExternalLogin : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString.Count == 0)
            {
                RedirectToLoginPage(this.Response);
                return;
            }
            string id = this.Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                RedirectToLoginPage(this.Response);
                return;
            }

            int accountId = 0;
            if (Int32.TryParse(id, out accountId) == false)
            {
                RedirectToLoginPage(this.Response);
                return;
            }

            Eurona.DAL.Entities.Account account = Storage<Eurona.DAL.Entities.Account>.ReadFirst(new Eurona.DAL.Entities.Account.ReadById { AccountId = accountId });
            if (account == null)
            {
                RedirectToLoginPage(this.Response);
                return;
            }

            Security.Login(account);

            //Prepocet eurona kosika
            Eurona.Common.Controls.Cart.EuronaCartHelper.RecalculateOpenCart(this);
            this.Response.Redirect("~/user/advisor/cart.aspx");
        }

        private void RedirectToLoginPage(HttpResponse response)
        {
            response.Redirect("~/user/advisor/cart.aspx");
        }
    }
}