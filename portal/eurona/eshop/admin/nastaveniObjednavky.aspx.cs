using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderSettingsEntity = Eurona.Common.DAL.Entities.OrderSettings;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;

namespace Eurona.EShop.Admin {
    public partial class NastaveniObjednavky : WebPage {
        private const string DEFAULT_SHIPMENT_NONE = "DEFAULT_SHIPMENT_NONE";
        private OrderSettingsEntity settingsPostageSK = null;
        private OrderSettingsEntity settingsPostageCS = null;
        private OrderSettingsEntity settingsPostagePL = null;
        private List<ShipmentEntity> shipments = null;

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

            //NastaveniObjednavky Prepravce
            this.shipments = Storage<ShipmentEntity>.Read();
            ShipmentEntity noDefaultShipment = new ShipmentEntity();
            noDefaultShipment.Code = DEFAULT_SHIPMENT_NONE;
            noDefaultShipment.Id = 0;
            noDefaultShipment.Name = "--- ŽÁDNÝ ---";
            this.shipments.Add(noDefaultShipment);
            this.ddlShipment.DataTextField = "Name";
            this.ddlShipment.DataValueField = "Code";
            this.ddlShipment.DataSource = this.shipments;
            if (!IsPostBack) {
                ShipmentEntity shipment = Storage<ShipmentEntity>.ReadFirst(new ShipmentEntity.ReadDefault());
                if (shipment != null) {
                    this.ddlShipment.SelectedValue = shipment.Code;
                } else {
                    this.ddlShipment.SelectedValue = DEFAULT_SHIPMENT_NONE;
                }
                this.ddlShipment.DataBind();
            }
        }

        protected void OnPovolenaChecked(object sender, EventArgs e) {
            this.txtValueCS.Enabled = this.cbPovelena.Checked;
            this.txtValueSK.Enabled = this.cbPovelena.Checked;
            this.txtValuePL.Enabled = this.cbPovelena.Checked;
            settingsPostageSK.Enabled = settingsPostageCS.Enabled = settingsPostagePL.Enabled = this.cbPovelena.Checked;
        }

        protected void OnSavePostovne(object sender, EventArgs e) {
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
        protected void OnCancelPostovne(object sender, EventArgs e) {
            Response.Redirect(Page.ResolveUrl("~/eshop/admin/default.aspx"));
        }
       
        protected void OnSaveDopravce(object sender, EventArgs e) {
            UpdateDefaultShipment(this.ddlShipment.SelectedValue);
            Response.Redirect(Page.ResolveUrl("~/eshop/admin/default.aspx"));
        }
        protected void OnCancelDopravce(object sender, EventArgs e) {
            Response.Redirect(Page.ResolveUrl("~/eshop/admin/default.aspx"));
        }

        private void UpdateDefaultShipment(string shipmentCode) {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage storage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = storage.Connect()) {
                DateTime now = DateTime.Now;
                string sql = @"UPDATE cShpShipment SET [Default]=NULL WHERE InstanceId=@InstanceId";
                storage.Exec(connection, sql, new SqlParameter("@InstanceId", 1));
                if (shipmentCode != DEFAULT_SHIPMENT_NONE) {
                    sql = @"UPDATE cShpShipment SET [Default]=1 WHERE Code=@Code AND InstanceId=@InstanceId";
                    storage.Exec(connection, sql, new SqlParameter("@InstanceId", 1), new SqlParameter("@Code", shipmentCode));
                }
            }
        }
    }
}