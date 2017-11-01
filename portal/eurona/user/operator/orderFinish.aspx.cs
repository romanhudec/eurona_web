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
using Eurona.Controls;


namespace Eurona.User.Operator
{
    public partial class OrderFinishPage : WebPage
    {
        private OrderEntity order = null;
        private const string virtualUrl = "~/user/operator/myorders.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            AliasUtilities aliasUtils = new AliasUtilities();
            string alias = aliasUtils.Resolve(virtualUrl, this.Page);

            this.order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = Convert.ToInt32(Request["id"]) });
            this.hlOrders.NavigateUrl = alias;

            if( this.order != null)
                CartOrderHelper.UhradaDobirkou(this.order);
        }
    }
}
