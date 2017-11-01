using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ImageGalleryCommentEntity = CMS.Entities.ImageGalleryComment;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace CMS.Controls.ImageGallery
{
		/// <summary>
		/// ImageGalleryComment control inherited from Comment control
		/// </summary>
		public class ImageGalleryCommentFormControl: Comment.CommentFormControl
		{
				public int ImageGalleryId
				{
						get { return Convert.ToInt32( ViewState["ImageGalleryId"] ); }
						set { ViewState["ImageGalleryId"] = value; }
				}

				public override void CreateComment( CMS.Entities.Comment comment )
				{
						ImageGalleryCommentEntity aComment = new CMS.Entities.ImageGalleryComment();
						aComment.AccountId = comment.AccountId;
						aComment.ParentId = comment.ParentId;
						aComment.ImageGalleryId = this.ImageGalleryId;
						aComment.Title = comment.Title;
						aComment.Content = comment.Content;
						Storage<ImageGalleryCommentEntity>.Create( aComment );
				}
		}
}
