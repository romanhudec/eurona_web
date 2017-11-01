using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;

namespace Eurona.Admin
{
	public partial class MimoradnaNabidkaPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			MimoradnaNabidka mn = Storage<MimoradnaNabidka>.ReadFirst();
			if (mn != null)
				this.adminMimoradnaNabidkaControl.MimoradnaNabidkaId = mn.Id;
		}
	}
}
