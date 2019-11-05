using Eurona.Common.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin {
    public partial class ZavozoveMistoEdit : WebPage {
        private ZavozoveMisto zavozoveMisto;
        protected void Page_Load(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(this.Request["id"]))
                 zavozoveMisto = Storage<ZavozoveMisto>.ReadFirst(new ZavozoveMisto.ReadById { Id =Convert.ToInt32(this.Request["id"])});

            if (!IsPostBack) {
                this.txtMesto.Text = zavozoveMisto.Mesto;
                this.dtpDatum.Text = zavozoveMisto.DatumACas.Value.ToString();
                this.txtCas.Text = string.Format("{0:00}:{1:00}", zavozoveMisto.DatumACas.Value.Hour, zavozoveMisto.DatumACas.Value.Minute);
                this.dtpDatumSkryti.Text = zavozoveMisto.DatumACas_Skryti.HasValue ? zavozoveMisto.DatumACas_Skryti.Value.ToString() : "";
                this.txtCasSkryti.Text = zavozoveMisto.DatumACas_Skryti.HasValue ? string.Format("{0:00}:{1:00}", zavozoveMisto.DatumACas_Skryti.Value.Hour, zavozoveMisto.DatumACas_Skryti.Value.Minute) : "";
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

        protected void OnSave(object sender, EventArgs e) {
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
            if (string.IsNullOrEmpty(this.txtCasSkryti.Text)) {
                datumSkryti = null;
            }

            zavozoveMisto.Mesto = this.txtMesto.Text;
            zavozoveMisto.DatumACas = datum.Value;
            zavozoveMisto.DatumACas_Skryti = datumSkryti;
            Storage<ZavozoveMisto>.Update(zavozoveMisto);

            this.dtpDatum.Value = null;
            this.dtpDatumSkryti.Value = null;
            this.txtMesto.Text = "";
            this.txtCas.Text = "";

            Response.Redirect("~/eshop/admin/nastaveniZavozovehoMista.aspx");
        }
    }
}
