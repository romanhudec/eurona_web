using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;

namespace Eurona.Admin
{
	public partial class WebErrorDetailPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if( String.IsNullOrEmpty(Request["Id"]) ) return;
			int id = Convert.ToInt32(Request["Id"]);

			Eurona.Common.DAL.Entities.Error error = Storage<Error>.ReadFirst(new Error.ReadById { Id = id });
			if (error == null) return;

			this.lblName.Text = error.Name;
			this.lblDate.Text = error.Stamp.ToString();
			this.lblLocation.Text = error.Location;
			this.lblExeption.Text = error.Exception;
			this.lblStackTrace.Text = error.StackTrace.Replace(Environment.NewLine, "<br />");
		}

		protected void OnBack(object sender, EventArgs e)
		{
			Response.Redirect("~/admin/webErrors.aspx");
		}
	}
}
