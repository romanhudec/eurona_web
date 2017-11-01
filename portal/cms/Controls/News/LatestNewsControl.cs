using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using NewsEntity = CMS.Entities.News;
using System.Web.UI;
using System.Data;
using System.ComponentModel;

namespace CMS.Controls.News
{
    // do not show the StatsData class in the Toolbox...
    [ToolboxItem(false)]
    public class TopNewsData : WebControl, INamingContainer
    {
        private NewsEntity news;

        internal TopNewsData(string tag, LatestNewsControl owner, NewsEntity news)
            : base(tag)
        {
            this.news = news;
            this.CssClass = owner.CssClass + "_container";
        }

        [Bindable(true)]
        public int Id
        {
            get { return this.news.Id; }
        }

        [Bindable(true)]
        public DateTime? Date
        {
            get { return this.news.Date; }
        }

        [Bindable(true)]
        public string Title
        {
            get { return this.news.Title; }
        }

        [Bindable(true)]
        public string Teaser
        {
            get { return this.news.Teaser; }
        }

        [Bindable(true)]
        public string Alias
        {
            get { return this.news.Alias; }
        }
    }

    public class LatestNewsControl : CmsControl, INamingContainer
    {
        public int MaxItemsCount { get; set; }
        public string DisplayUrlFormat { get; set; }
        private string tag = "div";
        public string Tag { get { return tag; } set { tag = value; } }

        private ITemplate topNewsTemplate = null;

        [Browsable(false)]
        [DefaultValue(null)]
        [Description("The top-news template.")]
        [TemplateContainer(typeof(TopNewsData))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate TopNewsTemplate
        {
            get
            {
                return topNewsTemplate;
            }
            set
            {
                topNewsTemplate = value;
            }
        }

        // For composite controls, the Controls collection needs to be overriden to
        // ensure that the child controls have been created before the Controls
        // property can be modified by the page developer...
        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();		// clear out the control hierarchy

            ITemplate template = this.TopNewsTemplate;
            if (template == null) template = new DefaultTopNewsTemplate(this);

            List<NewsEntity> list = Storage<NewsEntity>.Read(new NewsEntity.ReadLatest { Count = MaxItemsCount <= 0 ? 3 : MaxItemsCount });
            if (list.Count == 0)
            {
                this.Visible = false;
                return;
            }

            foreach (NewsEntity news in list)
            {
                TopNewsData data = new TopNewsData(tag, this, news);
                template.InstantiateIn(data);
                Controls.Add(data);
            }

            this.DataBind();
        }

        public override void DataBind()
        {
            this.ChildControlsCreated = true;
            base.DataBind();
        }

        internal sealed class DefaultTopNewsTemplate : ITemplate
        {
            public string CssClass { get; set; }
            private string DisplayAliasUrlFormat { get; set; }
            public string DisplayUrlFormat { get; set; }

            public DefaultTopNewsTemplate(LatestNewsControl owner)
            {
                this.CssClass = owner.CssClass;
                this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
                this.DisplayUrlFormat = owner.DisplayUrlFormat;
            }

            void ITemplate.InstantiateIn(Control container)
            {
                Table table = new Table();
                table.CssClass = this.CssClass + "_news";

                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Text = "» ";
                row.Cells.Add(cell);

                cell = new TableCell();
                HyperLink hlHead = new HyperLink();
                hlHead.DataBinding += (sender, e) =>
                {
                    TopNewsData data = (TopNewsData)hlHead.NamingContainer;
                    HyperLink control = sender as HyperLink;
                    control.Text = data.Title;

                    if (string.IsNullOrEmpty(data.Alias) && !string.IsNullOrEmpty(this.DisplayUrlFormat))
                        control.NavigateUrl = control.Page.ResolveUrl(string.Format(this.DisplayUrlFormat, data.Id));
                    else control.NavigateUrl = control.Page.ResolveUrl(string.Format(this.DisplayAliasUrlFormat, data.Alias));

                };
                hlHead.CssClass = this.CssClass + "_head";
                cell.Controls.Add(hlHead);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                container.Controls.Add(table);
            }

        }

    }
}
