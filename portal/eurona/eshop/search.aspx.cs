using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SHP.Entities;

namespace Eurona.EShop {
    public partial class SearchResultPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();
            if (!string.IsNullOrEmpty(Request["keywords"])) {
                this.productsControl.Filter = new Eurona.Common.DAL.Entities.Product.ReadByFilter { Expression = Request["keywords"], ProdejUkoncen=false, OnWeb=true };
            }
        }
    }
}
