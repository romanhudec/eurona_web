using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UzavierkaEntity = Eurona.Common.DAL.Entities.Uzavierka;

namespace Eurona.Controls {
    public partial class UzavierkaControl : Eurona.Common.Controls.UserControl {
        private UzavierkaEntity uzavierka = null;
        private UzavierkaEntity uzavierkaBefore = null;
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            this.uzavierkaBefore = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.EuronaBefor });
            if (this.uzavierkaBefore == null) {
                this.uzavierkaBefore = new UzavierkaEntity();
            }

            #region before uzavierka

            DateTime dateOd = uzavierkaBefore.UzavierkaOd.HasValue ? uzavierkaBefore.UzavierkaOd.Value : DateTime.MinValue;
            DateTime dateDo = uzavierkaBefore.UzavierkaDo.HasValue ? uzavierkaBefore.UzavierkaDo.Value : DateTime.MinValue;

            this.lblBeforeDatumOd.Text = dateOd.ToString();
            this.lblBeforeDatumDo.Text = dateDo.ToString();
            #endregion

            #region current uzavierka
            this.uzavierka = Storage<UzavierkaEntity>.ReadFirst(new UzavierkaEntity.ReadById { UzavierkaId = (int)UzavierkaEntity.UzavierkaId.Eurona });
            if (uzavierka == null) return;

            this.cbPovelena.Checked = uzavierka.Povolena;
            this.dtpDatumOd.Value = uzavierka.UzavierkaOd;
            this.dtpDatumDo.Value = uzavierka.UzavierkaDo;

            dateOd = uzavierka.UzavierkaOd.HasValue ? uzavierka.UzavierkaOd.Value : DateTime.MinValue;
            dateDo = uzavierka.UzavierkaDo.HasValue ? uzavierka.UzavierkaDo.Value : DateTime.MinValue;

            this.txtCasOd.Text = string.Format("{0:0#}:{1:0#}", dateOd.Hour, dateOd.Minute);
            this.txtCasDo.Text = string.Format("{0:0#}:{1:0#}", dateDo.Hour, dateDo.Minute);
            #endregion

            OnPovolenaChecked(this, null);
        }

        protected void OnPovolenaChecked(object sender, EventArgs e) {
            this.dtpDatumOd.Enabled = this.cbPovelena.Checked;
            this.dtpDatumDo.Enabled = this.cbPovelena.Checked;
            this.txtCasOd.Enabled = this.cbPovelena.Checked;
            this.txtCasDo.Enabled = this.cbPovelena.Checked;
        }

        private DateTime SetTimeToDateFromString(DateTime date, string timeText) {
            if (string.IsNullOrEmpty(this.txtCasOd.Text)) return date;

            string[] time = timeText.Split(':');
            if (time.Length < 2) return date;

            int hour = Convert.ToInt32(time[0]);
            int minute = Convert.ToInt32(time[1]);
            date = date.AddHours(hour);
            date = date.AddMinutes(minute);
            return date;
        }
        protected void OnSave(object sender, EventArgs e) {
            DateTime? datumOd = null;
            DateTime? datumDo = null;
            if (this.dtpDatumOd.Value != null) {
                DateTime tmpDate = Convert.ToDateTime(this.dtpDatumOd.Value);
                datumOd = new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day);
                datumOd = SetTimeToDateFromString(datumOd.Value, this.txtCasOd.Text);
            }
            if (this.dtpDatumDo.Value != null) {
                DateTime tmpDate = Convert.ToDateTime(this.dtpDatumDo.Value);
                datumDo = new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day);
                datumDo = SetTimeToDateFromString(datumDo.Value, this.txtCasDo.Text);
            }

            this.uzavierka.Povolena = this.cbPovelena.Checked;
            this.uzavierka.UzavierkaOd = datumOd;
            this.uzavierka.UzavierkaDo = datumDo;
            Storage<UzavierkaEntity>.Update(this.uzavierka);

            Response.Redirect(Page.ResolveUrl("~/eshop/admin/default.aspx"));
        }
        protected void OnCancel(object sender, EventArgs e) {
            Response.Redirect(Page.ResolveUrl("~/eshop/admin/default.aspx"));
        }

        protected void OnAddAndSave(object sender, EventArgs e) {

            DateTime? datumOd = null;
            DateTime? datumDo = null;
            if (this.dtpDatumOd.Value != null) {
                DateTime tmpDate = Convert.ToDateTime(this.dtpDatumOd.Value);
                datumOd = new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day);
                datumOd = SetTimeToDateFromString(datumOd.Value, this.txtCasOd.Text);
            }
            if (this.dtpDatumDo.Value != null) {
                DateTime tmpDate = Convert.ToDateTime(this.dtpDatumDo.Value);
                datumDo = new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day);
                datumDo = SetTimeToDateFromString(datumDo.Value, this.txtCasDo.Text);
            }

            this.uzavierkaBefore.Povolena = this.uzavierka.Povolena;
            this.uzavierkaBefore.UzavierkaOd = this.uzavierka.UzavierkaOd;
            this.uzavierkaBefore.UzavierkaDo = this.uzavierka.UzavierkaDo;
            if (this.uzavierkaBefore.Id == 0) {
                this.uzavierkaBefore.Id = (int)UzavierkaEntity.UzavierkaId.EuronaBefor;
                Storage<UzavierkaEntity>.Create(this.uzavierkaBefore);
            }else
                Storage<UzavierkaEntity>.Update(this.uzavierkaBefore);

            this.uzavierka.Povolena = this.cbPovelena.Checked;
            this.uzavierka.UzavierkaOd = datumOd;
            this.uzavierka.UzavierkaDo = datumDo;
            Storage<UzavierkaEntity>.Update(this.uzavierka);

            Response.Redirect(Page.ResolveUrl("~/eshop/admin/default.aspx"));
        }
    }
}