using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using Telerik.Web.UI;
using System.Text;
using Eurona.Controls;
using OrderEntity = Eurona.DAL.Entities.Order;
using AccountEntity = CMS.Entities.Account;
using EshopEmailNotification = SHP.EmailNotification;
using CMS.Utilities;

namespace Eurona.User.Operator {
    public partial class MyOrdersPage : WebPage {


        protected void Page_Load(object sender, EventArgs e) {

            this.lblMessage.Text = "";
            /* DOCASNE ZAKOMENTOVANE ESTE AJ V 'Eurona.User.Operator.MyOrdersPage' a 'Eurona.User.Advisor.OrdersPage'*/
            /*Povolene od 01.11.2017*/
            //-------------------------------------------------------------//
            decimal minAssociationPriceCS = Eurona.EShop.User.OrdersPage.GetMinAssociationPrice("cs");
            decimal minAssociationPriceSK = Eurona.EShop.User.OrdersPage.GetMinAssociationPrice("sk");
            decimal minAssociationPricePL = Eurona.EShop.User.OrdersPage.GetMinAssociationPrice("pl");

            string operatorLocale = Security.Account.Locale;
            string operatorCurrency = "Kč";
            if (operatorLocale == "sk") operatorCurrency = "Eur";
            if (operatorLocale == "pl") operatorCurrency = "Zl";

            String messageCs = Resources.EShopStrings.AdminOrdersControl_AsociatePriceLimitMessage.Replace(operatorCurrency, "Kč");
            String messageSk = Resources.EShopStrings.AdminOrdersControl_AsociatePriceLimitMessage.Replace(operatorCurrency, "Eur");
            String messagePl = Resources.EShopStrings.AdminOrdersControl_AsociatePriceLimitMessage.Replace(operatorCurrency, "Zl");
            messageCs = string.Format(messageCs, minAssociationPriceCS);
            messageSk = string.Format(messageSk, minAssociationPriceSK);
            messagePl = string.Format(messagePl, minAssociationPricePL);
            this.lblMessage.Text = messageCs + "<br/>" + messageSk + "<br/>" + messagePl;
            //-------------------------------------------------------------//

            if (!IsPostBack) {
                this.cbOnlyMyOrders.Checked = true;
                this.ddlRegion.Items.Clear();
                List<ListItem> items = new Eurona.Common.Controls.UserManagement.OrganizationControl.Hepler().GetRegions();
                foreach (ListItem item in items)
                    this.ddlRegion.Items.Add(new RadComboBoxItem(item.Text, item.Value));

                RadComboBoxItem itemEmpty = new RadComboBoxItem(string.Empty, string.Empty);
                this.ddlRegion.Items.Insert(0, itemEmpty);
            }

            if (this.cbOnlyMyOrders.Checked) this.adminOrdersControl.CreatedByAccountId = Security.Account.Id;
            else this.adminOrdersControl.CreatedByAccountId = null;

            #region js on click vytvaram objednavku
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("this.value = '{0} ...';", this.btnCreateOrder.Text);
            sb.Append("this.disabled = true;");
            sb.Append(Page.ClientScript.GetPostBackEventReference(this.btnCreateOrder, null) + ";");
            sb.Append("alert('Objednávky/a byly odeslány na zpracování.');");

            string submit_button_onclick_js = sb.ToString();
            this.btnCreateOrder.Attributes.Add("onclick", submit_button_onclick_js);
            #endregion
        }

        protected void OnOnlyMyOrdersCheckedChanged(object sender, EventArgs e) {
            if (this.cbOnlyMyOrders.Checked) this.adminOrdersControl.CreatedByAccountId = Security.Account.Id;
            else this.adminOrdersControl.CreatedByAccountId = null;

            this.cbSelecUnselectAll.Checked = false;
            this.adminOrdersControl.GridViewDataBind(true);
        }

        protected void OnSelectUnselectAll(object sender, EventArgs e) {
            this.adminOrdersControl.SelectUnselectAll();
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

            //Najdem objednavka, ktoru som vytvoril JA (Admin/Operator) a je to objednavka pouzivatela, ktoremu ju chcem pridruzit.
            Order parentOrder = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadByFilter { AccountId = associationAccountId, HasChilds = false, CreatedByAccountId = Security.Account.Id, OrderStatusCode = ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString() });

            List<OrderEntity> list = this.adminOrdersControl.GetSelectedOrdersToAssociation();
            if (list.Count == 0) {
                string js = "alert('Musíte označit aspoň jednu objednávku, která má být přidružená!');showControl('tblOrdersAssociation');";
                btnAssociate.Page.ClientScript.RegisterStartupScript(btnAssociate.Page.GetType(), "OrderAssociation", js, true);
                return;
            }

            /* DOCASNE ZAKOMENTOVANE ESTE AJ V 'Eurona.User.Operator.MyOrdersPage' a 'Eurona.EShop.User.OrdersPage'*/
            /*Povolene od 01.11.2017*/
            //-------------------------------------------------------------//
            //Kontrola katalogovych cien objednavok
            foreach (OrderEntity order in list) {
                decimal katalogovaCena = order.CartEntity.KatalogovaCenaCelkemByEurosap;//order.PriceWVAT;

                //Ceny striktní > 500Kč/83zl/18,5euro.
                decimal minAssociationPrice = Eurona.EShop.User.OrdersPage.GetMinAssociationPrice(order.CartEntity.Locale);
                String message = Resources.EShopStrings.AdminOrdersControl_AsociatePriceLimitMessage;
                message = string.Format(message, minAssociationPrice);

                if (katalogovaCena < minAssociationPrice) {
                    string js = string.Format("alert('" + message + "');showControl('tblOrdersAssociation');");
                    btnAssociate.Page.ClientScript.RegisterStartupScript(btnAssociate.Page.GetType(), "OrderAssociationCatalogPriceCheck", js, true);
                    return;
                }
            }
            //-------------------------------------------------------------//

            //Ulozenie objednavok
            foreach (OrderEntity order in list) {
                order.AssociationAccountId = associationAccountId;

                //Ak existuje objednavka "hlavna objednavka"
                if (parentOrder != null) {
                    order.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.Accepted;//Operator automaticky akceptuje potvrdenie
                    order.ParentId = parentOrder.Id;
                } else
                    order.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.WaitingToAccept;

                Storage<OrderEntity>.Update(order);
            }

            if (parentOrder != null) {
                string jsAlert = string.Format("alert('Objednávky byly přidruženy k objednávce č.{0}!');window.location='{1}';", parentOrder.OrderNumber, this.Request.RawUrl);
                btnAssociate.Page.ClientScript.RegisterStartupScript(btnAssociate.Page.GetType(), "OrderAssociation", jsAlert, true);
            } else Response.Redirect(this.Request.RawUrl);
        }

        protected void OnCreateOrder(object sender, EventArgs e) {
            List<OrderEntity> list = this.adminOrdersControl.GetSelectedOrdersToAssociation();
            if (list.Count == 0) {
                string js = "alert('Musíte označit aspoň jednu objednávku, která má být odeslána na zpracování!');;";
                btnCreateOrder.Page.ClientScript.RegisterStartupScript(btnCreateOrder.Page.GetType(), "OrderOrder", js, true);
                return;
            }
            foreach (OrderEntity order in list) {
                //Prepocitanie objednavky v TVD databazi
                if (CartOrderHelper.RecalculateTVDOrder(this.Page, null, order, true)) {
                    order.OrderStatusCode = ((int)OrderEntity.OrderStatus.InProccess).ToString();
                    Storage<OrderEntity>.Update(order);
                    //Objednavky pre pridruzenie k tejto objednavke
                    OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
                    filter.ParentId = order.Id;
                    List<OrderEntity> listChilds = Storage<OrderEntity>.Read(filter);
                    foreach (OrderEntity child in listChilds) {
                        child.ParentId = order.Id;
                        child.ShipmentCode = order.ShipmentCode;
                        child.OrderStatusCode = order.OrderStatusCode;
                        Storage<OrderEntity>.Update(child);
                    }
                } else
                    return;

                SendEmail(order);
            }

            Response.Redirect(this.Request.RawUrl);
        }

        private void SendEmail(OrderEntity order) {
            AccountEntity account = Security.Account;

            string root = CMS.Utilities.ServerUtilities.Root(this.Request);
            string urlOrder = String.Format("{0}{1}", root, GetOrdersUrl().Remove(0, 1));
            string urlCentral = String.Format("{0}eshop/admin/order.aspx?id={1}&ReturnUrl=/default.aspx", root, order.Id);
            string urlLogin = "http://www.euronabycerny.com/default.aspx?login";

            EshopEmailNotification email = new EshopEmailNotification();
            email.To = account.Email;
            email.Subject = string.Format(Resources.EShopStrings.UserOrderFinishPage_Email2User_Subject, order.OrderNumber);
            email.Message = String.Format(Resources.EShopStrings.UserOrderFinishPage_Email2User_Message, urlOrder, urlLogin).Replace("\\n", Environment.NewLine);
            email.Notify(true);

            // Notifi pouzivatelov pridruzenych objednavok
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
            foreach (OrderEntity o in list) {
                AccountEntity a = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = o.AccountId });
                if (string.IsNullOrEmpty(a.Email)) continue;

                email = new EshopEmailNotification();
                email.To = a.Email;
                email.Subject = string.Format(Resources.EShopStrings.UserOrderFinishPage_Email2User_Subject, o.OrderNumber);
                email.Message = String.Format(Resources.EShopStrings.UserOrderFinishPage_Email2User_Message, urlOrder, urlLogin).Replace("\\n", Environment.NewLine);
                email.Notify(true);
            }


            email = new EshopEmailNotification();
            email.To = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:SMTP:CentralInbox", this.Page);
            email.Subject = string.Format(Resources.EShopStrings.UserOrderFinishPage_Email2Central_Subject, order.OrderNumber);
            email.Message = String.Format(Resources.EShopStrings.UserOrderFinishPage_Email2Central_Message, urlCentral).Replace("\\n", Environment.NewLine);
            email.Notify(true);
        }
        protected string GetOrdersUrl() {
            const string virtualUrl = "~/user/advisor/orders.aspx?type=ar";

            AliasUtilities aliasUtils = new AliasUtilities();
            string alias = aliasUtils.Resolve(virtualUrl, this.Page);
            if (Request.ApplicationPath != "/" && alias.StartsWith(Request.ApplicationPath))
                alias = alias.Remove(0, Request.ApplicationPath.Length);

            return alias;
        }
    }
}
