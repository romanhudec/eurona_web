using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ArticleCommentEntity = CMS.Entities.ArticleComment;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace CMS.Controls.Article
{
		/// <summary>
		/// ArticleComment control inherited from Comment control
		/// </summary>
		public class ArticleCommentFormControl: Comment.CommentFormControl
		{
				public int ArticleId
				{
						get { return Convert.ToInt32( ViewState["ArticleId"] ); }
						set { ViewState["ArticleId"] = value; }
				}

				public override void CreateComment( CMS.Entities.Comment comment )
				{
						ArticleCommentEntity aComment = new CMS.Entities.ArticleComment();
						aComment.AccountId = comment.AccountId;
						aComment.ParentId = comment.ParentId;
						aComment.ArticleId = this.ArticleId;
						aComment.Title = comment.Title;
						aComment.Content = comment.Content;
						Storage<ArticleCommentEntity>.Create( aComment );
				}
		}
}
