using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.User.Advisor.Reports
{
		public partial class Objednavka: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
						{
								this.adminOrderControl.OrderId = Convert.ToInt32( this.Request["id"] );
								if ( this.adminOrderControl.OrderEntity == null ) return;

								if ( OrderEntity.GetOrderStatusFromCode( this.adminOrderControl.OrderEntity.OrderStatusCode ) != OrderEntity.OrderStatus.WaitingForProccess &&
								!Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) )
										this.adminOrderControl.IsEditing = false;
						}
				}
		}
}