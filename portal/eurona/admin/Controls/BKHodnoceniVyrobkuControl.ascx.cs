﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;
using Eurona.Common.DAL.Entities;

namespace Eurona.admin.Controls {
    public partial class BKHodnoceniVyrobkuControl : System.Web.UI.UserControl {
        private DAL.Entities.Classifiers.BonusovyKreditTyp typ = DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktHodnoceni;
        protected void Page_Load(object sender, EventArgs e) {
            this.gridView.DataSource = this.BonusovyKreditList;
            if (!Page.IsPostBack) {
                if (this.BonusovyKreditList.Count != 0) {
                    this.cbAktivni.Checked = this.BonusovyKreditList[0].Aktivni;
                    this.txtValueOd.Enabled = cbAktivni.Checked;
                    this.txtValueDo.Enabled = cbAktivni.Checked;
                    this.txtKredit.Enabled = cbAktivni.Checked;
                    this.btnSave.Enabled = cbAktivni.Checked;
                }
                this.gridView.DataBind();
            }
        }

        private List<BonusovyKredit> bonusovyKreditList = null;
        public List<BonusovyKredit> BonusovyKreditList {
            get {
                if (this.bonusovyKreditList != null) return this.bonusovyKreditList;
                this.bonusovyKreditList = Storage<BonusovyKredit>.Read(new BonusovyKredit.ReadByTyp { Typ = (int)this.typ });
                return this.bonusovyKreditList;
            }
        }
        protected void OnRowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName.ToUpper() == "DELETE_ITEM") {
                int id = Convert.ToInt32(e.CommandArgument);
                BonusovyKredit bk = Storage<BonusovyKredit>.ReadFirst(new BonusovyKredit.ReadById { Id = id });
                if (bk == null) return;

                Storage<BonusovyKredit>.Delete(bk);
            }

            this.bonusovyKreditList = null;
            this.gridView.DataSource = this.BonusovyKreditList;
            this.gridView.DataBind();
        }

        protected void OnAdd(object sender, EventArgs e) {
            decimal valueOd = 0;
            if (!Decimal.TryParse(this.txtValueOd.Text, out valueOd))
                return;

            decimal valueDo = 0;
            if (!Decimal.TryParse(this.txtValueDo.Text, out valueDo))
                return;

            decimal kredit = 0;
            if (!Decimal.TryParse(this.txtKredit.Text, out kredit))
                return;

            BonusovyKredit bk = new BonusovyKredit();
            bk.Typ = (int)this.typ;
            bk.HodnotaOd = valueOd;
            bk.HodnotaDo = valueDo;
            bk.Kredit = kredit;
            bk.Aktivni = this.cbAktivni.Checked;
            Storage<BonusovyKredit>.Create(bk);
            this.bonusovyKreditList = null;

            this.txtValueOd.Text = "";
            this.txtValueDo.Text = "";
            this.txtKredit.Text = "";

            this.gridView.DataSource = this.BonusovyKreditList;
            this.gridView.DataBind();
        }

        protected void cbAktivni_CheckedChanged(object sender, EventArgs e) {
            this.txtValueOd.Enabled = cbAktivni.Checked;
            this.txtValueDo.Enabled = cbAktivni.Checked;
            this.txtKredit.Enabled = cbAktivni.Checked;
            this.btnSave.Enabled = cbAktivni.Checked;
            this.bonusovyKreditList = Storage<BonusovyKredit>.Read(new BonusovyKredit.ReadByTyp { Typ = (int)this.typ });
            foreach (BonusovyKredit bk in bonusovyKreditList) {
                bk.Aktivni = cbAktivni.Checked;
                Storage<BonusovyKredit>.Update(bk);
            }
        }
    }
}