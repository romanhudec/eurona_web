using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using Eurona.Common.Controls.Cart;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using OrderEntity = Eurona.DAL.Entities.Order;
using CartEntity = Eurona.Common.DAL.Entities.Cart;
using AccountEntity = CMS.Entities.Account;
using EshopEmailNotification = SHP.EmailNotification;
using System.Text;
using Eurona.DAL.Entities;

namespace Eurona.User.Anonymous {
    public partial class FinishPage : WebPage {
        protected string locale = "cs";
        public OrderEntity order = null;
        private const string virtualUrl = "~/user/advisor/orders.aspx?type=ar";
        protected void Page_Load(object sender, EventArgs e) {
            Security.IsLogged(true);
            locale = Security.Account.Locale;//System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            this.linkOrder.HRef = Page.ResolveUrl(string.Format("~/user/advisor/order.aspx?id={0}", Request["id"]));
            this.divPoradce.Visible = false;

            Security.IsLogged(true);

            Organization org = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = Security.Account.Id });
            if (org == null) return;

            if (org.ParentId.HasValue) {
                Organization parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByTVDId { TVD_Id = org.ParentId.Value });
                if (parentOrg != null && parentOrg.Code != Organization.EURONA_CODE) {
                    this.divPoradce.Visible = true;
                    this.lblEmail.Text = parentOrg.Account.Email;
                    this.lblMobil.Text = parentOrg.ContactMobile;
                    this.lblName.Text = parentOrg.Name;
                }
            }

            Security.Account.RemoveFromRole(Eurona.Common.DAL.Entities.Role.ANONYMOUSADVISOR.ToString());
            Storage<Account>.Update(Security.Account);

            this.order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = Convert.ToInt32(Request["id"]) });

            if (!IsPostBack) SendEmail(order);

        }


        protected string GetOrdersUrl() {
            CMS.Utilities.AliasUtilities aliasUtils = new CMS.Utilities.AliasUtilities();
            string alias = aliasUtils.Resolve(virtualUrl, this.Page);
            if (Request.ApplicationPath != "/" && alias.StartsWith(Request.ApplicationPath))
                alias = alias.Remove(0, Request.ApplicationPath.Length);

            return alias;
        }

        private void SendEmail(OrderEntity order) {
            /*
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
            filter.ParentId = this.order.Id;
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
           */
        }
    }
}