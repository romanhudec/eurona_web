﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;

namespace Eurona.User.Advisor.Reports {
    public partial class OsobniPrehledPoradceReport : ReportPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (this.ForAdvisor == null) return;
            if (this.ForAdvisor.TVD_Id == null) return;

            //Ak nie je vierihodny - len seba
            if (this.ForAdvisor.RestrictedAccess == 1)
                this.txtAdvisorCode.Enabled = false;

            if (!IsPostBack)
                this.txtAdvisorCode.Text = this.ForAdvisor.Code;

            osobniPrehledPoradce.ForAdvisor = this.ForAdvisor;
            GridViewDataBind(!IsPostBack);
        }

        public override object GetFilter() {
            return base.GetFilter();
        }

        public override void OnGenerateReport() {
            if (this.ForAdvisor == null) return;

            if (!string.IsNullOrEmpty(this.txtAdvisorCode.Text)) {
                if (this.txtAdvisorCode.Text != this.ForAdvisor.Code) {
                    int? obdobi = null;
                    object filter = GetFilter();
                    if (filter != null) obdobi = (int)filter;

                    OrganizationEntity forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = this.txtAdvisorCode.Text });
                    if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/osobniPrehledPoradce.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
                    else {
                        forAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadBy { Name = this.txtAdvisorCode.Text });
                        if (forAdvisor != null) Response.Redirect(Page.ResolveUrl(string.Format("~/user/advisor/reports/osobniPrehledPoradce.aspx?id={0}&obdobi={1}", forAdvisor.TVD_Id, obdobi)));
                    }
                }
            }

            GridViewDataBind(true);

        }

        private void GridViewDataBind(bool bind) {
            int? obdobi = null;
            object filter = GetFilter();
            if (filter != null) obdobi = (int)filter;
            if (obdobi.HasValue == false || obdobi == 0) return;

            osobniPrehledPoradce.Obdobi = obdobi;
            osobniPrehledPoradce.GridViewDataBind(bind);
        }
    }
}