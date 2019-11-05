using Eurona.Common.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin {
    public partial class NastaveniZavozovehoMista : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            this.gridView.DataSource = this.ZavozoveMistoList;
            if (!Page.IsPostBack) {
                this.gridView.DataBind();
            }
        }

        protected void OnRowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName.ToUpper() == "DELETE_ITEM") {
                int id = Convert.ToInt32(e.CommandArgument);
                ZavozoveMisto zm = Storage<ZavozoveMisto>.ReadFirst(new ZavozoveMisto.ReadById { Id = id });
                if (zm == null) return;

                Storage<ZavozoveMisto>.Delete(zm);
            }

            if (e.CommandName.ToUpper() == "EDIT_ITEM") {
                int id = Convert.ToInt32(e.CommandArgument);
                Response.Redirect(string.Format("~/eshop/admin/zavozoveMistoEdit.aspx?id={0}", id));
            }

            this.zavozoveMistoList = null;
            this.gridView.DataSource = this.ZavozoveMistoList;
            this.gridView.DataBind();
        }

        private List<ZavozoveMisto> zavozoveMistoList = null;
        public List<ZavozoveMisto> ZavozoveMistoList {
            get {
                if (this.zavozoveMistoList != null) return this.zavozoveMistoList;
                this.zavozoveMistoList = Storage<ZavozoveMisto>.Read();
                return this.zavozoveMistoList;
            }
        }

        private DateTime SetTimeToDateFromString(DateTime date, string timeText) {
            if (string.IsNullOrEmpty(this.txtCas.Text)) return date;

            string[] time = timeText.Split(':');
            if (time.Length < 2) return date;

            int hour = Convert.ToInt32(time[0]);
            int minute = Convert.ToInt32(time[1]);
            date = date.AddHours(hour);
            date = date.AddMinutes(minute);
            return date;
        }

        protected void OnAdd(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(this.txtMesto.Text)) return;
            if (this.dtpDatum.Value == null) return;
            if (string.IsNullOrEmpty(this.txtCas.Text)) return;

            DateTime? datum = null;
            DateTime tmpDate = Convert.ToDateTime(this.dtpDatum.Value);
            datum = new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day);
            datum = SetTimeToDateFromString(datum.Value, this.txtCas.Text);

            DateTime? datumSkryti = null;
            DateTime tmpDateSkryti = Convert.ToDateTime(this.dtpDatumSkryti.Value);
            datumSkryti = new DateTime(tmpDateSkryti.Year, tmpDateSkryti.Month, tmpDateSkryti.Day);
            datumSkryti = SetTimeToDateFromString(datumSkryti.Value, this.txtCasSkryti.Text);

            ZavozoveMisto zm = new ZavozoveMisto();
            zm.Mesto = this.txtMesto.Text;
            zm.DatumACas = datum.Value;
            zm.DatumACas_Skryti = datumSkryti;
            Storage<ZavozoveMisto>.Create(zm);
            this.zavozoveMistoList = null;

            this.dtpDatum.Value = null;
            this.dtpDatumSkryti.Value = null;
            this.txtMesto.Text = "";
            this.txtCas.Text = "";

            this.gridView.DataSource = this.ZavozoveMistoList;
            this.gridView.DataBind();
        }
    }
}
