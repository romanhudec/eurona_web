using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CommentEntity = CMS.Entities.Comment;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

[assembly: WebResource("CMS.Controls.Comment.CommentForm.js", "application/x-javascript", PerformSubstitution = true)]
namespace CMS.Controls.Comment
{
    public class CommentFormControl : CmsControl
    {
        private HiddenField hfParentId = null;
        private TextBox txtTitle = null;
        private TextBox txtContent = null;
        private Button btnSend = null;

        public CommentFormControl()
        {
        }

        /// <summary>
        /// Nastavi clanok, ktora sa ma zobrazit.
        /// </summary>
        public int? ParentId
        {
            get
            {
                object o = ViewState["ParentId"];
                return o != null ? (int?)Convert.ToInt32(o) : null;
            }
            set { ViewState["ParentId"] = value; }
        }

        public bool? ShowTitle { get; set; }
        public int? ContentHeight { get; set; }

        public string RedirectUrl { get; set; }

        public string HiddenFieldParenId { get; set; }

        #region Protected overrides
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", this.CssClass);
            div.Attributes.Add("id", this.ClientID);

            Control control = CreateDetailControl();
            if (control != null)
                div.Controls.Add(control);

            this.Controls.Add(div);

            this.hfParentId = new HiddenField();
            this.hfParentId.ID = this.HiddenFieldParenId;
            this.Controls.Add(this.hfParentId);

            div.Attributes.Add("HiddenFieldParenId", this.hfParentId.ClientID);

            ClientScriptManager cs = this.Page.ClientScript;
            Type cstype = this.GetType();
            string urlInclude = cs.GetWebResourceUrl(typeof(CommentFormControl), "CMS.Controls.Comment.CommentForm.js");
            cs.RegisterClientScriptInclude(cstype, "CommentFormJs", urlInclude);

        }
        #endregion

        /// <summary>
        /// Vytvori Control Clanku
        /// </summary>
        private Control CreateDetailControl()
        {
            if (!this.ShowTitle.HasValue) this.ShowTitle = true;
            if (!this.ContentHeight.HasValue) this.ContentHeight = 200;
            this.txtTitle = new TextBox();
            this.txtTitle.ID = "txtTitle";
            this.txtTitle.Width = Unit.Percentage(90);

            this.txtContent = new TextBox();
            this.txtContent.ID = "txtContent";
            this.txtContent.TextMode = TextBoxMode.MultiLine;
            this.txtContent.Width = Unit.Percentage(90);
            this.txtContent.Height = Unit.Pixel(this.ContentHeight.Value);

            this.btnSend = new Button();
            this.btnSend.CausesValidation = true;
            this.btnSend.Text = Resources.Controls.CommentControl_SendCommentButton_Text;
            this.btnSend.Click += new EventHandler(OnSendComment);

            Table table = new Table();
            //table.Attributes.Add( "border", "1" );
            table.Width = this.Width;
            table.Height = this.Height;

            table.Rows.Add(CreateTableRow(Resources.Controls.CommentControl_NewComment, false, this.CssClass + "_header"));
            if (this.ShowTitle.Value)
            {
                table.Rows.Add(CreateTableRow(Resources.Controls.CommentControl_Title, false, this.CssClass + "_title"));
                table.Rows.Add(CreateTableRow(this.txtTitle, false));
            }
            table.Rows.Add(CreateTableRow(Resources.Controls.CommentControl_Content, true, this.CssClass + "_content"));
            table.Rows.Add(CreateTableRow(this.txtContent, true));

            //Save Cancel Buttons
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.HorizontalAlign = HorizontalAlign.Left;
            cell.Controls.Add(this.btnSend);
            row.Cells.Add(cell);
            table.Rows.Add(row);

            return table;
        }

        private TableRow CreateTableRow(string labelText, bool required, string cssClas)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = cssClas;
            cell.Text = labelText;
            row.Cells.Add(cell);
            return row;
        }

        private TableRow CreateTableRow(Control control, bool required)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(control);
            cell.VerticalAlign = VerticalAlign.Top;
            if (required) cell.Controls.Add(base.CreateRequiredFieldValidatorControl(control.ID));
            row.Cells.Add(cell);

            return row;
        }

        private CommentEntity CreateEntityFromUI()
        {
            int? parentId = this.ParentId;
            if (!string.IsNullOrEmpty(this.hfParentId.Value))
                parentId = Convert.ToInt32(this.hfParentId.Value);

            CommentEntity comment = new CommentEntity();
            comment.AccountId = Security.Account.Id;
            comment.ParentId = parentId;
            if (this.ShowTitle.Value) comment.Title = this.txtTitle.Text;
            else comment.Title = string.Empty;
            comment.Content = this.txtContent.Text;
            return comment;
        }

        public virtual void CreateComment(CommentEntity comment)
        {
            Storage<CommentEntity>.Create(comment);
        }

        void OnSendComment(object sender, EventArgs e)
        {
            if (!Security.IsLogged(true))
                return;

            CommentEntity comment = CreateEntityFromUI();
            CreateComment(comment);

            if (string.IsNullOrEmpty(this.RedirectUrl))
                return;

            Response.Redirect(Page.ResolveUrl(this.RedirectUrl));
        }
    }
}
