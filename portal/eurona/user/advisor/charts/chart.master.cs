using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;

namespace Eurona.User.Advisor.Charts
{
		public partial class ChartMasterPage: System.Web.UI.MasterPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						this.lblTitle.Text = this.Page.Title;
						this.lblAdvisorInfo.Text = string.Format( "{0} {1}", this.LogedAdvisor.Name, this.LogedAdvisor.Code );
				}

				public OrganizationEntity LogedAdvisor
				{
						get
						{
								return ( this.Master as User.Advisor.PageMasterPage ).LogedAdvisor;
						}
				}
		}
}