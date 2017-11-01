using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Eurona.Common.DAL.Entities;
using OrderEntity = Eurona.DAL.Entities.Order;
using AccountEntity = Eurona.DAL.Entities.Account;
using SettingsEntity = Eurona.Common.DAL.Entities.Settings;
using Eurona.Common.Controls.Cart;
using Eurona.Common;
using System.Configuration;

namespace Eurona.User.Advisor {
    public partial class CartPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            this.cartControl.OnCartItemsChanged += new EventHandler(cartControl_OnCartItemsChanged);
            /*
            this.divOrderForUser.Visible = false;
            if (this.cartControl.CartEntity == null) return;

            //Objednavku za ineho pouizvatela moze iba Top manager
            if ((Page.Master as PageMasterPage).LogedAdvisor.TopManager == 1)
                this.divOrderForUser.Visible = true;

            //Ak je uzavierka nie je mozne vytvarat objednavky
            if (Security.Account.IsInRole(Role.ADVISOR) && Eurona.Common.Application.EuronaUzavierka.IsUzavierka4Advisor())
                this.divOrderForUser.Visible = false;

            if (!IsPostBack) {
                this.ddlRegion.Items.Clear();
                List<ListItem> items = new Eurona.Common.Controls.UserManagement.OrganizationControl.Hepler().GetRegions();
                foreach (ListItem item in items)
                    this.ddlRegion.Items.Add(new RadComboBoxItem(item.Text, item.Value));

                RadComboBoxItem itemEmpty = new RadComboBoxItem(string.Empty, string.Empty);
                this.ddlRegion.Items.Insert(0, itemEmpty);
            }
            */

            //Upozornenie o automatickom vyprazdnovani kosiku v urxenu hodinu
            SettingsEntity settingsVysypaniVsechKosiku = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ESHOP_VYSYPANIVSECHKOSIKU" });
            if (settingsVysypaniVsechKosiku != null) {
                SettingsEntity.VysypaniVsechKosikuValue value = SettingsEntity.ParseVysypaniVsechKosikuStringValue(settingsVysypaniVsechKosiku);
                if (value.Povelena) {
                    this.lblAutomaticEmptyCartMessage.Text = String.Format(Resources.EShopStrings.AutomaticEmptyCartWarning, value.Cas);
                    this.divAutomaticEmptyCartMessage.Visible = true;
                }
            }

            //Info o placeni postovneho
            this.SetupInfoOplaceniPostovneho();

            this.txtKod.Focus();
        }

        private void SetupInfoOplaceniPostovneho() {
            this.lblPostovneInfo.Visible = false;
        }

        void cartControl_OnCartItemsChanged(object sender, EventArgs e) {
            (this.Page.Master as Eurona.User.Advisor.PageMasterPage).UpdateCartInfo();
            //Info o placeni postovneho
            this.SetupInfoOplaceniPostovneho();
        }

        #region Private Handlert Vytvorenia objednavky za ineho pouzivatela
        /*
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
            this.cartControl.CreateOrder(ownerAccountId, true);
        }
        */
        protected void OnAddCart(object sender, EventArgs e) {
            int quantity = 1;
            if (!Int32.TryParse(this.txtMnozstvi.Text, out quantity)) quantity = 1;

            Product p = Storage<Product>.ReadFirst(new Product.ReadByCode { Code = this.txtKod.Text });
            if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod.Text, p, quantity, this))
                return;
            if (!EuronaCartHelper.AddProductToCart(this.Page, p.Id, quantity, this))
                return;

            ////Alert s informaciou o pridani do nakupneho kosika
            //string js = string.Format( "alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();",
            //    string.Format( SHP.Resources.Controls.AdminProductControl_ProductWasAddedToCart_Message, p.Name, quantity ),
            //   this.Request.RawUrl.Contains( "?" ) ? "&" : "?" );
            //this.Page.ClientScript.RegisterStartupScript( this.GetType(), "addProductToCart", js, true );

            Response.Redirect(this.Request.RawUrl);
        }
        #endregion
    }
}
