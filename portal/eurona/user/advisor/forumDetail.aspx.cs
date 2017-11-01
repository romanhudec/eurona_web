using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona
{
		public partial class ForumDetailPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( Request["Id"] ) )
								this.forumDetailControl.ForumId = Convert.ToInt32( Request["Id"] );
						if ( !string.IsNullOrEmpty( this.Request["threadId"] ) )
								this.forumDetailControl.ForumThreadId = Convert.ToInt32( this.Request["threadId"] );
				}
		}
}
