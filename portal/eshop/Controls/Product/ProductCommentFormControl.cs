using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ProductCommentEntity = SHP.Entities.ProductComment;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace SHP.Controls.Product
{
    /// <summary>
    /// ProductComment control inherited from Comment control
    /// </summary>
    public class ProductCommentFormControl : CMS.Controls.Comment.CommentFormControl
    {
        public delegate void CommentCreatedEventHandler(ProductCommentEntity comment);
        public event CommentCreatedEventHandler OnCommentCreated;
        public int ProductId
        {
            get { return Convert.ToInt32(ViewState["ProductId"]); }
            set { ViewState["ProductId"] = value; }
        }

        public override void CreateComment(CMS.Entities.Comment comment)
        {
            ProductCommentEntity aComment = new ProductCommentEntity();
            aComment.AccountId = comment.AccountId;
            aComment.ParentId = comment.ParentId;
            aComment.ProductId = this.ProductId;
            aComment.Title = comment.Title;
            aComment.Content = comment.Content;
            Storage<ProductCommentEntity>.Create(aComment);
            if (OnCommentCreated != null)
                OnCommentCreated(aComment);
        }
    }
}
