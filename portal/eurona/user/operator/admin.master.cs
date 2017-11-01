using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;

namespace Eurona.Operator
{
		public partial class AdminMasterPage: System.Web.UI.MasterPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						Security.IsLogged( true );

						if ( !Security.IsInRole( Role.OPERATOR ) ){
								Response.Redirect( "~/right.aspx" );
								return;
						}
				}
		}
}
