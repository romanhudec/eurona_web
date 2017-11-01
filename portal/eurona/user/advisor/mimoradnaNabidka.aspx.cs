using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User.Advisor
{
	public partial class MimoradnaNabidkaPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Request["id"]))
				this.mimoradnaNabidkaControl.MimoradnaNabidkaId = Convert.ToInt32(this.Request["id"]);
		}
	}
}