using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.EShop {
    /// <summary>
    /// Virtualny katalog vyrobkov
    /// </summary>
    public partial class CatalogSeabar : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            this.aThisPage.Text = Resources.Strings.Navigation_ProductsCatalog;
            this.DataBind();
        }
    }
}