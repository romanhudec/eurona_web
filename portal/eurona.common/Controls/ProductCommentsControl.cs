using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ProductCommentEntity = SHP.Entities.ProductComment;
using CommentEntity = CMS.Entities.Comment;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using CMS.Controls;
using CMS;

namespace Eurona.Common.Controls.Product {
    // do not show the StatsData class in the Toolbox...
    [ToolboxItem(false)]
    public class ProductCommentsData : WebControl, INamingContainer {
        private ProductCommentEntity comment;

        internal ProductCommentsData(ProductCommentsControl owner, ProductCommentEntity comment)
            : base("div") {
            this.comment = comment;
            this.CssClass = owner.CssClass + "_container";
            this.Childs = new List<ProductCommentsData>();
        }

        [Bindable(true)]
        public int Id {
            get { return this.comment.Id; }
        }

        [Bindable(true)]
        public int CommentId {
            get { return this.comment.CommentId; }
        }

        [Bindable(true)]
        public int? ParentId {
            get { return this.comment.ParentId; }
        }

        [Bindable(true)]
        public int AccountId {
            get { return this.comment.AccountId; }
        }

        [Bindable(true)]
        public DateTime Date {
            get { return this.comment.Date; }
        }

        [Bindable(true)]
        public string AccountName {
            get { return this.comment.AccountName; }
        }

        [Bindable(true)]
        public string Title {
            get { return this.comment.Title; }
        }

        [Bindable(true)]
        public string Content {
            get { return this.comment.Content; }
        }

        [Bindable(true)]
        public double RatingResult {
            get { return this.comment.RatingResult; }
        }

        [Bindable(true)]
        public List<ProductCommentsData> Childs { get; set; }

    }

    public class ProductCommentsControl : CmsControl {
        public int MaxItemsCount { get; set; }
        public string DisplayUrlFormat { get; set; }
        public string CssCarma { get; set; }

        public string CommentFormID {
            get { return ViewState["CommentFormID"] == null ? string.Empty : ViewState["CommentFormID"].ToString(); }
            set { ViewState["CommentFormID"] = value; }
        }

        public int ProductId {
            get { return Convert.ToInt32(ViewState["ProductId"]); }
            set { ViewState["ProductId"] = value; }
        }

        [Browsable(false)]
        [DefaultValue(null)]
        [Description("The comment template.")]
        [TemplateContainer(typeof(ProductCommentsData))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate ProductCommentsTemplate { get; set; }

        // For composite controls, the Controls collection needs to be overriden to
        // ensure that the child controls have been created before the Controls
        // property can be modified by the page developer...
        public override ControlCollection Controls {
            get {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        private List<ProductCommentsData> BuildTreeData(List<ProductCommentEntity> entityList) {
            List<ProductCommentsData> list = new List<ProductCommentsData>();
            foreach (ProductCommentEntity entity in entityList) {
                if (entity.ParentId != null) continue;

                ProductCommentsData data = new ProductCommentsData(this, entity);
                data.Childs.AddRange(GetChilds(data.CommentId, entityList));
                list.Add(data);
            }

            return list;
        }

        private List<ProductCommentsData> GetChilds(int parentId, List<ProductCommentEntity> entityList) {
            List<ProductCommentsData> list = new List<ProductCommentsData>();
            foreach (ProductCommentEntity entity in entityList) {
                if (entity.ParentId != parentId) continue;

                ProductCommentsData data = new ProductCommentsData(this, entity);
                data.Childs.AddRange(GetChilds(data.CommentId, entityList));
                list.Add(data);
            }

            return list;
        }

        public override void RenderControl(HtmlTextWriter writer) {
            writer.Write(string.Format("<div id='{0}'>", this.ClientID));
            base.RenderControl(writer);
            writer.Write("</div>");
        }
        protected override void CreateChildControls() {
            ProductEntity product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = this.ProductId });
            if (product == null)
                return;

            //Nadpis clanku
            HtmlGenericControl divProductTitle = new HtmlGenericControl("div");
            divProductTitle.Attributes.Add("class", this.CssClass + "_title");
            HyperLink hlArtisleTitle = new HyperLink();
            hlArtisleTitle.Text = product.Name;
            string returnUrl = this.ReturnUrl;
            if (!string.IsNullOrEmpty(returnUrl))
                hlArtisleTitle.NavigateUrl = Page.ResolveUrl(returnUrl);
            else if (!string.IsNullOrEmpty(product.Alias)) hlArtisleTitle.NavigateUrl = Page.ResolveUrl(product.Alias);
            else if (!string.IsNullOrEmpty(this.DisplayUrlFormat))
                hlArtisleTitle.NavigateUrl = string.Format(Page.ResolveUrl(this.DisplayUrlFormat), this.ProductId);
            divProductTitle.Controls.Add(hlArtisleTitle);
            this.Controls.Add(divProductTitle);

            //BR
            this.Controls.Add(new LiteralControl("<br/>"));

            HtmlGenericControl divReply = new HtmlGenericControl("div");
            HyperLink hlReply = new HyperLink();
            hlReply.CssClass = this.CssClass + "_item_reply";
            hlReply.Text = global::SHP.Resources.Controls.ProductCommentsControl_Reply;
            hlReply.DataBinding += (sender, e) => {
                HtmlGenericControl control = sender as HtmlGenericControl;
                hlReply.Attributes.Add("onclick", string.Format("showReplyForm('{0}', '{1}', null )",
                        this.CommentFormID,
                        this.ClientID));

            };
            divReply.Controls.Add(hlReply);
            this.Controls.Add(divReply);

            //BR
            this.Controls.Add(new LiteralControl("<br/>"));

            HtmlGenericControl txtContent = new HtmlGenericControl("div");
            txtContent.ID = "txtContent";
            txtContent.Style.Add("overflow", "scroll");
            txtContent.Style.Add("border", "solid 1px #DDDDDD");
            txtContent.InnerHtml = product.Description + "<br/>" + product.DescriptionLong;
            txtContent.Style.Add("width", "100%");
            txtContent.Style.Add("height", "100px");
            this.Controls.Add(txtContent);

            //BR
            this.Controls.Add(new LiteralControl("<br/>"));

            //Template list
            ITemplate template = this.ProductCommentsTemplate;
            if (template == null) template = new DefaultProductCommentsTemplate(this);

            List<ProductCommentEntity> list = Storage<ProductCommentEntity>.Read(new ProductCommentEntity.ReadByProductId { ProductId = this.ProductId });
            List<ProductCommentsData> dataSource = BuildTreeData(list);

            foreach (ProductCommentsData data in dataSource) {
                template.InstantiateIn(data);
                Controls.Add(data);
            }

            this.DataBind();
        }


        public override void DataBind() {
            this.ChildControlsCreated = true;
            base.DataBind();
        }

        internal sealed class DefaultProductCommentsTemplate : ITemplate {
            public string CssClass { get; set; }
            public ProductCommentsControl Owner { get; set; }
            public string CommentFormID { get; set; }

            public DefaultProductCommentsTemplate(ProductCommentsControl owner) {
                this.Owner = owner;
                this.CssClass = owner.CssClass;
                this.CommentFormID = owner.CommentFormID;
            }

            void ITemplate.InstantiateIn(Control container) {
                HtmlGenericControl divReplyFormContainer = new HtmlGenericControl("div");

                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes.Add("class", this.CssClass + "_item");

                Table table = new Table();
                //table.Attributes.Add( "border", "1" );

                //Title
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell = new TableCell();
                cell.VerticalAlign = VerticalAlign.Top;
                cell.CssClass = this.CssClass + "_item_title";
                Label lblTitle = new Label();
                lblTitle.DataBinding += (sender, e) => {
                    ProductCommentsData data = (ProductCommentsData)lblTitle.NamingContainer;
                    Label control = sender as Label;
                    control.Text = data.Title;
                };
                cell.Controls.Add(lblTitle);
                row.Cells.Add(cell);

                //Carma
                cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.VerticalAlign = VerticalAlign.Top;
                cell.CssClass = this.CssClass + "_item_carma";
                CMS.Controls.Vote.CarmaControl carmaControl = new CMS.Controls.Vote.CarmaControl();
                carmaControl.CssClass = this.Owner.CssCarma;
                carmaControl.OnVote += (objectId, rating) => {
                    ProductCommentEntity.IncrementVoteCommand cmd = new ProductCommentEntity.IncrementVoteCommand();
                    cmd.AccountId = Security.Account.Id;
                    cmd.CommentId = objectId;
                    cmd.Rating = rating;
                    Storage<ProductCommentEntity>.Execute(cmd);

                    ProductCommentEntity comment = Storage<ProductCommentEntity>.ReadFirst(new ProductCommentEntity.ReadByCommentId { CommentId = objectId });
                    return comment.RatingResult;
                };

                carmaControl.DataBinding += (sender, e) => {
                    ProductCommentsData data = (ProductCommentsData)lblTitle.NamingContainer;
                    CMS.Controls.Vote.CarmaControl control = sender as CMS.Controls.Vote.CarmaControl;
                    control.ObjectId = data.CommentId;
                    control.ObjectTypeId = (int)ProductCommentEntity.AccountVoteType;
                    control.RatingResult = data.RatingResult;
                };
                cell.Controls.Add(carmaControl);
                row.Cells.Add(cell);

                table.Rows.Add(row);

                //Content
                row = new TableRow();
                cell = new TableCell();
                cell.ColumnSpan = 2;
                Label lblContent = new Label();
                lblContent.CssClass = this.CssClass + "_item_content";
                lblContent.DataBinding += (sender, e) => {
                    ProductCommentsData data = (ProductCommentsData)lblContent.NamingContainer;
                    Label control = sender as Label;
                    control.Text = data.Content;
                };
                cell.Controls.Add(lblContent);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                #region Footer
                //Reply button
                row = new TableRow();
                cell = new TableCell();
                HyperLink hlReply = new HyperLink();
                hlReply.CssClass = this.CssClass + "_item_reply";
                hlReply.Text = global::SHP.Resources.Controls.ProductCommentsControl_Reply;
                hlReply.DataBinding += (sender, e) => {
                    ProductCommentsData data = (ProductCommentsData)hlReply.NamingContainer;
                    HtmlGenericControl control = sender as HtmlGenericControl;
                    divReplyFormContainer.Attributes.Add("id", string.Format("formContainer_{0}", data.CommentId));
                    hlReply.Attributes.Add("onclick", string.Format("showReplyForm('{0}', '{1}', {2} )",
                            this.CommentFormID,
                            divReplyFormContainer.ClientID,
                            data.CommentId));

                };
                cell.Controls.Add(hlReply);
                row.Cells.Add(cell);

                //User name
                cell = new TableCell();
                cell.ColumnSpan = 1;
                cell.HorizontalAlign = HorizontalAlign.Right;
                Label lblUserName = new Label();
                lblUserName.CssClass = this.CssClass + "_item_user";
                lblUserName.DataBinding += (sender, e) => {
                    ProductCommentsData data = (ProductCommentsData)lblUserName.NamingContainer;
                    Label control = sender as Label;
                    control.Text = data.AccountName;
                };
                cell.Controls.Add(lblUserName);

                cell.Controls.Add(new LiteralControl("|"));

                //DateTime
                Label lblDateTime = new Label();
                lblDateTime.CssClass = this.CssClass + "_item_date";
                lblDateTime.DataBinding += (sender, e) => {
                    ProductCommentsData data = (ProductCommentsData)lblDateTime.NamingContainer;
                    Label control = sender as Label;
                    control.Text = data.Date.ToString();
                };
                cell.Controls.Add(lblDateTime);
                row.Cells.Add(cell);

                //Nevhodny prispevok
                bool showInappropriatePost = false;
                if (Security.IsInRole(CMS.Entities.Role.ADMINISTRATOR) || Security.IsInRole(Eurona.Common.DAL.Entities.Role.OPERATOR)) {
                    cell.Controls.Add(new LiteralControl("|"));
                    LinkButton btnInappropriatePost = new LinkButton();
                    btnInappropriatePost.Text = "Smazat";//SHP.Resources.Controls.ProductCommentsControl_InappropriatePost;
                    btnInappropriatePost.CausesValidation = false;
                    btnInappropriatePost.CssClass = this.CssClass + "_item_deletecomment";
                    btnInappropriatePost.DataBinding += (sender, e) => {
                        ProductCommentsData data = (ProductCommentsData)btnInappropriatePost.NamingContainer;
                        LinkButton control = sender as LinkButton;
                        control.CommandArgument = data.Id.ToString();
                    };
                    btnInappropriatePost.Click += (s, e) => {
                        ProductCommentsData data = (ProductCommentsData)lblTitle.NamingContainer;
                        LinkButton control = s as LinkButton;
                        int id = Convert.ToInt32(control.CommandArgument);
                        ProductCommentEntity pce = Storage<ProductCommentEntity>.ReadFirst(new ProductCommentEntity.ReadById { ProductCommentId = id });
                        if (pce != null) {
                            CommentEntity ce = Storage<CommentEntity>.ReadFirst(new CommentEntity.ReadById { CommentId = pce.CommentId });
                            if (ce != null) {
                                /*
                                lblContent.Text = SHP.Resources.Controls.ProductCommentsControl_InappropriatePost;
                                ce.Content = SHP.Resources.Controls.ProductCommentsControl_InappropriatePost;
                                Storage<CommentEntity>.Update( ce );
                                 */
                                Storage<CommentEntity>.Delete(ce);
                                this.Owner.Page.Response.Redirect(cell.Page.Request.RawUrl);
                            }
                        }
                    };
                    cell.Controls.Add(btnInappropriatePost);
                }

                table.Rows.Add(row);
                #endregion

                //Reply Form container
                row = new TableRow();
                cell = new TableCell();
                cell.ColumnSpan = showInappropriatePost ? 3 : 2;
                cell.Controls.Add(divReplyFormContainer);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                //Childs
                row = new TableRow();
                cell = new TableCell();
                cell.ColumnSpan = showInappropriatePost ? 3 : 2;
                HtmlGenericControl divChilds = new HtmlGenericControl("div");
                divChilds.Attributes.Add("class", this.CssClass + "_item_children");
                divChilds.DataBinding += (sender, e) => {
                    ProductCommentsData data = (ProductCommentsData)lblContent.NamingContainer;
                    foreach (ProductCommentsData child in data.Childs) {
                        ITemplate template = new DefaultProductCommentsTemplate(this.Owner);
                        template.InstantiateIn(child);
                        divChilds.Controls.Add(child);
                    }
                };
                cell.Controls.Add(divChilds);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                div.Controls.Add(table);
                container.Controls.Add(div);
            }
        }

    }
}
