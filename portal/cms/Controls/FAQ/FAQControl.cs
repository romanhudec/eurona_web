using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using FAQEntity = CMS.Entities.FAQ;
using System.Web.UI;
using System.Data;
using System.ComponentModel;

namespace CMS.Controls.FAQ
{
    // do not show the StatsData class in the Toolbox...
    [ToolboxItem(false)]
    public class FAQData : WebControl, INamingContainer
    {
        private readonly FAQEntity faq;
        private readonly FAQControl owner;

        internal FAQData(string tag, FAQControl owner, FAQEntity faq)
            : base(tag)
        {
            this.owner = owner;
            this.faq = faq;
            this.CssClass = owner.CssClass + "_container";
        }

        [Bindable(true)]
        public int Id
        {
            get { return this.faq.Id; }
        }

        [Bindable(true)]
        public int Order
        {
            get { return this.faq.Order??0; }
        }

        [Bindable(true)]
        public string Question
        {
            get { return this.faq.Question; }
        }

        [Bindable(true)]
        public string Answer
        {
            get { return this.owner.FormatTextToView(this.faq.Answer); }
        }
    }

    public class FAQControl : CmsControl, INamingContainer
    {
        public int MaxItemsCount { get; set; }
        public string DisplayUrlFormat { get; set; }
        private string tag = "div";
        public string Tag { get { return tag; } set { tag = value; } }

        private ITemplate faqTemplate = null;

        public delegate string ContentProcessorHandler(FAQControl sender, string content);
        public event ContentProcessorHandler ContentProcessor;

        public string FormatTextToView(string content)
        {
            if (ContentProcessor != null) content = ContentProcessor(this, content);
            return content;
        }

        [Browsable(false)]
        [DefaultValue(null)]
        [Description("FAQ Item Template.")]
        [TemplateContainer(typeof(FAQData))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate FAQTemplate {
            get { return faqTemplate; }
            set { faqTemplate = value; }
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

            ITemplate template = this.FAQTemplate;
            if (template == null) template = new DefaultTemplate(this);

            List<FAQEntity> list = Storage<FAQEntity>.Read(null);
            if (list.Count == 0)
            {
                this.Visible = false;
                return;
            }

            foreach (FAQEntity faq in list)
            {
                FAQData data = new FAQData(tag, this, faq);
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

        internal sealed class DefaultTemplate : ITemplate
        {
            public string CssClass { get; set; }
            private string DisplayAliasUrlFormat { get; set; }
            public string DisplayUrlFormat { get; set; }
            private readonly FAQControl owner;

            public DefaultTemplate(FAQControl owner)
            {
                this.owner = owner;
                this.CssClass = owner.CssClass;
                this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
                this.DisplayUrlFormat = owner.DisplayUrlFormat;
            }

            void ITemplate.InstantiateIn(Control container)
            {
                Table table = new Table();
                table.CssClass = this.CssClass + "_faqItem";

                TableRow row = new TableRow();

                TableCell cell = new TableCell();
                Label lblQuestion = new Label();
                lblQuestion.DataBinding += (s1, e1) =>
                {
                    Label control = s1 as Label;
										FAQData r = (FAQData)control.NamingContainer;
                    control.Text = r.Question;
                };
                lblQuestion.CssClass = this.CssClass + "_question";
                cell.Controls.Add(lblQuestion);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                row = new TableRow();
                cell = new TableCell();
                Label lblAnswer = new Label();
                lblAnswer.CssClass = this.CssClass + "_answer";
                lblAnswer.DataBinding += (s1, e1) =>
                {
                    Label control = s1 as Label;
										FAQData r = (FAQData)control.NamingContainer;
										control.Text = r.Answer;
                };
                cell.Controls.Add(lblAnswer);
                row.Cells.Add(cell);
                table.Rows.Add(row);

                container.Controls.Add(table);

            }

        }

    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.UI;
//using FAQEntinty = CMS.Entities.FAQ;
//using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;
//using System.ComponentModel;

//namespace CMS.Controls.FAQ
//{
//    public class FAQControl : CmsControl
//    {
//        [ToolboxItem(false)]
//        public class FAQDataControl : WebControl, INamingContainer
//        {
//            private FAQEntinty faq;

//            internal FAQDataControl(FAQControl owner, FAQEntinty faq)
//                : base("div")
//            {
//                this.faq = faq;
//            }

//            [Bindable(true)]
//            public FAQEntinty DataItem
//            {
//                get { return this.faq; }
//            }
//        }

//        private GridView dataGrid = null;

//        public delegate string ContentProcessorHandler(FAQControl sender, string content);
//        public event ContentProcessorHandler ContentProcessor;

//        #region Protected overrides
//        protected override void CreateChildControls()
//        {
//            base.CreateChildControls();

//            this.dataGrid = CreateGridControl();
//            this.Controls.Add(this.dataGrid);

//            //Binding
//            this.dataGrid.PagerTemplate = null;
//            this.dataGrid.DataSource = GetDataGridData();

//            if (!IsPostBack)
//            {
//                this.dataGrid.DataKeyNames = new string[] { "Id" };
//                this.dataGrid.DataBind();
//            }

//        }
//        #endregion

//        private GridView CreateGridControl()
//        {
//            GridView dg = new GridView();
//            dg.EnableViewState = true;
//            dg.GridLines = GridLines.None;

//            dg.CssClass = CssClass;
//            dg.RowStyle.CssClass = CssClass + "_rowStyle";
//            dg.FooterStyle.CssClass = CssClass + "_footerStyle";
//            dg.PagerStyle.CssClass = CssClass + "_pagerStyle";
//            dg.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
//            dg.HeaderStyle.CssClass = CssClass + "_headerStyle";
//            dg.EditRowStyle.CssClass = CssClass + "_editRowStyle";
//            dg.AlternatingRowStyle.CssClass = CssClass + "_alternatingRowStyle";

//            dg.AllowPaging = true;
//            dg.PageSize = 5;
//            dg.PagerSettings.Mode = PagerButtons.NumericFirstLast;
//            dg.PagerSettings.PageButtonCount = 10;
//            dg.PageIndexChanging += OnPageIndexChanging;

//            dg.AutoGenerateColumns = false;

//            TemplateField tf = new TemplateField();
//            DefaultTemplate faqTemplate = new DefaultTemplate(this);
//            faqTemplate.CssClass = this.CssClass;

//            tf.ItemTemplate = faqTemplate;
//            tf.ItemStyle.Wrap = true;

//            /*
//            FAQDataControl data = new FAQDataControl(this, faq);
//            ITemplate template = this.NewsTemplate;
//            if (template == null) template = new DefaultTemplate(this, this.ArchivUrl);
//            template.InstantiateIn(data);
//            this.Controls.Add(data);
//            */

//            dg.Columns.Add(tf);


//            return dg;
//        }

//        private List<FAQEntinty> GetDataGridData()
//        {
//            List<FAQEntinty> list = Storage<FAQEntinty>.Read();
//            return list;
//        }

//        public delegate string ContentProcessorHandler(FAQControl sender, string content);
//        public event ContentProcessorHandler ContentProcessor;

//        public string FormatTextToView(string content)
//        {
//            if (ContentProcessor != null) content = ContentProcessor(this, content);
//            return content;
//        }

//        #region Event handlers
//        /// <summary>
//        /// Implementacia strankovania zaznamov v gride.
//        /// </summary>
//        void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
//        {
//            this.dataGrid.PageIndex = e.NewPageIndex;

//            this.dataGrid.DataSource = GetDataGridData();
//            this.dataGrid.DataKeyNames = new string[] { "Id" };
//            this.dataGrid.DataBind();
//        }

//        #endregion

//        [Browsable(false)]
//        [DefaultValue(null)]
//        [Description("FAQ item template.")]
//        [TemplateContainer(typeof(FAQDataControl))]
//        [PersistenceMode(PersistenceMode.InnerProperty)]
//        public virtual ITemplate FAQTemplate { get; set; }

//        /// <summary>
//        /// Template field na zobrazenie FAQ.
//        /// </summary>
//        private class DefaultTemplate : ITemplate
//        {
//            #region Public properties
//            public string CssClass { get; set; }
//            #endregion

//            private readonly FAQControl owner;

//            public DefaultTemplate(FAQControl owner)
//            {
//                this.owner = owner;
//            }

//            #region ITemplate Members

//            public void InstantiateIn(Control container)
//            {
//                Table tableArchiv = new Table();
//                tableArchiv.CssClass = this.CssClass + "_faqItem";

//                TableRow row = new TableRow();

//                TableCell cell = new TableCell();
//                Label lblQuestion = new Label();
//                lblQuestion.DataBinding += OnQuestionDataBinding;
//                lblQuestion.CssClass = this.CssClass + "_question";
//                cell.Controls.Add(lblQuestion);
//                row.Cells.Add(cell);
//                tableArchiv.Rows.Add(row);

//                row = new TableRow();
//                cell = new TableCell();
//                Label lblAnswer = new Label();
//                lblAnswer.CssClass = this.CssClass + "_answer";
//                lblAnswer.DataBinding += OnAnswerDataBinding;
//                cell.Controls.Add(lblAnswer);
//                row.Cells.Add(cell);
//                tableArchiv.Rows.Add(row);

//                container.Controls.Add(tableArchiv);

//            }
//            void OnQuestionDataBinding(object sender, EventArgs e)
//            {
//                Label control = sender as Label;
//                GridViewRow row = (GridViewRow)control.NamingContainer;

//                control.Text = (row.DataItem as FAQEntinty).Question;
//            }

//            void OnAnswerDataBinding(object sender, EventArgs e)
//            {
//                Label control = sender as Label;
//                GridViewRow row = (GridViewRow)control.NamingContainer;

//                control.Text = this.owner.FormatTextToView((row.DataItem as FAQEntinty).Answer);
//            }

//            #endregion
//        }
//    }
//}
