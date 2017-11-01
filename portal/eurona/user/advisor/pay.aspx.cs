using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.EShop.User
{
	public partial class PayPage : WebPage
	{
		private OrderEntity order = null;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Request["id"]))
			{
				this.order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = Convert.ToInt32(Request["id"]) });

				this.adminOrderControl.OrderId = Convert.ToInt32(this.Request["id"]);
				if (this.adminOrderControl.OrderEntity == null) return;

				if (OrderEntity.GetOrderStatusFromCode(this.adminOrderControl.OrderEntity.OrderStatusCode) != OrderEntity.OrderStatus.WaitingForProccess &&
				!Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR))
					this.adminOrderControl.IsEditing = false;

				if (this.order.PaydDate.HasValue == false)
				{
					this.payOrderControl.OrderId = this.order.Id;
					this.payOrderControl.Visible = this.order.PaydDate.HasValue == false;
					this.divEPay.Visible = true;
				}
			}
		}
	}
}
