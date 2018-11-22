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


namespace Eurona.User.Advisor {
    public partial class OrderFinishPage : WebPage {
        public OrderEntity order = null;
        private const string virtualUrl = "~/user/advisor/orders.aspx?type=ar";
        protected void Page_Load(object sender, EventArgs e) {
            string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            if (locale == "pl") this.imgLogo.ImageUrl = Page.ResolveUrl("~/images/order_finish_header_pl.png");
            else if (locale == "sk") this.imgLogo.ImageUrl = Page.ResolveUrl("~/images/order_finish_header_sk.png");

            AliasUtilities aliasUtils = new AliasUtilities();
            string alias = aliasUtils.Resolve(virtualUrl, this.Page);

            this.order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = Convert.ToInt32(Request["id"]) });

            this.hlOrders.NavigateUrl = alias;
        }

        protected string GetOrdersUrl() {
            AliasUtilities aliasUtils = new AliasUtilities();
            string alias = aliasUtils.Resolve(virtualUrl, this.Page);
            if (Request.ApplicationPath != "/" && alias.StartsWith(Request.ApplicationPath))
                alias = alias.Remove(0, Request.ApplicationPath.Length);

            return alias;
        }

    }
}
