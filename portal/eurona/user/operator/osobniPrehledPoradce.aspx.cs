using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;

namespace Eurona.User.Operator
{
		public partial class OsobniPrehledPoradceReport: Page
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( string.IsNullOrEmpty( Request["id"] ) ) return;
						
						int accountId = Convert.ToInt32( Request["id"] );
						OrganizationEntity forAdvisor = Storage<OrganizationEntity>.ReadFirst( new OrganizationEntity.ReadByAccountId { AccountId = accountId } );
						if ( !forAdvisor.TVD_Id.HasValue ) return;

						osobniPrehledPoradce.ForAdvisor = forAdvisor;
						GridViewDataBind( true );
				}

				private void GridViewDataBind( bool bind )
				{
						osobniPrehledPoradce.Obdobi = osobniPrehledPoradce.CurrentObdobiRRRRMM;
						osobniPrehledPoradce.GridViewDataBind( bind );
				}
		}
}