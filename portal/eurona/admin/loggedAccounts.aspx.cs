using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;

namespace Eurona.Admin
{
	public partial class LoggedAccountsPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			CMS.Utilities.RadGridUtilities.Localize(this.gridView);

			List<LoggedAccount> list = Storage<LoggedAccount>.Read(new LoggedAccount.ReadLogged { HasTVDId = true });
			this.gridView.DataSource = list;
			if (!IsPostBack) this.gridView.DataBind();
		}
	}
}
