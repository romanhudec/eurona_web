using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.EShop.User {
    public partial class NewOrderPage : WebPage {
        private OrderEntity order = null;
        protected void Page_Load(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(this.Request["id"])) {
                this.order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = Convert.ToInt32(Request["id"]) });
                this.repayOrderControl.OrderId = Convert.ToInt32(this.Request["id"]);

                this.payOrderControl.OrderId = this.order.Id;
                this.payOrderControl.Visible = this.order.PaydDate.HasValue == false;
                this.payOrderControl.HeaderControl.Visible = false;
                this.payOrderControl.PlatbaKarout4ZdruzeneObjednavkyMessageControl.Visible = false;
            }

        }
    }
}
