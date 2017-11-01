using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Eurona;
using Eurona.DAL.Entities;
using System.Globalization;
using System.Threading;
using System.Configuration;

public partial class Page3ContentMasterPage : System.Web.UI.MasterPage
{
	/// <summary>
	/// Update informácie v nákupnom košiku.
	/// </summary>
	public void UpdateCartInfo()
	{
		(this.Master as PageMasterPage).UpdateCartInfo();
	}
}
