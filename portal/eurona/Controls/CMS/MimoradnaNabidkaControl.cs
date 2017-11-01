using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MimoradnaNabidkaEntity = Eurona.DAL.Entities.MimoradnaNabidka;
using CMS.Controls;

namespace Eurona.Controls
{
    public class MimoradnaNabidkaControl : CmsControl
    {
        [ToolboxItem(false)]
        public class MimoradnaNabidkaDataControl : WebControl, INamingContainer
        {
            private MimoradnaNabidkaEntity entity;

            internal MimoradnaNabidkaDataControl(MimoradnaNabidkaControl owner, MimoradnaNabidkaEntity entity)
                : base("div")
            {
                this.entity = entity;
            }

            [Bindable(true)]
            public MimoradnaNabidkaEntity DataItem
            {
                get { return this.entity; }
            }
        }

        public MimoradnaNabidkaControl()
        {
        }

        [Browsable(false)]
        [DefaultValue(null)]
        [Description("Article template.")]
        [TemplateContainer(typeof(MimoradnaNabidkaDataControl))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate MimoradnaNabidkaTemplate { get; set; }

        public string ArchivUrl { get; set; }

        /// <summary>
        /// Nastavi novinku, ktora sa ma zobrazit.
        /// </summary>
        public int MimoradnaNabidkaId
        {
            get { return Convert.ToInt32(ViewState["MimoradnaNabidkaId"]); }
            set { ViewState["MimoradnaNabidkaId"] = value; }
        }

        #region Protected overrides
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            MimoradnaNabidkaEntity entity = Storage<MimoradnaNabidkaEntity>.ReadFirst(new MimoradnaNabidkaEntity.ReadById() { MimoradnaNabidkaId = this.MimoradnaNabidkaId });
            if (entity == null)
                return;

            //Select template
            MimoradnaNabidkaDataControl data = new MimoradnaNabidkaDataControl(this, entity);
            ITemplate template = this.MimoradnaNabidkaTemplate;
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

            public DefaultTemplate(MimoradnaNabidkaControl owner, string archivUrl)
            {
                if (!string.IsNullOrEmpty(archivUrl))
                {
                    CMS.Utilities.AliasUtilities aliasUtils = new CMS.Utilities.AliasUtilities();
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
                MimoradnaNabidkaEntity entity = (container as MimoradnaNabidkaDataControl).DataItem;

                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Attributes.Add("class", this.CssClass);

                //Header
                Table table = new Table();
                table.Width = Unit.Percentage(100);
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.CssClass = this.CssClass + "_title";
                cell.Text = entity.Title;
                row.Cells.Add(cell);
                table.Rows.Add(row);

                //Content
                row = new TableRow();
                cell = new TableCell();
                cell.CssClass = this.CssClass + "_content";
                cell.Controls.Add(new LiteralControl(entity.Content));
                row.Cells.Add(cell);
                table.Rows.Add(row);

                div.Controls.Add(table);

                return div;
            }
        }
    }
}
