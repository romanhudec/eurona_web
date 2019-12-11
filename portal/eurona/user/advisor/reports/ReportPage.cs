using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using System.Configuration;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Data;

namespace Eurona.User.Advisor.Reports {
    public class ReportPage : WebPage {
        public OrganizationEntity ForAdvisor {
            get {
                return (this.Page.Master as User.Advisor.Reports.ReportMasterPage).ForAdvisor;
            }
        }
        public OrganizationEntity LogedAdvisor {
            get {
                return (this.Page.Master as User.Advisor.Reports.ReportMasterPage).LogedAdvisor;
            }
        }


        /// <summary>
        /// Vrati aktualny filter
        /// </summary>
        public virtual object GetFilter() {
            return (this.Page.Master as User.Advisor.Reports.ReportMasterPage).GetFilter();
        }

        /// <summary>
        /// Aktualne obdobie RRRRMM
        /// </summary>
        public int CurrentObdobiRRRRMM {
            get {
                int year = DateTime.Now.Year * 100;
                return year + DateTime.Now.Month;
            }
        }
        public bool DisableObdobi {
            get { return (this.Page.Master as User.Advisor.Reports.ReportMasterPage).DisableObdobi; }
            set { (this.Page.Master as User.Advisor.Reports.ReportMasterPage).DisableObdobi = value; }
        }

        public bool HideObdobi {
            get { return (this.Page.Master as User.Advisor.Reports.ReportMasterPage).HideObdobi; }
            set { (this.Page.Master as User.Advisor.Reports.ReportMasterPage).HideObdobi = value; }
        }

        public virtual void OnGenerateReport() { }
        public virtual RadGrid GetGridView() { return null; }

        /// <summary>
        /// Vrati connection string do TVD
        /// </summary>
        public string ConnectionString {
            get {
                return ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            }
        }

        public static int? GetCurrentObdobiFromTVD() {
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;

            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = string.Empty;
                sql = @"SELECT TOP 1 RRRRMM FROM provize_aktualni";
                //Clear data
                DataTable dt = tvdStorage.Query(connection, sql);
                if (dt == null) return null;
                if (dt.Rows.Count == 0) return null;
                object objectValue = dt.Rows[0]["RRRRMM"];
                if (objectValue == DBNull.Value) return null;
                return Convert.ToInt32(objectValue);

            }
        }
    }
}