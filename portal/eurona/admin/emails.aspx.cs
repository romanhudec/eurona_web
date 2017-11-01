using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin
{
	public partial class EmailsPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.gridView.DataSource = Storage<CMS.Entities.EmailLog>.Read();
			if (!IsPostBack)
			{
				this.gridView.DataBind();
			}
		}
	}
}
