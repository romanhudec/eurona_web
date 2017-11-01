using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderSettingsEntity = Eurona.Common.DAL.Entities.OrderSettings;

namespace Eurona.EShop.Admin {
    public partial class NastaveniObjednavky : WebPage {
        private OrderSettingsEntity settingsPostageSK = null;
        private OrderSettingsEntity settingsPostageCS = null;
        private OrderSettingsEntity settingsPostagePL = null;

        protected void Page_Load(object sender, EventArgs e) {
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();
            settingsPostageSK = OrderSettingsEntity.GetFreePostageSumaSK();
            settingsPostageCS = OrderSettingsEntity.GetFreePostageSumaCS();
            settingsPostagePL = OrderSettingsEntity.GetFreePostageSumaPL();

            txtValueCS.Text = settingsPostageCS.Value.ToString("F0");
            txtValueSK.Text = settingsPostageSK.Value.ToString("F0");
            txtValuePL.Text = settingsPostagePL.Value.ToString("F0");

            this.cbPovelena.Checked = settingsPostageCS.Enabled;
            OnPovolenaChecked(this, null);
        }

        protected void OnPovolenaChecked(object sender, EventArgs e) {
            this.txtValueCS.Enabled = this.cbPovelena.Checked;
            this.txtValueSK.Enabled = this.cbPovelena.Checked;
            this.txtValuePL.Enabled = this.cbPovelena.Checked;
            settingsPostageSK.Enabled = settingsPostageCS.Enabled = settingsPostagePL.Enabled = this.cbPovelena.Checked;
        }

        protected void OnSave(object sender, EventArgs e) {
            double valueCS = 0;
            Double.TryParse(this.txtValueCS.Text, out valueCS);
            double valueSK = 0;
            Double.TryParse(this.txtValueSK.Text, out valueSK);
            double valuePL = 0;
            Double.TryParse(this.txtValuePL.Text, out valuePL);

            settingsPostageCS.Value = Convert.ToDecimal(valueCS);
            settingsPostageSK.Value = Convert.ToDecimal(valueSK);
            settingsPostagePL.Value = Convert.ToDecimal(valuePL);

            Storage<OrderSettingsEntity>.Update(settingsPostageSK);
            Storage<OrderSettingsEntity>.Update(settingsPostageCS);
            Storage<OrderSettingsEntity>.Update(settingsPostagePL);

            Response.Redirect(Page.ResolveUrl("~/eshop/admin/default.aspx"));
        }
        protected void OnCancel(object sender, EventArgs e) {
            Response.Redirect(Page.ResolveUrl("~/eshop/admin/default.aspx"));
        }
    }
}