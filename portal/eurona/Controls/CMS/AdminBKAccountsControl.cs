using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CMS.Controls;
using AccountEntity = Eurona.DAL.Entities.Account;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Eurona.Controls {
    public class AdminBKAccountsControl : CmsControl {
        private RadGrid gridView;
        private const string CREDIT_COMMAND = "CREDIT_ITEM";

        public AdminBKAccountsControl() {
        }

        public string IdentificationUrlFromat { get; set; }
        public string NewUrl { get; set; }
        public string EditUrlFormat { get; set; }
        public string AddCreditUrlFormat { get; set; }
        public string RolesUrlFormat { get; set; }

        public SortDirection SortDirection {
            get { return GetSession<SortDirection>("AdminBKAccountsControl-SortDirection", SortDirection.Ascending); }
            set { SetSession<SortDirection>("AdminBKAccountsControl-SortDirection", value); }
        }

        public string SortExpression {
            get { return GetSession<string>("AdminBKAccountsControl-SortExpression", "Login"); }
            set { SetSession<string>("AdminBKAccountsControl-SortExpression", value); }
        }

        public bool HideCredit { get; set; }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            gridView = CreateGridView();
            this.Controls.Add(gridView);

            GridViewDataBind(!IsPostBack);
        }

        private void GridViewDataBind(bool bind) {
            int instanceId = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId);

            string sql = @"SELECT Id=a.AccountId, a.*,
								BodyZaAktualniMesic = (SELECT SUM(Hodnota) FROM vBonusoveKredityUzivatele WHERE  YEAR(Datum) = YEAR(GETDATE()) AND MONTH(Datum)=MONTH(GETDATE()) AND AccountId=a.AccountId  )
								FROM vAccounts a
								WHERE InstanceId=@InstanceId";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            DataTable dt = null;
            CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = storage.Connect()) {
                dt = storage.Query(connection, sql,
                        new SqlParameter("@InstanceId", instanceId));
            }
            dt.Columns.Add("BodyKCerpani");

            foreach (DataRow row in dt.Rows) {
                int accountId = Convert.ToInt32(row["Id"]);
                if (row["TVD_Id"] == DBNull.Value) continue;
                int tvd_id = Convert.ToInt32(row["TVD_Id"]);
                decimal body = BonusovyKreditUzivateleHelper.GetKreditNarokCelkem(accountId, tvd_id, DateTime.Now.Year, DateTime.Now.Month);
                if (body != 0m) row["BodyKCerpani"] = body;
            }

            gridView.DataSource = dt;
            if (bind) gridView.DataBind();
        }

        private RadGrid CreateGridView() {
            RadGrid grid = new RadGrid();
            CMS.Utilities.RadGridUtilities.Localize(grid);
            grid.MasterTableView.DataKeyNames = new string[] { "Id" };

            grid.AllowAutomaticInserts = true;
            grid.AllowFilteringByColumn = true;
            grid.ShowStatusBar = false;
            grid.ShowGroupPanel = true;
            grid.GroupingEnabled = true;
            grid.GroupingSettings.ShowUnGroupButton = true;
            grid.GroupingSettings.CaseSensitive = false;
            grid.ClientSettings.AllowDragToGroup = true;
            grid.ClientSettings.AllowColumnsReorder = true;

            grid.MasterTableView.ShowHeader = true;
            grid.MasterTableView.ShowFooter = false;
            grid.MasterTableView.AllowPaging = true;
            grid.MasterTableView.PageSize = 25;
            grid.MasterTableView.PagerStyle.AlwaysVisible = true;
            grid.MasterTableView.AllowSorting = true;
            grid.MasterTableView.GridLines = GridLines.None;
            grid.MasterTableView.AutoGenerateColumns = false;

            grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;

            grid.Columns.Add(new GridBoundColumn {
                DataField = "Created",
                HeaderText = "Datum registrace",
                SortExpression = "Created",
                AutoPostBackOnFilter = true,
                DataFormatString = "{0:d}",
                CurrentFilterFunction = GridKnownFunction.Contains
            });

            if (string.IsNullOrEmpty(this.IdentificationUrlFromat)) {
                grid.Columns.Add(new GridBoundColumn {
                    DataField = "Login",
                    HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnLogin,
                    SortExpression = "Login",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains,
                });
            } else {
                grid.Columns.Add(new GridHyperLinkColumn {
                    DataTextField = "Login",
                    HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnLogin,
                    SortExpression = "Login",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains,
                    DataNavigateUrlFields = new string[] { "Id" },
                    DataNavigateUrlFormatString = Page.ResolveUrl(this.IdentificationUrlFromat + "&" + base.BuildReturnUrlQueryParam()),
                });
            }
            grid.Columns[0].HeaderStyle.Width = Unit.Pixel(80);

            grid.Columns.Add(new GridHyperLinkColumn {
                DataTextField = "Email",
                HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnEmail,
                SortExpression = "Email",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains,
                DataTextFormatString = "<a href=mailto:{0:g}>{0:g}</a>",
            });
            grid.Columns[1].HeaderStyle.Width = Unit.Pixel(120);

            grid.Columns.Add(new GridBoundColumn {
                DataField = "BodyZaAktualniMesic",
                HeaderText = "Body za aktualní měsíc",
                SortExpression = "BodyZaAktualniMesic",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains,
            });
            grid.Columns.Add(new GridBoundColumn {
                DataField = "BodyKCerpani",
                HeaderText = "Body k čerpání",
                SortExpression = "BodyKCerpani",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains,
            });
            if (!string.IsNullOrEmpty(this.AddCreditUrlFormat)) {
                GridButtonColumn btnAddCredit = new GridButtonColumn();
                btnAddCredit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
                btnAddCredit.ImageUrl = ConfigValue("CMS:EuroButtonImage");
                btnAddCredit.Text = "Ruční zadání bonusových kreditů uživateli";
                btnAddCredit.ButtonType = GridButtonColumnType.ImageButton;
                btnAddCredit.CommandName = CREDIT_COMMAND;
                grid.MasterTableView.Columns.Add(btnAddCredit);
            }

            grid.ItemCommand += OnRowCommand;

            return grid;
        }

        void OnRowCommand(object sender, GridCommandEventArgs e) {
            if (e.CommandName == CREDIT_COMMAND) OnAddCreditCommand(sender, e);
        }
        private void OnAddCreditCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int accountId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            Response.Redirect(String.Format(AddCreditUrlFormat, accountId) + "&" + base.BuildReturnUrlQueryParam());
        }
    }
}
