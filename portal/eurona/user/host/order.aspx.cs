using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using OrderEntity = SHP.Entities.Order;

namespace Eurona.User.Host
{
		public partial class OrderPage: System.Web.UI.Page
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
						{
								this.adminOrderControl.OrderId = Convert.ToInt32( this.Request["id"] );
								if ( this.adminOrderControl.OrderEntity.OrderStatusId.Value != (int)OrderEntity.OrderStatus.WaitingForProccess &&
								!Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) )
										this.adminOrderControl.IsEditing = false;
						}
				}
		}
}
