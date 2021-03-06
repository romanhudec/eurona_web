﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;
using CMS;

namespace Eurona.Admin
{
	public partial class AdminMasterPage : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Security.IsLogged(true);

			if (!Security.IsInRole(Role.ADMINISTRATOR))
			{
				Response.Redirect("~/right.aspx");
				return;
			}
		}
	}
}
