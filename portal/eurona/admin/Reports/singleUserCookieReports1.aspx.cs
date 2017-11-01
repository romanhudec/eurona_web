using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Admin.Controls;
using CMS.Entities.Classifiers;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Eurona.Admin.Reports {
    public partial class SingleUserCookieReports1Page : WebPage {
        private int obdobi = 0;
        protected void Page_Load(object sender, EventArgs e) {
            Int32.TryParse(this.txtObdobi.Text, out obdobi);

            if (!IsPostBack) {
                this.txtObdobi.Text = this.CurrentObdobiRRRRMM.ToString();
            }
            this.GridViewDataBind(!IsPostBack);
        }
        public int CurrentObdobiRRRRMM {
            get {
                int year = DateTime.Now.Year * 100;
                return year + DateTime.Now.Month;
            }
        }

        private void GridViewDataBind(bool bind) {
            if( obdobi == 0 ) obdobi = this.CurrentObdobiRRRRMM;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {

                string sql = @"WITH Report (Obdobi, CookieAccountId, Pocet)
                AS
                (
                    SELECT Obdobi=DATEPART(YEAR, sucla.Timestamp)*100+DATEPART(MONTH, sucla.Timestamp), sucla.CookieAccountId, Pocet=COUNT(sucla.Id)
                    FROM tSingleUserCookieLinkActivity sucla
                    GROUP BY DATEPART(YEAR, Timestamp)*100+DATEPART(MONTH, Timestamp), CookieAccountId
                )
                SELECT r.*, a.Login, a.Email, Name=o.Name, RegistrationCode=o.Code from Report r
                INNER JOIN vAccounts a ON r.CookieAccountId=a.AccountId
                INNER JOIN vOrganizations o ON r.CookieAccountId=o.AccountId
                WHERE r.Obdobi=@Obdobi";
                  DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@Obdobi", obdobi));

                this.gridView.DataSource = dt;
            }

            if (bind) gridView.DataBind();
        }

        protected void OnGenearte(object sender, EventArgs e) {
            GridViewDataBind(true);
        }
    }
}
