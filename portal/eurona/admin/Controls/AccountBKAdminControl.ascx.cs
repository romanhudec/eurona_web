using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;
using Eurona.Common.DAL.Entities;
using Eurona.Controls;

namespace Eurona.admin.Controls {
    public partial class AccountBKAdminControl : System.Web.UI.UserControl {
        public event EventHandler OnKreditChanged;

        private DAL.Entities.Classifiers.BonusovyKreditTyp typ = DAL.Entities.Classifiers.BonusovyKreditTyp.RucneZadany;
        private int accountId = 0;
        protected void Page_Load(object sender, EventArgs e) {
            string accountId = Request["id"];
            if (string.IsNullOrEmpty(accountId)) return;

            this.accountId = Convert.ToInt32(accountId);

            this.gridView.DataSource = this.BonusovyKreditUzivateleList;
            if (!Page.IsPostBack) {
                this.rbPristiMesic.Checked = true;
                this.gridView.DataBind();
            }
        }
        private BonusovyKredit bonusovyKredit = null;
        public BonusovyKredit BonusovyKredit {
            get {
                if (this.bonusovyKredit != null) return this.bonusovyKredit;
                this.bonusovyKredit = Storage<BonusovyKredit>.ReadFirst(new BonusovyKredit.ReadByTyp { Typ = (int)this.typ });
                if (this.bonusovyKredit == null) {
                    this.bonusovyKredit = new BonusovyKredit();
                    this.bonusovyKredit.Typ = (int)this.typ;
                    this.bonusovyKredit = Storage<BonusovyKredit>.Create(this.bonusovyKredit);
                }
                return this.bonusovyKredit;
            }
        }
        private List<BonusovyKreditUzivatele> bonusovyKreditUzivateleList = null;
        public List<BonusovyKreditUzivatele> BonusovyKreditUzivateleList {
            get {
                if (this.bonusovyKreditUzivateleList != null) return this.bonusovyKreditUzivateleList;
                this.bonusovyKreditUzivateleList = Storage<BonusovyKreditUzivatele>.Read(new BonusovyKreditUzivatele.ReadByBonusovyKreditAndAccount { BonusovyKreditId = this.BonusovyKredit.Id, AccountId = this.accountId });
                return this.bonusovyKreditUzivateleList;
            }
        }
        protected void OnRowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName.ToUpper() == "DELETE_ITEM") {
                int id = Convert.ToInt32(e.CommandArgument);
                BonusovyKreditUzivatele bk = Storage<BonusovyKreditUzivatele>.ReadFirst(new BonusovyKreditUzivatele.ReadById { Id = id });
                if (bk == null) return;

                Storage<BonusovyKreditUzivatele>.Delete(bk);
            }

            this.bonusovyKreditUzivateleList = null;
            this.gridView.DataSource = this.BonusovyKreditUzivateleList;
            this.gridView.DataBind();

            if (OnKreditChanged != null) OnKreditChanged(sender, e);
        }

        protected void OnAdd(object sender, EventArgs e) {
            decimal kredit = 0;
            if (!Decimal.TryParse(this.txtKredit.Text, out kredit))
                return;

            int addMonths = 0;
            if (this.rbPristiMesic.Checked) addMonths = 1;
            DateTime now = DateTime.Now.AddMonths(addMonths);
            DateTime datumOd = new DateTime(now.Year, now.Month, 1);
            DateTime datumDo = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));

            //Ak pouzivatel dosiahol maximalny mozny kredit, dalsi sa mu nezapisuje
            decimal maximalniPocetBKzaMesic = BonusovyKreditUzivateleHelper.GetMaximalniPocetBKZaMesic();
            Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = this.accountId });
            decimal kreditcelkemZaMesic = BonusovyKreditUzivateleHelper.GetPlatnyKreditCelkem(account, now.Year, now.Month);
            if (kreditcelkemZaMesic >= maximalniPocetBKzaMesic) {
                //Alert s informaciou o pridani do nakupneho kosika
                string js = string.Format("alert('Maximální počet bonusových kreditů za měsíc je {0}!');", maximalniPocetBKzaMesic);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "addProductToCart", js, true);
                return;
            }

            if (kreditcelkemZaMesic + kredit > maximalniPocetBKzaMesic)
                kredit = maximalniPocetBKzaMesic - kreditcelkemZaMesic;

            BonusovyKreditUzivatele bk = new BonusovyKreditUzivatele();
            bk.Datum = DateTime.Now;
            bk.BonusovyKreditId = this.BonusovyKredit.Id;
            bk.AccountId = this.accountId;
            bk.Poznamka = this.txtPoznamka.Text; ;
            bk.Hodnota = kredit;
            bk.PlatnostOd = datumOd;
            bk.PlatnostDo = datumDo;
            Storage<BonusovyKreditUzivatele>.Create(bk);
            this.bonusovyKreditUzivateleList = null;

            this.txtKredit.Text = "";
            this.txtPoznamka.Text = string.Empty;

            this.gridView.DataSource = this.BonusovyKreditUzivateleList;
            this.gridView.DataBind();

            if (OnKreditChanged != null) OnKreditChanged(sender, e);
        }
    }
}