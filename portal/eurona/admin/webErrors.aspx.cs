using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin
{
	public partial class WebErrorsPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.gridView.DataSource = Storage<Eurona.Common.DAL.Entities.Error>.Read();
			if (!IsPostBack)
			{
				this.gridView.DataBind();
			}
		}
	}
}
