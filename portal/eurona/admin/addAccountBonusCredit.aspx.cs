using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin {
    public partial class AddAccountBonusCredit : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            Response.Write("Tato funkce již není podporována! Bonusové kredity se spravují v EUROSAP!");
            Response.End();
            return;

            //this.accountBKAdminControl.OnKreditChanged += new EventHandler(OnKreditChanged);
            //this.bonusoveKredityUzivateleInfoControl.AccountId = Convert.ToInt32(Request["id"]);
            //this.bonusoveKredityUzivateleInfoControl.DataBind();
        }

        protected void OnKreditChanged(object sender, EventArgs e) {
            this.bonusoveKredityUzivateleInfoControl.ReloadControlData();
        }
    }
}
