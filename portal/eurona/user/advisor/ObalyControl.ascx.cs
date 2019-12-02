using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProductEntity = Eurona.Common.DAL.Entities.Product;

namespace Eurona.user.advisor {
    public partial class ObalyControl : Eurona.Common.Controls.UserControl {
        public string CssClass { get; set; }
        private List<ProductEntity> dataSource = null;


        public delegate void OnAddObalProductHandler(int productId, int quantity);
        public event OnAddObalProductHandler OnAddObalProduct;

        protected void Page_Load(object sender, EventArgs e) {
            dataSource = Storage<ProductEntity>.Read(new ProductEntity.ReadByFilter { Obal = true });
            if (!IsPostBack && this.rpObaly != null) {
                this.rpObaly.DataSource = dataSource;
                this.rpObaly.DataBind();
            }
        }


        protected void rpObaly_ItemCommand(object source, RepeaterCommandEventArgs e) {
            RepeaterItem ritem = (RepeaterItem)e.Item;
            TextBox txtPocetKs = (TextBox)ritem.FindControl("txtPocetKs");
            int productId = Convert.ToInt32(e.CommandArgument);

            int quantity = 1;
            if (!Int32.TryParse(txtPocetKs.Text, out quantity)) quantity = 1;
            if (OnAddObalProduct != null) {
                OnAddObalProduct(productId, quantity);
            }
        }

    }
}