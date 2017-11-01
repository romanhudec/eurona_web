using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using System.Web.UI.HtmlControls;

namespace CMS.Controls.RadEditor {
    public class RadEditor : CompositeControl, INamingContainer {
        private Telerik.Web.UI.RadEditor editor = null;

        public RadEditor() {
            this.editor = new Telerik.Web.UI.RadEditor();
            this.editor.ID = "radEditor";
            this.editor.Style.Clear();
            this.editor.Style.Add("width", "auto!important");

        }

        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            EnsureChildControls();
        }

        public new string ID {
            set { this.editor.ID = value; }
        }
        public Telerik.Web.UI.RadEditor Editor {
            get { return this.editor; }
        }

        public string Content {
            get { return this.Editor.Content; }
            set { this.Editor.Content = value; }
        }

        public string Text {
            get { return this.Editor.Text; }
            set { this.Editor.Text = value; }
        }

        public string ToolsFile { get; set; }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            this.editor.EnableEmbeddedSkins = true;
            this.editor.EnableResize = false;
            this.editor.ToolbarMode = Telerik.Web.UI.EditorToolbarMode.Default;

            //Nastavenie tools toolbarov
            if (string.IsNullOrEmpty(this.ToolsFile)) {
                string toolsFile = CMS.Utilities.ConfigUtilities.ConfigValue("RadEditor:ToolsFile", this.Page);
                if (!string.IsNullOrEmpty(toolsFile)) this.editor.ToolsFile = toolsFile;
            } else this.editor.ToolsFile = this.ToolsFile;

            string tmpI = CMS.Utilities.ConfigUtilities.ConfigValue("RadEditor:MaxUploadImageSize", this.Page);
            string tmpM = CMS.Utilities.ConfigUtilities.ConfigValue("RadEditor:MaxUploadMediaSize", this.Page);
            string tmpD = CMS.Utilities.ConfigUtilities.ConfigValue("RadEditor:MaxUploadDocumentSize", this.Page);
            string tmpF = CMS.Utilities.ConfigUtilities.ConfigValue("RadEditor:MaxUploadFlashSize", this.Page);
            int maxUploadImageSize = 204800;//Default max 200KB
            Int32.TryParse(tmpI, out maxUploadImageSize);
            int maxUploadMediaSize = 5242880;//Default max 5MB
            Int32.TryParse(tmpM, out maxUploadMediaSize);
            int maxUploadDocumentSize = 5242880;//Default max 5MB
            Int32.TryParse(tmpD, out maxUploadDocumentSize);
            int maxUploadFlashSize = 5242880;//Default max 5MB
            Int32.TryParse(tmpF, out maxUploadFlashSize);

            //Nastavenie image manager-a
            string ufp = CMS.Utilities.ConfigUtilities.ConfigValue("RadEditor:UserFilesPath", this.Page);
            this.editor.ImageManager.ViewPaths = new string[] { ufp };
            this.editor.ImageManager.DeletePaths = new string[] { ufp };
            this.editor.ImageManager.UploadPaths = new string[] { ufp };
            this.editor.ImageManager.MaxUploadFileSize = maxUploadImageSize;

            this.editor.DocumentManager.ViewPaths = new string[] { ufp };
            this.editor.DocumentManager.DeletePaths = new string[] { ufp };
            this.editor.DocumentManager.UploadPaths = new string[] { ufp };
            this.editor.DocumentManager.MaxUploadFileSize = maxUploadDocumentSize;

            this.editor.MediaManager.ViewPaths = new string[] { ufp };
            this.editor.MediaManager.DeletePaths = new string[] { ufp };
            this.editor.MediaManager.UploadPaths = new string[] { ufp };
            this.editor.MediaManager.MaxUploadFileSize = maxUploadMediaSize;

            this.editor.FlashManager.ViewPaths = new string[] { ufp };
            this.editor.FlashManager.DeletePaths = new string[] { ufp };
            this.editor.FlashManager.UploadPaths = new string[] { ufp };
            this.editor.FlashManager.MaxUploadFileSize = maxUploadFlashSize;

            this.editor.AllowScripts = true;
            this.editor.DocumentManager.SearchPatterns = new string[] { "*.jpg", "*.png", "*.txt", "*.pdf", "*.doc", "*.docx", "*.xls", "*.xlsx", "*.rtf", "*.*" };

            this.Controls.Add(this.editor);
        }
    }
}
