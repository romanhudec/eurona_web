using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;
using Eurona.Common.DAL.Entities;
using Telerik.Web.UI;

namespace Eurona.EShop.User {
    public partial class OrdersPage : WebPage {
        public static decimal MIN_ASSOCIATION_PRICE_CZK = 500m;
        public static decimal MIN_ASSOCIATION_PRICE_EUR = 18.5m;
        public static decimal MIN_ASSOCIATION_PRICE_PLN = 83m;

        public static decimal GetMinAssociationPrice(string locale) {
            decimal minAssociationPrice = MIN_ASSOCIATION_PRICE_CZK;
            if (Security.Account.Locale == "cs") minAssociationPrice = MIN_ASSOCIATION_PRICE_CZK;
            if (Security.Account.Locale == "sk") minAssociationPrice = MIN_ASSOCIATION_PRICE_EUR;
            if (Security.Account.Locale == "pl") minAssociationPrice = MIN_ASSOCIATION_PRICE_PLN;
            return minAssociationPrice;
        }

        private string message = string.Empty;

        protected void Page_Load(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Request["type"])) return;

            this.lblMessage.Text = "";
            /* DOCASNE ZAKOMENTOVANE ESTE AJ V 'Eurona.User.Operator.MyOrdersPage' a 'Eurona.User.Advisor.OrdersPage'*/
            /*Povolene od 01.11.2017*/
            //-------------------------------------------------------------//
            decimal minAssociationPrice = GetMinAssociationPrice(Security.Account.Locale);
            message = Resources.EShopStrings.AdminOrdersControl_AsociatePriceLimitMessage;
            message = string.Format(message, minAssociationPrice);
            this.lblMessage.Text = message;
            //-------------------------------------------------------------//

            string type = Request["type"];
            divOrderAssociation.Visible = false;
            //Archiv objednavok
            if (type == "ar") {
                adminArchivedOrdersControl.Visible = true;
                adminActiveOrdersControl.Visible = false;
                divOrderAssociation.Visible = false;
                this.lblTitle.Text = Resources.EShopStrings.AdminOrdersControl_Title_OrderArchiv;
            }//Aktualne objednavky v stave "Caka na spracovanie"
            else if (type == "ac") {
                divOrderAssociation.Visible = false;
                adminActiveOrdersControl.Visible = true;
                adminArchivedOrdersControl.Visible = false;
                this.lblTitle.Text = Resources.EShopStrings.AdminOrdersControl_Title_ActiveOrders;

                EnsureChildControls();
                List<OrderEntity> list = adminActiveOrdersControl.GetOrdersNotAssociated();
                if (list.Count != 0) {
                    divOrderAssociation.Visible = true && Settings.IsZdruzeneObjednavkyPovolena();
                    if (!IsPostBack) {
                        this.ddlRegion.Items.Clear();
                        List<ListItem> items = new Eurona.Common.Controls.UserManagement.OrganizationControl.Hepler().GetRegions();
                        foreach (ListItem item in items)
                            this.ddlRegion.Items.Add(new RadComboBoxItem(item.Text, item.Value));

                        RadComboBoxItem itemEmpty = new RadComboBoxItem(string.Empty, string.Empty);
                        this.ddlRegion.Items.Insert(0, itemEmpty);
                    }
                }
            }

        }

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

        protected void OnAssociateOrders(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(this.ddlAdvisor.SelectedValue)) {
                string js = "alert('Vyberte poradce ke kterému mají být objednávky přidruženy!');showControl('tblOrdersAssociation');";
                btnAssociate.Page.ClientScript.RegisterStartupScript(btnAssociate.Page.GetType(), "OrderAssociation", js, true);
                return;
            }
            int associationAccountId = Convert.ToInt32(this.ddlAdvisor.SelectedValue);
            List<OrderEntity> list = this.adminActiveOrdersControl.GetSelectedOrdersToAssociation();
            if (list.Count == 0) {
                string js = "alert('Musíte označit aspoň jednu objednávku, která má být přidružená!');showControl('tblOrdersAssociation');";
                btnAssociate.Page.ClientScript.RegisterStartupScript(btnAssociate.Page.GetType(), "OrderAssociation", js, true);
                return;
            }

            /* DOCASNE ZAKOMENTOVANE ESTE AJ V 'Eurona.User.Operator.MyOrdersPage' a 'Eurona.User.Advisor.OrdersPage'*/
            /*Povolene od 01.11.2017*/
            //-------------------------------------------------------------//
            //Kontrola katalogovych cien objednavok
            foreach (OrderEntity order in list) {
                decimal katalogovaCena = order.CartEntity.KatalogovaCenaCelkemByEurosap;//order.PriceWVAT;

                //Ceny striktní > 500Kč/83zl/18,5euro.
                decimal minAssociationPrice = GetMinAssociationPrice(Security.Account.Locale);
                String message = Resources.EShopStrings.AdminOrdersControl_AsociatePriceLimitMessage;
                message = string.Format(message, minAssociationPrice);

                if (katalogovaCena < minAssociationPrice) {
                    string js = string.Format("alert('" + message + "');showControl('tblOrdersAssociation');", minAssociationPrice);
                    btnAssociate.Page.ClientScript.RegisterStartupScript(btnAssociate.Page.GetType(), "OrderAssociationCatalogPriceCheck", js, true);
                    return;
                }
            }
            //-------------------------------------------------------------//

            //Ulozenie objednavok
            foreach (OrderEntity order in list) {
                order.AssociationAccountId = associationAccountId;
                order.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.WaitingToAccept;
                Storage<OrderEntity>.Update(order);
            }

            Response.Redirect(this.Request.RawUrl);
        }

    }
}
