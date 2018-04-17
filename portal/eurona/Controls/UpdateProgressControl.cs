using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace Eurona.Controls {
    public class UpdateProgressControl : CMS.Controls.CmsControl {
        public string AssociatedUpdatePanelID { get; set; }
        protected override void CreateChildControls() {
            base.CreateChildControls();
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", this.CssClass);

            UpdateProgress upp = new UpdateProgress();
            upp.AssociatedUpdatePanelID = this.AssociatedUpdatePanelID;
            upp.DisplayAfter = 10;
            upp.DynamicLayout = false;

            Image progressImage = new Image();
            progressImage.ImageUrl = this.Page.ResolveUrl("~/images/ajax-indicator.gif");

            upp.Controls.Add(progressImage);

            div.Controls.Add(upp);
            this.Controls.Add(div);
        }
    }
}