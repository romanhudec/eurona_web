using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ImageGalleryItemCommentEntity = CMS.Entities.ImageGalleryItemComment;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace CMS.Controls.ImageGallery
{
		/// <summary>
		/// ImageGalleryComment control inherited from Comment control
		/// </summary>
		public class ImageGalleryItemCommentFormControl: Comment.CommentFormControl
		{
				public int ImageGalleryItemId
				{
						get { return Convert.ToInt32( ViewState["ImageGalleryItemId"] ); }
						set { ViewState["ImageGalleryItemId"] = value; }
				}

				public override void CreateComment( CMS.Entities.Comment comment )
				{
						ImageGalleryItemCommentEntity aComment = new ImageGalleryItemCommentEntity();
						aComment.AccountId = comment.AccountId;
						aComment.ParentId = comment.ParentId;
						aComment.ImageGalleryItemId = this.ImageGalleryItemId;
						aComment.Title = comment.Title;
						aComment.Content = comment.Content;
						Storage<ImageGalleryItemCommentEntity>.Create( aComment );
				}
		}
}
