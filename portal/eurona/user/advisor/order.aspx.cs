using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.EShop.User {
    public partial class OrderPage : WebPage {
        //private OrderEntity order = null;
        protected void Page_Load(object sender, EventArgs e) {

            if (Request.Browser.MSDomVersion.Major == 0) // Non IE Browser?
            {
                Response.Cache.SetNoStore(); // No client side cashing for non IE browsers
            }
            if (!IsPostBack) {
                Response.Buffer = true;
                Response.CacheControl = "no-cache";
                Response.AddHeader("Pragma", "no-cache");
                Response.AppendHeader("Cache-Control", "no-store");
                Response.Expires = -1441;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();
            }

            if (!string.IsNullOrEmpty(this.Request["id"])) {
                //this.order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = Convert.ToInt32(Request["id"]) });

                this.adminOrderControl.OrderId = Convert.ToInt32(this.Request["id"]);
                if (this.adminOrderControl.OrderEntity == null) return;

                if (OrderEntity.GetOrderStatusFromCode(this.adminOrderControl.OrderEntity.OrderStatusCode) != OrderEntity.OrderStatus.WaitingForProccess &&
                !Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR))
                    this.adminOrderControl.IsEditing = false;
            }

        }
    }
}
