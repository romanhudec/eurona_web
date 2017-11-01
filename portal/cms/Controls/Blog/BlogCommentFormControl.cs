using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using BlogCommentEntity = CMS.Entities.BlogComment;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace CMS.Controls.Blog
{
		/// <summary>
		/// BlogComment control inherited from Comment control
		/// </summary>
		public class BlogCommentFormControl: Comment.CommentFormControl
		{
				public int BlogId
				{
						get { return Convert.ToInt32( ViewState["BlogId"] ); }
						set { ViewState["BlogId"] = value; }
				}

				public override void CreateComment( CMS.Entities.Comment comment )
				{
						BlogCommentEntity aComment = new CMS.Entities.BlogComment();
						aComment.AccountId = comment.AccountId;
						aComment.ParentId = comment.ParentId;
						aComment.BlogId = this.BlogId;
						aComment.Title = comment.Title;
						aComment.Content = comment.Content;
						Storage<BlogCommentEntity>.Create( aComment );
				}
		}
}
