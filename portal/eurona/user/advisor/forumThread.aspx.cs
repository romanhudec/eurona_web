using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona
{
	public partial class ForumThreadPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(Request["Id"]))
				this.forumsControl.ForumThreadId = Convert.ToInt32(Request["Id"]);
		}
	}
}
