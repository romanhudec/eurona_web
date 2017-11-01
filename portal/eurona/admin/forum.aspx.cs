using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin
{
		public partial class Forum: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
								this.adminForumControl.ForumId = Convert.ToInt32( this.Request["id"] );
						if ( !string.IsNullOrEmpty( this.Request["threadId"] ) )
								this.adminForumControl.ForumThreadId = Convert.ToInt32( this.Request["threadId"] );
				}
		}
}
