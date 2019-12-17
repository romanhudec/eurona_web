using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User.Advisor.Reports {
    public partial class ReportyReport : ReportPage {
        protected void Page_Load(object sender, EventArgs e) {
            string code = this.LogedAdvisor.Code;
        }


        public override object GetFilter() {
            return base.GetFilter();
        }
        public override void OnGenerateReport() {
            //object filter = GetFilter();
            base.OnGenerateReport();
        }
    }
}