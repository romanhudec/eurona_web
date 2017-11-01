using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona.admin.Controls {
    public partial class BKOdeslatPriteliControl : System.Web.UI.UserControl {
        private DAL.Entities.Classifiers.BonusovyKreditTyp typ = DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktEmailPoslatPriteli;
        protected void Page_Load(object sender, EventArgs e) {
            if (this.BonusovyKredit == null) {
                this.bonusovyKredit = new BonusovyKredit();
                this.bonusovyKredit.Typ = (int)this.typ;
                Storage<BonusovyKredit>.Create(this.bonusovyKredit);
            }

            if (!Page.IsPostBack) {
                this.cbAktivni.Checked = this.BonusovyKredit.Aktivni;
                this.txtKredit.Text = this.BonusovyKredit.Kredit.ToString();
                this.txtKredit.Enabled = cbAktivni.Checked;
                this.btnSave.Enabled = cbAktivni.Checked;
            }

        }

        private BonusovyKredit bonusovyKredit = null;
        public BonusovyKredit BonusovyKredit {
            get {
                if (this.bonusovyKredit != null) return this.bonusovyKredit;
                this.bonusovyKredit = Storage<BonusovyKredit>.ReadFirst(new BonusovyKredit.ReadByTyp { Typ = (int)this.typ });
                return this.bonusovyKredit;
            }
        }

        protected void OnSave(object sender, EventArgs e) {
            this.BonusovyKredit.Kredit = Convert.ToDecimal(this.txtKredit.Text);
            this.BonusovyKredit.Aktivni = this.cbAktivni.Checked;
            Storage<BonusovyKredit>.Update(this.BonusovyKredit);
            this.bonusovyKredit = null;
        }

        protected void cbAktivni_CheckedChanged(object sender, EventArgs e) {
            this.txtKredit.Enabled = cbAktivni.Checked;
            this.btnSave.Enabled = cbAktivni.Checked;
            OnSave(sender, e);
        }
    }
}