using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SHP.Entities.Classifiers;
using Eurona.Controls.Classifiers;

namespace Eurona.EShop.Admin.Classifier
{
		public partial class OrderStatuses: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						ClassifiersControl<OrderStatus> ctrl = new ClassifiersControl<OrderStatus>();
						ctrl.CssClass = "dataGrid";
						ctrl.NewUrl = "orderStatus.aspx";
						ctrl.EditUrlFormat = "orderStatus.aspx?Id={0}";
						this.divControls.Controls.Add( ctrl );
				}
		}
}
