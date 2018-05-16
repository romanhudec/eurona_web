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

namespace Eurona.EShop {
    /// <summary>
    /// Stranka zobrazi vsetky produkty v danej kategorii
    /// </summary>
    public partial class CategoryPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(Request["id"])) {
                int categoryId = 0;
                Int32.TryParse((Request["id"]), out categoryId);
                if (categoryId == 0) Response.Redirect("~/");
                (this.Master as Eurona.EShop.DefautMasterPage).SelectedCategory = categoryId;
            }

            this.productsControl.OnProductAddedToChart += new EventHandler(productsControl_OnProductAddedToChart);
        }
        void productsControl_OnProductAddedToChart(object sender, EventArgs e) {
            this.UpdateCartInfo();
        }
        /// <summary>
        /// Update informácie v nákupnom košiku.
        /// </summary>
        public void UpdateCartInfo() {
            (this.Master as Eurona.EShop.DefautMasterPage).UpdateCartInfo();
        }
        protected void OnLoadProducts(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(Request["id"])) {
                this.categoryControl.CategoryId = Convert.ToInt32(Request["id"]);
                this.Title = this.categoryControl.Title;

                this.productsControl.CategoryId = Convert.ToInt32(Request["id"]);
                (this.Master as Eurona.EShop.DefautMasterPage).SelectedCategory = Convert.ToInt32(Request["id"]);
                //Session["selectedCategory"] = Convert.ToInt32( Request["id"] );
            }
        }
    }
}
