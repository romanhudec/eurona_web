using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Eurona.Operator
{
	public partial class NewsletterAccounts : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void OnExport(object sender, EventArgs e)
		{
			RadGrid grid = this.adminAccounts.GetGridView();
			if (grid == null) return;

			grid.ExportSettings.IgnorePaging = true;
			grid.ExportSettings.FileName = "Prijemci_tiskovych_a_el_materialu.xls";
			grid.MasterTableView.ExportToExcel();
		}
	}
}
