using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;
using CMS;

namespace Eurona.User.Advisor
{
		public partial class ChangePasswordPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( Request["id"] ) ) return;
						this.changePassword.AccountId = Convert.ToInt32( Request["id"] );
				}
		}
}
