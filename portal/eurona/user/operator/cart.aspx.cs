using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Eurona.Common.DAL.Entities;
using System.Text;
using Eurona.Common.Controls.Cart;
using OrderEntity = Eurona.DAL.Entities.Order;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using AddressEntity = SHP.Entities.Address;
using Eurona.Common;

namespace Eurona.User.Operator {
    public partial class CartPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (this.cartControl.CartEntity == null) this.divOrderForUser.Visible = false;

            this.ddlShipment.DataSource = Storage<ShipmentEntity>.Read();
            this.ddlShipment.DataTextField = "Name";
            this.ddlShipment.DataValueField = "Code";

            if (!IsPostBack) {
                this.ddlRegion.Items.Clear();
                List<ListItem> items = new Eurona.Common.Controls.UserManagement.OrganizationControl.Hepler().GetRegions();
                foreach (ListItem item in items)
                    this.ddlRegion.Items.Add(new RadComboBoxItem(item.Text, item.Value));

                RadComboBoxItem itemEmpty = new RadComboBoxItem(string.Empty, string.Empty);
                this.ddlRegion.Items.Insert(0, itemEmpty);

                this.ddlShipment.DataBind();
            }

            #region js on click vytvaram objednavku
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("this.value = '{0} ...';", Resources.EShopStrings.CartControl_CreateOrderButton_Text);
            sb.Append("this.disabled = true;");
            sb.Append(Page.ClientScript.GetPostBackEventReference(this.btnCreateOrder, null) + ";");
            //sb.Append( "alert('Objednávka byla úspěšne vytvořena.');" );

            string submit_button_onclick_js = sb.ToString();
            btnCreateOrder.Attributes.Add("onclick", submit_button_onclick_js);
            #endregion
        }

        #region Private Handlert Vytvorenia objednavky za ineho pouzivatela
        protected void OnDdlAdvisor_DataBound(object sender, EventArgs e) {
            //set the initial footer label
            ((Literal)ddlAdvisor.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(ddlAdvisor.Items.Count);
        }

        protected void OnDdlAdvisor_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e) {
            //List<Organization> list = Storage<Organization>.Read();
            //this.ddlAdvisor.DataSource = list;
            //this.ddlAdvisor.DataBind();
            OnFindAdvisor(sender, null);
        }
        protected void OnDdlAdvisor_ItemDataBound(object sender, RadComboBoxItemEventArgs e) {
            //set the Text and Value property of every item
            //here you can set any other properties like Enabled, ToolTip, Visible, etc.
            e.Item.Text = ((Organization)e.Item.DataItem).Name;
            e.Item.Value = ((Organization)e.Item.DataItem).AccountId.ToString();
        }
        protected void OnFindAdvisor(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(this.txtAdvisorName.Text) && string.IsNullOrEmpty(this.txtCity.Text) && string.IsNullOrEmpty(this.ddlRegion.SelectedValue) && string.IsNullOrEmpty(this.txtAdvisorCode.Text)) {
                this.ddlAdvisor.DataSource = new List<Organization>();
                this.ddlAdvisor.DataBind();
                this.ddlAdvisor.Text = string.Empty;
                return;
            }
            string city = string.IsNullOrEmpty(txtCity.Text) ? null : txtCity.Text;
            string advisorCode = string.IsNullOrEmpty(txtAdvisorCode.Text) ? null : txtAdvisorCode.Text;
            string advisorName = string.IsNullOrEmpty(txtAdvisorName.Text) ? null : txtAdvisorName.Text;
            string regionCode = string.IsNullOrEmpty(ddlRegion.SelectedValue) ? null : ddlRegion.SelectedValue;

            List<Organization> list = Storage<Organization>.Read(new Organization.ReadBy { City = city, Name = advisorName, RegionCode = regionCode, Code = advisorCode });
            this.ddlAdvisor.DataSource = list;
            this.ddlAdvisor.DataBind();
        }
        protected void OnCreateOrderForUser(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(this.ddlAdvisor.SelectedValue)) {
                string js = "alert('Vyberte poradce kterému se má objednávka vytvořit!');showControl('tblOrdersForUser');";
                btnCreateOrder.Page.ClientScript.RegisterStartupScript(btnCreateOrder.Page.GetType(), "OrderForUser", js, true);
                return;
            }

            int ownerAccountId = Convert.ToInt32(this.ddlAdvisor.SelectedValue);
            this.cartControl.CartEntity.ShipmentCode = this.ddlShipment.SelectedValue;
            this.cartControl.CreateOrder(ownerAccountId, true);
        }
        protected void OnAddCart(object sender, EventArgs e) {
            int quantity = 1;
            if (!Int32.TryParse(this.txtMnozstvi.Text, out quantity)) quantity = 1;
            if (quantity == 0) return;

            Product p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod.Text });
            if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod.Text, p, quantity, false, this, true))
                return;
            if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                return;

            ////Alert s informaciou o pridani do nakupneho kosika
            //string js = string.Format( "alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();",
            //    string.Format( SHP.Resources.Controls.AdminProductControl_ProductWasAddedToCart_Message, p.Name, quantity ),
            //   this.Request.RawUrl.Contains( "?" ) ? "&" : "?" );
            //this.Page.ClientScript.RegisterStartupScript( this.GetType(), "addProductToCart", js, true );

            Response.Redirect(this.Request.RawUrl);
        }

        protected void OnAddCartHz(object sender, EventArgs e) {
            int quantity = 1;
            Product p = null;
            string alert = string.Empty;

            #region Row1
            if (!Int32.TryParse(this.txtMnozstvi1.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod1.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod1.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod1.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row2
            if (!Int32.TryParse(this.txtMnozstvi2.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod2.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod2.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod2.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row3
            if (!Int32.TryParse(this.txtMnozstvi3.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod3.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod3.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod3.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row4
            if (!Int32.TryParse(this.txtMnozstvi4.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod4.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod4.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod4.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row5
            if (!Int32.TryParse(this.txtMnozstvi5.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod5.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod5.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod5.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row6
            if (!Int32.TryParse(this.txtMnozstvi6.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod6.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod6.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod6.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row7
            if (!Int32.TryParse(this.txtMnozstvi7.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod7.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod7.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod7.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row8
            if (!Int32.TryParse(this.txtMnozstvi8.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod8.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod8.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod8.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row9
            if (!Int32.TryParse(this.txtMnozstvi9.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod9.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod9.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod9.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row10
            if (!Int32.TryParse(this.txtMnozstvi10.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod10.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod10.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod10.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row11
            if (!Int32.TryParse(this.txtMnozstvi11.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod11.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod11.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod11.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row12
            if (!Int32.TryParse(this.txtMnozstvi12.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod12.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod12.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod12.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row13
            if (!Int32.TryParse(this.txtMnozstvi13.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod13.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod13.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod13.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row14
            if (!Int32.TryParse(this.txtMnozstvi14.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod14.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod14.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod14.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion

            #region Row15
            if (!Int32.TryParse(this.txtMnozstvi15.Text, out quantity)) quantity = 1;
            if (!string.IsNullOrEmpty(this.txtKod15.Text)) {
                p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod15.Text });
                if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod15.Text, p, quantity, false, this, true))
                    return;
                if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, false, this, true))
                    return;
            }
            #endregion
            Response.Redirect(this.Request.RawUrl);
        }
        #endregion
    }
}
