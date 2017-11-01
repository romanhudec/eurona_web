using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.pay.test_post
{
		public partial class post: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{

				}

				protected void OnZaplatitKartou( object sender, EventArgs e )
				{
						PAY.CS.Transaction tran = new PAY.CS.Transaction();
						tran.Amount = "1000";
						tran.Var1 = "Zbozi: Walkman XYZ";
						string result = tran.PostTransaction( this );

						Response.Write( result );
						Response.End();
				}
		}
}