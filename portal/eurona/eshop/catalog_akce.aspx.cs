using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.EShop
{
	/// <summary>
	/// Virtualny katalog vyrobkov
	/// </summary>
	public partial class CatalogAkce : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			this.aThisPage.Text = Resources.Strings.Navigation_ProductsCatalog;
			this.DataBind();

			if (this.ShowCZ()) this.vc_cs.Visible = true;
			if (this.ShowPL()) this.vc_pl.Visible = true;
		}

		public bool ShowCZ()
		{
			if (System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "cs" ||
					System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "sk")
				return true;
			else return false;
		}
		public bool ShowPL()
		{
			return System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() == "pl";
		}

	}
}