using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin
{
		public partial class PollOption: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
								this.adminOptionControl.PollOptionId = Convert.ToInt16( this.Request["id"] );
						if ( !string.IsNullOrEmpty( this.Request["pollId"] ) )
								this.adminOptionControl.PollId = Convert.ToInt16( this.Request["pollId"] );
				}
		}
}
