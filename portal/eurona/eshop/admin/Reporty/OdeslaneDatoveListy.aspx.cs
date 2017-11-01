using Eurona.Common.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.EShop.Admin.Reporty {
    public partial class OdeslaneDatoveListy : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            this.gridView.DataSource = Storage<DokumentProduktuEmail>.Read();
            this.gridView.DataBind();
        }

        protected void btnFilter_Click(object sender, EventArgs e) {
            DateTime? dateFrom = (DateTime?)this.dtpDatumOd.Value;
            if (dateFrom != null) {
                dateFrom = new DateTime(dateFrom.Value.Year, dateFrom.Value.Month, dateFrom.Value.Day, 0, 0, 0);
            }
            DateTime? dateTo = (DateTime?)this.dtpDatumDo.Value;
            if (dateTo != null) {
                dateTo = new DateTime(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, 23, 59, 59);
            }
            this.gridView.DataSource = Storage<DokumentProduktuEmail>.Read(new DokumentProduktuEmail.ReadByDate { DateFrom = dateFrom, DateTo = dateTo });
            this.gridView.DataBind();
        }
    }
}
