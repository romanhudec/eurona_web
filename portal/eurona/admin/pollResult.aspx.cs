using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin
{
		public partial class PollResult: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
								this.pollControl.PollId = Convert.ToInt16( this.Request["id"] );
				}
		}
}
