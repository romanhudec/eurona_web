using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using PageEntity = Eurona.DAL.Entities.AdvisorPage;
using CMS.Controls.RadEditor;
using CMS.Utilities;

namespace Eurona.Admin
{
    public partial class AdvisorContentEditorPage : WebPage
    {
        protected CMSEditor editor;
        private PageEntity page;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (string.IsNullOrEmpty(this.Request["id"])) return;
            this.PageId = Convert.ToInt32(this.Request["id"]);

            if (this.PageEntity == null)
                return;

            this.editor = new CMSEditor();
            this.editor.Editor.Style.Clear();
            this.editor.Editor.Attributes.Clear();
            this.editor.Editor.Width = Unit.Pixel(100);//Min Width
            this.editor.Editor.Height = Unit.Pixel(100);//Min Height
            this.editor.Content = this.PageEntity.Content;

            this.editor.Editor.OnClientLoad = "editorOnClientLoad";
            this.editorContainer.Controls.Add(this.editor);
        }

        private PageEntity PageEntity
        {
            get
            {
                if (page != null) return page;
                page = Storage<PageEntity>.ReadFirst(new PageEntity.ReadById { AdvisorPageId = this.PageId });
                return page;
            }
        }

        /// <summary>
        /// ID stránky
        /// </summary>
        public int PageId { get; set; }

        protected void OnSave(object sender, EventArgs e)
        {
            this.PageEntity.Content = editor.Content;
            this.PageEntity.ContentKeywords = StringUtilities.RemoveDiacritics(editor.Text);
            Storage<PageEntity>.Update(this.PageEntity);

            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "aaa", "closeRadWindow('save');", true);
        }

    }
}
