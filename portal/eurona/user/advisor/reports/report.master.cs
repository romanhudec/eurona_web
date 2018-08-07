using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Eurona.User.Advisor.Reports
{
    public partial class ReportMasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblTitle.Text = this.Page.Title;
            if (!IsPostBack)
            {
                this.txtObdobi.Text = string.Format("{0}{1:#00}", DateTime.Now.Year, DateTime.Now.Month);
                if (!string.IsNullOrEmpty(Request["obdobi"]))
                    this.txtObdobi.Text = Request["obdobi"];
            }

            if (this.ForAdvisor == null) return;
            this.lblAdvisorInfo.Text = string.Format("{0} {1}", this.ForAdvisor.Name, this.ForAdvisor.Code);


            if (!string.IsNullOrEmpty(Request["id"])) OnGenearte(this, null);
        }

        public bool DisableObdobi
        {
            get { return !this.txtObdobi.Enabled; }
            set { this.txtObdobi.Enabled = !value; }
        }

        public bool HideObdobi
        {
            get { return !this.txtObdobi.Visible; }
            set { 
                this.txtObdobi.Visible = !value;
                this.lblObdobi.Visible = this.txtObdobi.Visible;
            }
        }

        /// <summary>
        /// Report pre poradcu
        /// </summary>
        public OrganizationEntity ForAdvisor
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
                    OrganizationEntity byAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByTVDId { TVD_Id = Convert.ToInt32(Request["id"]) });
                    if (byAdvisor == null) return null;

                    CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
                    using (SqlConnection connection = tvdStorage.Connect())
                    {
                        //string sql = @"SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele) WHERE Id_Odberatele=@childId";
                        string sql = @"SELECT Id_Odberatele FROM fGetOdberateleStrom(@Id_odberatele, DATEPART(YEAR, GETDATE())*100 +  DATEPART(MONTH, GETDATE())) WHERE Id_Odberatele=@childId";
                        DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@Id_odberatele", this.LogedAdvisor.TVD_Id), new SqlParameter("@childId", byAdvisor.TVD_Id));
                        if (dt.Rows.Count == 0) return null;
                        else return byAdvisor;
                    }
                }
                return this.LogedAdvisor;
            }
        }
        /// <summary>
        /// Prihlaseny poradca
        /// </summary>
        public OrganizationEntity LogedAdvisor
        {
            get
            {
                return (this.Master as User.Advisor.Reports.PageMasterPage).LogedAdvisor;
            }
        }

        /// <summary>
        /// Vrati aktualny filter
        /// </summary>
        public object GetFilter()
        {
            int obdobi = Convert.ToInt32(string.Format("{0}{1:#00}", DateTime.Now.Year, DateTime.Now.Month));
            Int32.TryParse(this.txtObdobi.Text, out obdobi);
            return obdobi;
        }

        protected void OnGenearte(object sender, EventArgs e)
        {
            if (this.Page is ReportPage)
            {
                ReportPage reportPage = (this.Page as ReportPage);
                reportPage.OnGenerateReport();
            }
        }

        protected void OnExport(object sender, EventArgs e)
        {
            if (this.Page is Reports.ReportPage)
            {
                ReportPage rp = (this.Page as Reports.ReportPage);
                RadGrid grid = rp.GetGridView();
                if (grid == null) return;

                grid.ExportSettings.IgnorePaging = true;
                grid.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
                grid.ExportSettings.ExportOnlyData = true;
                grid.MasterTableView.ExportToExcel();
            }
        }
    }
}