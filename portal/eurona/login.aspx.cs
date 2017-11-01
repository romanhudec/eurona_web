using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;

namespace Eurona
{
	public partial class Login : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Security.IsLogged(false) && Security.Account.IsInRole(Role.ADMINISTRATOR)) return;
			if (!Request.QueryString.ToString().Contains("login"))
			{
				//Redirect na default page, kde sa zobrazi login box.
				Response.Redirect("~/default.aspx?login");
				return;
			}
			//this.login.OnContinueRegistration += OnContinueRegistration;
		}

        //protected void OnContinueRegistration(Account account)
        //{
        //    Response.Redirect(String.Format("~/user/advisor/registerUser.aspx?token={0}", account.Id));
        //}
	}
}
