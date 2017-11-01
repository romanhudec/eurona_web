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
using System.Text;

namespace Eurona.User.Anonymous
{
	public partial class OrderPage : WebPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			#region js on click vytvaram objednavku
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("this.value = '{0}';", SHP.Resources.Controls.CartControl_OrderButton_Text);
			sb.Append("this.disabled = true;");
			sb.Append(Page.ClientScript.GetPostBackEventReference(btnContinue, null) + ";");

			string submit_button_onclick_js = sb.ToString();
			btnContinue.Attributes.Add("onclick", submit_button_onclick_js);
			#endregion

			if (!string.IsNullOrEmpty(this.Request["id"]))
			{
				this.adminOrderControl.OrderId = Convert.ToInt32(this.Request["id"]);
				if (this.adminOrderControl.OrderEntity == null) return;

				if (OrderEntity.GetOrderStatusFromCode(this.adminOrderControl.OrderEntity.OrderStatusCode) != OrderEntity.OrderStatus.WaitingForProccess &&
				!Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR))
					this.adminOrderControl.IsEditing = false;
			}
		}

		void cartControl_OnCartItemsChanged(object sender, EventArgs e)
		{
			(this.Page.Master as PageMasterPage).UpdateCartInfo();
		}

		protected void OnContinue(object sender, EventArgs e)
		{
            this.adminOrderControl.FinishUrlFormat = aliasUtilities.Resolve("~/user/anonymous/orderFinishPayments.aspx") + "?id={0}";
			this.adminOrderControl.OnOrder();
		}
	}
}