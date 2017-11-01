using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.Operator
{
    public partial class OrderPage : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Browser.MSDomVersion.Major == 0) // Non IE Browser?
            {
                Response.Cache.SetNoStore(); // No client side cashing for non IE browsers
            }

            if (!string.IsNullOrEmpty(this.Request["id"]))
            {
                this.adminOrderControl.OrderId = Convert.ToInt32(this.Request["id"]);
                //this.adminOrderControl.FinishUrlFormat = this.adminOrderControl.ReturnUrl;
                if (OrderEntity.GetOrderStatusFromCode(this.adminOrderControl.OrderEntity.OrderStatusCode) != OrderEntity.OrderStatus.WaitingForProccess &&
                !Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR))
                    this.adminOrderControl.IsEditing = false;
            }
        }
    }
}
