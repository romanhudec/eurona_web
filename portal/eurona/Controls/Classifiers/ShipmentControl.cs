using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using CMS.Entities.Classifiers;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using VATEntity = SHP.Entities.Classifiers.VAT;
using CMS.Controls;
using SupportedLocaleEntity = CMS.Entities.Classifiers.SupportedLocale;
using System.Collections.Generic;
using SHP.Controls.Classifiers;

namespace Eurona.Controls.Classifiers {
    public class ShipmentControl : ClassifierControl<ShipmentEntity> {
        protected TextBox txtOrder = null;
        protected CheckBox cbHide = null;
        protected CheckBox cbPlatbaDobirkou = null;
        protected CheckBox cbPlatbaKartou = null;
        public ShipmentControl() {
        }

        #region Protected overrides
        protected override void CreateChildControls() {
            Control pollControl = CreateDetailControl();
            if (pollControl != null)
                this.Controls.Add(pollControl);

            //Binding
            if (!this.Id.HasValue) this.classifier = new ShipmentEntity();
            else this.classifier = Storage<ShipmentEntity>.Read(new ClassifierBase.ReadById { Id = this.Id.Value })[0];


            //Datasource for DropDownList
            if (!IsPostBack) {
                this.txtName.Text = this.classifier.Name;
                this.txtOrder.Text = this.classifier.Order.ToString();
                this.cbHide.Checked = this.classifier.Hide;
                this.cbPlatbaDobirkou.Checked = this.classifier.PlatbaDobirkou;
                this.cbPlatbaKartou.Checked = this.classifier.PlatbaKartou;
                this.DataBind();
            }
        }
        #endregion


        /// <summary>
        /// Vytvori Control
        /// </summary>
        protected override Control CreateDetailControl() {
            this.txtName = new TextBox();
            this.txtName.ID = "txtName";
            this.txtName.Width = Unit.Percentage(100);
            this.txtName.Enabled = false;

            this.txtOrder = new TextBox();
            this.txtOrder.ID = "txtOrder";
            this.txtOrder.Width = Unit.Pixel(100);

            this.cbHide = new CheckBox();
            this.cbHide.ID = "cbHide";
            this.cbHide.Width = Unit.Pixel(100);

            this.cbPlatbaDobirkou = new CheckBox();
            this.cbPlatbaDobirkou.ID = "cbPlatbaDobirkou";
            this.cbPlatbaDobirkou.Width = Unit.Pixel(100);

            this.cbPlatbaKartou = new CheckBox();
            this.cbPlatbaKartou.ID = "cbPlatbaKartou";
            this.cbPlatbaKartou.Width = Unit.Pixel(100);

            this.btnSave = new Button();
            this.btnSave.CausesValidation = true;
            this.btnSave.Text = SHP.Resources.Controls.SaveButton_Text;
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnCancel = new Button();
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Text = SHP.Resources.Controls.CancelButton_Text;
            this.btnCancel.Click += new EventHandler(OnCancel);

            Table table = new Table();
            table.Width = this.Width;
            table.Height = this.Height;

            //Name
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = "form_label_required";
            cell.Text = SHP.Resources.Controls.ClassifierControl_Name;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(this.txtName);
            cell.Controls.Add(CreateRequiredFieldValidatorControl(this.txtName.ID));
            row.Cells.Add(cell);
            table.Rows.Add(row);

            //Order
            row = new TableRow();
            cell = new TableCell();
            cell.CssClass = "form_label";
            cell.Text = "Pořadí";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(this.txtOrder);
            row.Cells.Add(cell);
            table.Rows.Add(row);

            //Hide 
            row = new TableRow();
            cell = new TableCell();
            cell.CssClass = "form_label";
            cell.Text = "Skrýt";
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(this.cbHide);
            row.Cells.Add(cell);
            table.Rows.Add(row);

            //Platba Dobirkou 
            row = new TableRow();
            cell = new TableCell();
            cell.CssClass = "form_label";
            cell.Text = "Platba dobírkou";
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(this.cbPlatbaDobirkou);
            row.Cells.Add(cell);
            table.Rows.Add(row);

            //Platba Kartou 
            row = new TableRow();
            cell = new TableCell();
            cell.CssClass = "form_label";
            cell.Text = "Platba kartou";
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(this.cbPlatbaKartou);
            row.Cells.Add(cell);
            table.Rows.Add(row);

            //Save Cancel Buttons
            row = new TableRow();
            cell = new TableCell();
            cell.ColumnSpan = 2;
            cell.Controls.Add(this.btnSave);
            cell.Controls.Add(this.btnCancel);
            row.Cells.Add(cell);
            table.Rows.Add(row);

            return table;
        }

        public override void Save() {
            int order = 0;
            Int32.TryParse(this.txtOrder.Text, out order);
            this.classifier.Order = order;
            this.classifier.Hide = cbHide.Checked;
            this.classifier.PlatbaDobirkou = cbPlatbaDobirkou.Checked;
            this.classifier.PlatbaKartou = cbPlatbaKartou.Checked;
            Storage<ShipmentEntity>.Update(this.classifier);
        }
    }
}
