using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Operator {
    public partial class Accounts : WebPage {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void btnNajitPoradce_Click(object sender, EventArgs e) {
            adminAccounts.FilterAdvisorCode = this.txtRegistracniCislo.Text;
            adminAccounts.FilterLogin = this.txtKonto.Text;
            adminAccounts.FilterEmail = this.txtEmail.Text;
            adminAccounts.GridViewDataBind(true);
        }

        protected void btnAddAcount_Click(object sender, EventArgs e) {
            adminAccounts.AddAccount();
        }
    }
}
