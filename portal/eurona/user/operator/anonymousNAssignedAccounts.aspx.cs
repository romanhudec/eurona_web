using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;

namespace Eurona.Operator
{
	public partial class AnonymousAccountsPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			CMS.Utilities.RadGridUtilities.Localize(this.gridView);

			List<Organization> list = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration=true, Assigned=false });
			this.gridView.DataSource = list;
			if (!IsPostBack) this.gridView.DataBind();
		}
	}
}
