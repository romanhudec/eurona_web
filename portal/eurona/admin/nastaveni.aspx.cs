using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SettingsEntity = Eurona.Common.DAL.Entities.Settings;
using Eurona.Common.DAL.Entities;
using Telerik.Web.UI;

namespace Eurona.Admin {
    public partial class EuronaNastaveniPage : WebPage {
        private SettingsEntity settingsVysypaniVsechKosiku = null;
        private SettingsEntity settingsEmailKomentar = null;
        private SettingsEntity settingsEmailPrispevek = null;
        private SettingsEntity settingsPlatbaKartou = null;
        private SettingsEntity settingsPlatbaKartouLimit = null;
        private SettingsEntity settingsAccountLinkCookiesLimit = null;
        private SettingsEntity settingsAccountLinkCookiesEnabled = null;
        private SettingsEntity settingsZdruzeneObjednavky = null;
        private SettingsEntity settingsPlatbaKartouZO = null;
        protected void Page_Load(object sender, EventArgs e) {
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            this.settingsEmailKomentar = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "EMAIL_KOMENTAR_PRODUKTU" });
            this.settingsEmailPrispevek = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "EMAIL_PRISPEVEK_DISKUSE" });
            this.settingsVysypaniVsechKosiku = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ESHOP_VYSYPANIVSECHKOSIKU" });
            this.settingsPlatbaKartou = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ESHOP_PLATBAKARTOU" });
            this.settingsZdruzeneObjednavky = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ESHOP_ZDRUZENE_OBJEDNAVKY" });
            this.settingsPlatbaKartouZO = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ESHOP_PLATBAKARTOU_ZDRUZENE_OBJEDNAVKY" });
            this.settingsPlatbaKartouLimit = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ESHOP_PLATBAKARTOU_LIMIT" });
            this.settingsAccountLinkCookiesLimit = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ACCOUNT_LINK_COOKIES_LIMIT" });
            this.settingsAccountLinkCookiesEnabled = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ACCOUNT_LINK_COOKIES_ENABLED" });

            if (this.settingsEmailKomentar != null)
                this.txtEmaliKomentar.Text = this.settingsEmailKomentar.Value;
            if (this.settingsEmailPrispevek != null)
                this.txtEmailPrispevek.Text = this.settingsEmailPrispevek.Value;

            if (this.settingsVysypaniVsechKosiku != null) {
                SettingsEntity.VysypaniVsechKosikuValue value = SettingsEntity.ParseVysypaniVsechKosikuStringValue(this.settingsVysypaniVsechKosiku);
                this.cbECartPovelena.Checked = value.Povelena;
                this.txtECartCas.Text = value.Cas;

                this.txtECartCas.Enabled = this.cbECartPovelena.Checked;
            }
            if (this.settingsPlatbaKartou != null) {
                bool value = Convert.ToBoolean(this.settingsPlatbaKartou.Value);
                this.cbEPlatbaKartouPovelena.Checked = value;
            }
            if (this.settingsPlatbaKartouLimit != null) {
                int limit = 15;
                Int32.TryParse(this.settingsPlatbaKartouLimit.Value, out limit);
                this.txtCardPaymentLimit.Text = limit.ToString();
            }
            if (this.settingsZdruzeneObjednavky != null) {
                bool value = Convert.ToBoolean(this.settingsZdruzeneObjednavky.Value);
                this.cbZdruzeneObjednavkyPovelena.Checked = value;
            }
            if (this.settingsPlatbaKartouZO != null) {
                bool value = Convert.ToBoolean(this.settingsPlatbaKartouZO.Value);
                this.cbEPlatbaKartouZOPovelena.Checked = value;
            }

            if (this.settingsAccountLinkCookiesEnabled != null) {
                bool enabled = true;
                Boolean.TryParse(this.settingsAccountLinkCookiesEnabled.Value, out enabled);
                this.cbAccountsLinkCookieEnabled.Checked = enabled;
                this.txtAccountLinkCookiesLimit.Enabled = enabled;
            }
            if (this.settingsAccountLinkCookiesLimit != null) {
                int limit = 30;
                Int32.TryParse(this.settingsAccountLinkCookiesLimit.Value, out limit);
                this.txtAccountLinkCookiesLimit.Text = limit.ToString();
            }

        }


        protected void OnECartPovolenaChecked(object sender, EventArgs e) {
            this.txtECartCas.Enabled = this.cbECartPovelena.Checked;
        }
        protected void cbAccountsLinkCookieEnabled_CheckedChanged(object sender, EventArgs e) {
            this.txtAccountLinkCookiesLimit.Enabled = this.cbAccountsLinkCookieEnabled.Checked;
        }

        protected void OnSave(object sender, EventArgs e) {
            if (this.settingsEmailKomentar != null) {
                this.settingsEmailKomentar.Value = this.txtEmaliKomentar.Text;
                Storage<SettingsEntity>.Update(this.settingsEmailKomentar);
            }
            if (this.settingsEmailPrispevek != null) {
                this.settingsEmailPrispevek.Value = this.txtEmailPrispevek.Text;
                Storage<SettingsEntity>.Update(this.settingsEmailPrispevek);
            }
            if (this.settingsVysypaniVsechKosiku != null) {
                this.settingsVysypaniVsechKosiku.Value = string.Format("{0};{1}", this.cbECartPovelena.Checked, this.txtECartCas.Text);
                Storage<SettingsEntity>.Update(this.settingsVysypaniVsechKosiku);
            }
            if (this.settingsPlatbaKartou != null) {
                this.settingsPlatbaKartou.Value = this.cbEPlatbaKartouPovelena.Checked.ToString();
                Storage<SettingsEntity>.Update(this.settingsPlatbaKartou);
            }
            if (this.settingsPlatbaKartouLimit != null) {
                this.settingsPlatbaKartouLimit.Value = this.txtCardPaymentLimit.Text;
                Storage<SettingsEntity>.Update(this.settingsPlatbaKartouLimit);
            }
            if (this.settingsZdruzeneObjednavky != null) {
                this.settingsZdruzeneObjednavky.Value = this.cbZdruzeneObjednavkyPovelena.Checked.ToString();
                Storage<SettingsEntity>.Update(this.settingsZdruzeneObjednavky);
            }
            if (this.settingsPlatbaKartouZO != null) {
                this.settingsPlatbaKartouZO.Value = this.cbEPlatbaKartouZOPovelena.Checked.ToString();
                Storage<SettingsEntity>.Update(this.settingsPlatbaKartouZO);
            }
            if (this.settingsAccountLinkCookiesLimit != null) {
                this.settingsAccountLinkCookiesLimit.Value = this.txtAccountLinkCookiesLimit.Text;
                Storage<SettingsEntity>.Update(this.settingsAccountLinkCookiesLimit);
            }
            if (this.settingsAccountLinkCookiesEnabled != null) {
                this.settingsAccountLinkCookiesEnabled.Value = this.cbAccountsLinkCookieEnabled.Checked.ToString();
                Storage<SettingsEntity>.Update(this.settingsAccountLinkCookiesEnabled);
            }
            Response.Redirect(Page.ResolveUrl("~/admin/default.aspx"));
        }

        protected void OnCancel(object sender, EventArgs e) {
            Response.Redirect(Page.ResolveUrl("~/admin/default.aspx"));
        }
    }
}