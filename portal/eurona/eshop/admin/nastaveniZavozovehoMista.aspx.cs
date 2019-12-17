using Eurona.Common.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.Admin {
    public partial class NastaveniZavozovehoMista : WebPage {
        private static string OSOBNI_ODBER_MESTO = "Osobní odběr v sídle společnosti";
        private static string OSOBNI_ODBER_PSC = "549 41";
        private static string OSOBNI_ODBER_ADRESA = "Lhota za Červeným Kostelcem 261, Červený Kostelec";

        private ZavozoveMistoLimit zavozoveMistoLimit = new ZavozoveMistoLimit("");
        protected void Page_Load(object sender, EventArgs e) {

            this.gridView.DataSource = this.ZavozoveMistoList;
            if (!Page.IsPostBack) {
                this.gridView.DataBind();
                if (this.ZavozoveMistoOsobniOdber != null) {
                    this.cbPovoliOsobniOdber.Checked = !this.ZavozoveMistoOsobniOdber.DatumACas_Skryti.HasValue;
                }
                this.panelOsobniOdber.Enabled = this.cbPovoliOsobniOdber.Checked;
            }
        }

        private List<ZavozoveMisto> zavozoveMistoList = null;
        public List<ZavozoveMisto> ZavozoveMistoList {
            get {
                if (this.zavozoveMistoList != null) return this.zavozoveMistoList;
                this.zavozoveMistoList = Storage<ZavozoveMisto>.Read(new ZavozoveMisto.ReadBy { OsobniOdberVSidleSpolecnosti = false });
                return this.zavozoveMistoList;
            }
        }

        private ZavozoveMisto zavozoveMistoOsobniOdber = null;
        public ZavozoveMisto ZavozoveMistoOsobniOdber {
            get {
                if (this.zavozoveMistoOsobniOdber != null) return this.zavozoveMistoOsobniOdber;
                this.zavozoveMistoOsobniOdber = Storage<ZavozoveMisto>.ReadFirst(new ZavozoveMisto.ReadBy { OsobniOdberVSidleSpolecnosti = true });
                return this.zavozoveMistoOsobniOdber;
            }
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            if (this.ZavozoveMistoOsobniOdber != null) {
                this.zavozoveMistoLimit = new ZavozoveMistoLimit(this.ZavozoveMistoOsobniOdber.OsobniOdberPovoleneCasy);
                this.gridViewLimit.DataSource = this.zavozoveMistoLimit.GetData();
                if (!IsPostBack) this.gridViewLimit.DataBind();
            }

            this.ddlLimitFrom.Items.Clear();
            this.ddlLimitTo.Items.Clear();
            List<String> daysInWeek = new List<string>();
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            for (int day = 0; day < 7; day++) {
                this.ddlLimitFrom.Items.Add(new ListItem { Value = day.ToString(), Text = ci.DateTimeFormat.DayNames[day] });
                this.ddlLimitTo.Items.Add(new ListItem { Value = day.ToString(), Text = ci.DateTimeFormat.DayNames[day] });
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
            if (string.IsNullOrEmpty(this.txtCasSkryti.Text)) {
                datumSkryti = null;
            }

            ZavozoveMisto zm = new ZavozoveMisto();
            zm.Stat = this.ddlStat.SelectedValue.ToString();
            zm.Mesto = this.txtMesto.Text;
            zm.Psc = this.txtPsc.Text;
            zm.Popis = this.txtPopis.Text;
            zm.DatumACas = datum.Value;
            zm.DatumACas_Skryti = datumSkryti;
            Storage<ZavozoveMisto>.Create(zm);
            this.zavozoveMistoList = null;

            this.dtpDatum.Value = null;
            this.dtpDatumSkryti.Text = "";
            this.dtpDatumSkryti.Value = null;
            this.txtCasSkryti.Text = "";
            this.txtCas.Text = "";
            this.txtMesto.Text = "";
            this.txtPsc.Text = "";
            this.txtPopis.Text = "";

            this.gridView.DataSource = this.ZavozoveMistoList;
            this.gridView.DataBind();
        }

        protected void cbPovoliOsobniOdber_CheckedChanged(object sender, EventArgs e) {
            this.panelOsobniOdber.Enabled = this.cbPovoliOsobniOdber.Checked;
            if (this.ZavozoveMistoOsobniOdber == null) {
                this.zavozoveMistoOsobniOdber = new ZavozoveMisto();
                this.zavozoveMistoOsobniOdber.OsobniOdberVSidleSpolecnosti = true;
                this.zavozoveMistoOsobniOdber.Mesto = OSOBNI_ODBER_MESTO;
                this.zavozoveMistoOsobniOdber.Psc = OSOBNI_ODBER_PSC;
                this.zavozoveMistoOsobniOdber.OsobniOdberAdresaSidlaSpolecnosti = OSOBNI_ODBER_ADRESA;
                this.zavozoveMistoOsobniOdber = Storage<ZavozoveMisto>.Create(this.zavozoveMistoOsobniOdber);
            }

            if (this.cbPovoliOsobniOdber.Checked) {
                this.zavozoveMistoOsobniOdber.DatumACas_Skryti = null;
            } else {
                this.zavozoveMistoOsobniOdber.DatumACas_Skryti = DateTime.Now.AddHours(-1);
            }
            this.zavozoveMistoOsobniOdber.Mesto = OSOBNI_ODBER_MESTO;
            this.zavozoveMistoOsobniOdber.Psc = OSOBNI_ODBER_PSC;
            this.zavozoveMistoOsobniOdber.OsobniOdberAdresaSidlaSpolecnosti = OSOBNI_ODBER_ADRESA;
            Storage<ZavozoveMisto>.Update(this.zavozoveMistoOsobniOdber);
        }

        protected void OnLimitRowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName.ToUpper() == "DELETE_ITEM") {
                string limitString = e.CommandArgument.ToString();

                this.zavozoveMistoLimit.Remove(limitString);
                this.zavozoveMistoOsobniOdber.OsobniOdberPovoleneCasy = this.zavozoveMistoLimit.ToString();
                Storage<ZavozoveMisto>.Update(this.zavozoveMistoOsobniOdber);

                this.gridViewLimit.DataSource = this.zavozoveMistoLimit.GetData();
                this.gridViewLimit.DataBind();
            }
        }
        protected void OnAddLimit(object sender, EventArgs e) {
            int dayFrom = Convert.ToInt32(this.ddlLimitFrom.SelectedValue);
            int dayTo = Convert.ToInt32(this.ddlLimitTo.SelectedValue);

            int hoursFrom = 0;
            int minutesFrom = 0;
            int hoursTo = 0;
            int minutesTo = 0;
            Int32.TryParse(this.txtLimitFromHodin.Text, out hoursFrom);
            Int32.TryParse(this.txtLimitFromMinut.Text, out minutesFrom);
            Int32.TryParse(this.txtLimitToHodin.Text, out hoursTo);
            Int32.TryParse(this.txtLimitToMinut.Text, out minutesTo);


            string limitString = ZavozoveMistoLimit.Limit.GetStringLimit(dayFrom, hoursFrom, minutesFrom, dayTo, hoursTo, minutesTo);
            this.zavozoveMistoLimit.Add(limitString);

            if (this.ZavozoveMistoOsobniOdber == null) {
                this.zavozoveMistoOsobniOdber = new ZavozoveMisto();
                this.zavozoveMistoOsobniOdber.OsobniOdberVSidleSpolecnosti = true;
                this.zavozoveMistoOsobniOdber.Mesto = OSOBNI_ODBER_MESTO;
                this.zavozoveMistoOsobniOdber.Psc = OSOBNI_ODBER_PSC;
                this.zavozoveMistoOsobniOdber.OsobniOdberAdresaSidlaSpolecnosti = OSOBNI_ODBER_ADRESA;
                this.zavozoveMistoOsobniOdber = Storage<ZavozoveMisto>.Create(this.zavozoveMistoOsobniOdber);
            }

            this.zavozoveMistoOsobniOdber.Mesto = OSOBNI_ODBER_MESTO;
            this.zavozoveMistoOsobniOdber.Psc = OSOBNI_ODBER_PSC;
            this.zavozoveMistoOsobniOdber.OsobniOdberAdresaSidlaSpolecnosti = OSOBNI_ODBER_ADRESA;
            this.zavozoveMistoOsobniOdber.OsobniOdberPovoleneCasy = this.zavozoveMistoLimit.ToString();
            Storage<ZavozoveMisto>.Update(this.zavozoveMistoOsobniOdber);

            this.gridViewLimit.DataSource = this.zavozoveMistoLimit.GetData();
            this.gridViewLimit.DataBind();
        }
    }
}
