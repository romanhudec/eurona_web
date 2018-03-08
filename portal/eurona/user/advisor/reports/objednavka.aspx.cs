using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.User.Advisor.Reports {
    public partial class Objednavka : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(this.Request.Form["id"])) {
                //From POSTING form
                this.adminOrderControl.OrderId = Convert.ToInt32(this.Request.Form["id"]);
                if (this.adminOrderControl.OrderEntity == null) return;
                this.adminOrderControl.IsEditing = false;
            } else {
                if (!string.IsNullOrEmpty(this.Request["id"])) {

                    this.adminOrderControl.OrderId = Convert.ToInt32(this.Request["id"]);
                    if (this.adminOrderControl.OrderEntity == null) return;
                    this.adminOrderControl.IsEditing = false;
                } else {
                    if (!string.IsNullOrEmpty(this.Request["cislo"])) {
                        OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadByFilter { OrderNumber = this.Request["cislo"] });
                        if (order == null) return;

                        this.adminOrderControl.OrderId = order.Id;
                        if (this.adminOrderControl.OrderEntity == null) return;
                        this.adminOrderControl.IsEditing = false;
                    }
                }
            }
        }
    }
}