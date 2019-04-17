using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;
using AccountEntity = CMS.Entities.Account;
using EshopEmailNotification = SHP.EmailNotification;
using CMS;
using System.Configuration;
using CMS.Utilities;
using System.Net.Mail;
using System.IO;


namespace Eurona.User.Anonymous {
    public partial class OrderFinishPaymentPage : WebPage {
        public OrderEntity order = null;
        private const string virtualUrl = "~/user/advisor/orders.aspx?type=ar";
        protected void Page_Load(object sender, EventArgs e) {
            Security.Account.RemoveFromRole(Eurona.Common.DAL.Entities.Role.ANONYMOUSADVISOR.ToString());
            Storage<AccountEntity>.Update(Security.Account);

            this.order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = Convert.ToInt32(Request["id"]) });
            this.payOrderControl.OrderId = this.order.Id;
            this.payOrderControl.Visible = this.order.PaydDate.HasValue == false;

            if (!IsPostBack) SendEmail(this.order);
        }

        protected string GetOrdersUrl() {
            AliasUtilities aliasUtils = new AliasUtilities();
            string alias = aliasUtils.Resolve(virtualUrl, this.Page);
            if (Request.ApplicationPath != "/" && alias.StartsWith(Request.ApplicationPath))
                alias = alias.Remove(0, Request.ApplicationPath.Length);

            return alias;
        }

        private void SendEmail(OrderEntity order) {
            AccountEntity account = Security.Account;

            string root = CMS.Utilities.ServerUtilities.Root(this.Request);
            string urlOrder = String.Format("{0}{1}", root, GetOrdersUrl().Remove(0, 1));
            string urlCentral = String.Format("{0}eshop/admin/order.aspx?id={1}&ReturnUrl=/default.aspx", root, order.Id);
            string urlLogin = "http://www.euronabycerny.com/default.aspx?login";
            string mailAttachment = "~/userfiles/OBCHODNÍ PODMÍNKY CZ.pdf";

            Attachment attachment = new Attachment(Server.MapPath(mailAttachment));

            EshopEmailNotification email = new EshopEmailNotification();
            email.To = account.Email;
            email.Subject = string.Format(Resources.EShopStrings.UserOrderFinishPage_Email2User_Subject, order.OrderNumber);
            email.Message = String.Format(Resources.EShopStrings.UserOrderFinishPage_Email2User_Message, urlOrder, urlLogin).Replace("\\n", Environment.NewLine);
            email.Attachments = new List<System.Net.Mail.Attachment>();
            email.Attachments.Add(attachment);
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
                email.Attachments = new List<System.Net.Mail.Attachment>();
                email.Attachments.Add(attachment);
                email.Notify(true);
            }

            /*
            email = new EshopEmailNotification();
            email.To = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:SMTP:CentralInbox", this.Page);
            email.Subject = string.Format(Resources.EShopStrings.UserOrderFinishPage_Email2Central_Subject, order.OrderNumber);
            email.Message = String.Format(Resources.EShopStrings.UserOrderFinishPage_Email2Central_Message, urlCentral).Replace("\\n", Environment.NewLine);
            email.Notify(true);
*/
        }
    }
}
