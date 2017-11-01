using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin
{
		public partial class ForumPost: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
								this.adminForumPostControl.ForumPostId = Convert.ToInt32( this.Request["id"] );
						if ( !string.IsNullOrEmpty( this.Request["forumId"] ) )
								this.adminForumPostControl.ForumId = Convert.ToInt32( this.Request["forumId"] );
				}
		}
}
