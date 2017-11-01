using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using NewsEntity = CMS.Entities.News;
using CMS.Utilities;

namespace CMS.Controls.News
{
    public class NewsControl : CmsControl
    {
        [ToolboxItem(false)]
        public class NewsDataControl : WebControl, INamingContainer
        {
            private NewsEntity news;

            internal NewsDataControl(NewsControl owner, NewsEntity news)
                : base("div")
            {
                this.news = news;
            }

            [Bindable(true)]
            public NewsEntity DataItem
            {
                get { return this.news; }
            }
        }

        public NewsControl()
        {
        }

        [Browsable(false)]
        [DefaultValue(null)]
        [Description("Article template.")]
        [TemplateContainer(typeof(NewsDataControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate NewsTemplate { get; set; }

        public string ArchivUrl { get; set; }

        /// <summary>
        /// Nastavi novinku, ktora sa ma zobrazit.
        /// </summary>
        public int NewsId
        {
            get { return Convert.ToInt32(ViewState["NewsId"]); }
            set { ViewState["NewsId"] = value; }
        }

        #region Protected overrides
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            NewsEntity news = Storage<NewsEntity>.ReadFirst(new NewsEntity.ReadById() { NewsId = this.NewsId });
            if (news == null)
                return;

            //Select template
            NewsDataControl data = new NewsDataControl(this, news);
            ITemplate template = this.NewsTemplate;
            if (template == null) template = new DefaultTemplate(this, this.ArchivUrl);
            template.InstantiateIn(data);
            this.Controls.Add(data);

            this.DataBind();

        }
        public override void DataBind()
        {
            this.ChildControlsCreated = true;
            base.DataBind();
        }
        #endregion

        private class DefaultTemplate : ITemplate
        {
            #region Public properties
            public string CssClass { get; set; }
            private string archivUrl;
            #endregion

            #region ITemplate Members

            public DefaultTemplate(NewsControl owner, string archivUrl)
            {
                if (!string.IsNullOrEmpty(archivUrl))
                {
                    AliasUtilities aliasUtils = new AliasUtilities();
                    this.archivUrl = aliasUtils.Resolve(archivUrl, owner.Page);
                }

                this.CssClass = owner.CssClass;
            }

            public void InstantiateIn(Control container)
            {
                Control control = CreateDetailControl(container);
                if (control != null)
                    container.Controls.Add(control);
            }
            #endregion

            /// <summary>
            /// Vytvori Control Clanok
            /// </summary>
            private Control CreateDetailControl(Control container)
            {
                NewsEntity news = (container as NewsDataControl).DataItem;

                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes.Add("class", this.CssClass);

                //Header
                Table table = new Table();
                table.Width = Unit.Percentage(100);
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.CssClass = this.CssClass + "_title";
                cell.Text = news.Title;
                row.Cells.Add(cell);
                table.Rows.Add(row);

                //Content
                row = new TableRow();
                cell = new TableCell();
                cell.CssClass = this.CssClass + "_content";
                cell.Controls.Add(new LiteralControl(news.Content));
                row.Cells.Add(cell);
                table.Rows.Add(row);

                //Archiv link
                row = new TableRow();
                cell = new TableCell();
                cell.CssClass = this.CssClass + "_archivLink";
                HyperLink link = new HyperLink();
                link.NavigateUrl = container.ResolveUrl(archivUrl);
                link.Text = Resources.Controls.NewsControl_NewsArchivLink;
                cell.Controls.Add(link);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                div.Controls.Add(table);

                return div;
            }
        }
    }
}
