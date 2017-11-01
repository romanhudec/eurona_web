using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UzavierkaEntity = Eurona.Common.DAL.Entities.Uzavierka;

namespace Eurona.user.advisor {
    public partial class AdvisorCookieLinkControl : Eurona.Common.Controls.UserControl {
        public string CssClass { get; set; }
        protected override void CreateChildControls() {
            base.CreateChildControls();

            this.divContainer.Attributes.Add("class", this.CssClass);
            this.divContainer.Style.Add("margin-bottom", "10px");
            
            string link = Utilities.Root(Request) + String.Format("?{0}={1}", 
                CookiesUtils.COOKIE_SINGLEUSERCOOKIESLINK_QUERY_PARAMETER,
                CMS.Utilities.Cryptographer.Encrypt(Security.Account.Id.ToString()));
            this.lblAdvisorCookieLink.Text = link;
        }
    }
}