using System.Web.UI;
using System;
using System.Web;
using System.Text;
using System.Configuration;
using System.Web.UI.WebControls;
using CMS.Controls.Page;
using RoleEntity = CMS.Entities.Role;
using System.Web.UI.HtmlControls;


namespace CMS.Controls {
    public class ContentEditorCmsControl : CmsControl {
        public string CssEditorToolBar { get; set; }
        public string CssEditorContent { get; set; }
        public string NewUrl { get; set; }
        public string ManageUrl { get; set; }

        private HtmlGenericControl contentContainer = null;

        protected override void CreateChildControls() {
            base.CreateChildControls();

            if (!this.CanManageControl()) return;

            ContentEditorToolbarControl toolBar = CreateEditorToolBar();
            if (toolBar != null) {
                base.Controls.Add(toolBar);

                this.contentContainer = new HtmlGenericControl("div");
                this.contentContainer.Attributes.Add("class", this.CssEditorContent);
                base.Controls.Add(this.contentContainer);

                toolBar.Attributes.Add("onmouseover", "document.getElementById('" + toolBar.ClientID + "').nextSibling.className = '" + this.CssEditorContent + "_selected'");
                toolBar.Attributes.Add("onmouseout", "document.getElementById('" + toolBar.ClientID + "').nextSibling.className = '" + this.CssEditorContent + "'");
            }
        }

        /// <summary>
        /// Ak je mozne v tomto okamihu spravovat komponentu (upravit obsah,...)
        /// všetky child controls sa vytvárajú do content kontainera.
        /// </summary>
        public override ControlCollection Controls {
            get {
                if (this.contentContainer != null) return this.contentContainer.Controls;
                return base.Controls;
            }
        }

        /// <summary>
        /// Vrtáti true ak je možne v okamihu zobrazenie spravovat contentovy control.
        /// Teda či sa ma zobraziť v režime s editorom.
        /// </summary>
        public virtual bool CanManageControl() {
            string roleString = this.ConfigValue("CMS:ContentEditor:Roles");
            string[] roles = roleString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            if (roles == null || roles.Length == 0) return false;

            if (!Security.IsLogged(false))
                return false;
            if (Security.Account.Roles == null || Security.Account.Roles.Count == 0)
                return false;

            foreach (string role in roles)
                if (Security.IsInRole(role)) return true;

            return false;
        }

        /// <summary>
        /// Metóda vytvorí default toolBar pre spravu obsahu.
        /// </summary>
        public virtual ContentEditorToolbarControl CreateEditorToolBar() {
            //Editor toolbar
            ContentEditorToolbarControl editorToolBar = new ContentEditorToolbarControl();
            editorToolBar.ID = "pageEditToolbarControl";
            editorToolBar.CssClass = this.CssEditorToolBar;
            string navigateUrl = string.Empty;

            //Create toolbar items
            if (!string.IsNullOrEmpty(this.ManageUrl)) {
                string paramSeparator = this.ManageUrl.IndexOf("?") > 0 ? "&" : "?";
                navigateUrl = Page.ResolveUrl(this.ManageUrl) + paramSeparator + this.BuildReturnUrlQueryParam();
                ContentEditorToolbarButton btnManage = new ContentEditorToolbarButton(Resources.Controls.PageEditToolBar_ManageButtonText, this.ConfigValue("CMS:ManageButtonImage"), navigateUrl);
                editorToolBar.Items.Add(btnManage);
            }

            if (!string.IsNullOrEmpty(this.NewUrl)) {
                string paramSeparator = this.NewUrl.IndexOf("?") > 0 ? "&" : "?";
                navigateUrl = Page.ResolveUrl(this.NewUrl) + paramSeparator + this.BuildReturnUrlQueryParam();
                ContentEditorToolbarButton btnNew = new ContentEditorToolbarButton(Resources.Controls.PageEditToolBar_NewButtonText, this.ConfigValue("CMS:NewButtonImage"), navigateUrl);
                editorToolBar.Items.Add(btnNew);
            }

            if (editorToolBar.Items.Count == 0)
                return null;

            return editorToolBar;
        }
    }
}
