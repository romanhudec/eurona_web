using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;
using Eurona.Common.DAL.Entities;
using Telerik.Web.UI;

namespace Eurona.User.Advisor
{
		public partial class NewAdvisorsPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						this.newAdvisorsControl.ParentId = ( this.Page.Master as PageMasterPage ).LogedAdvisor.TVD_Id;
				}
		}
}
