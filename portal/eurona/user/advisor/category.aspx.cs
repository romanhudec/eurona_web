using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ShpResources = SHP.Resources.Controls;
using ShpCultureUtilities = SHP.Utilities.CultureUtilities;
using SHP.Controls.Cart;

namespace Eurona.User.Advisor
{
    /// <summary>
    /// Stranka zobrazi vsetky produkty v danej kategorii
    /// </summary>
    public partial class CategoryPage : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["id"]))
            {
                this.categoryNavigation.SelectedValue = Convert.ToInt32(Request["id"]);
            }
        }
        protected void OnLoadProducts(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["id"]))
            {
                this.categoryControl.CategoryId = Convert.ToInt32(Request["id"]);
                this.Title = this.categoryControl.Title;

                this.productsControl.CategoryId = Convert.ToInt32(Request["id"]);
                this.categoryNavigation.SelectedValue = Convert.ToInt32(Request["id"]);
                //Session["selectedCategory"] = Convert.ToInt32( Request["id"] );
            }
        }

        protected void OnProductAddedToChart(object sender, EventArgs e)
        {
            (this.Master as PageMasterPage).UpdateCartInfo();
        }
    }
}
