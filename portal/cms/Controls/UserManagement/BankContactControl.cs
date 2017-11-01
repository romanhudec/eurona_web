using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;

namespace CMS.Controls.UserManagement {
    public class BankContactControl : CmsControl {
        #region Private mebers
        private TextBox txtBankName = null;
        private TextBox txtBankCode = null;
        private TextBox txtAccountNumber = null;
        private TextBox txtIBAN = null;
        private TextBox txtSWIFT = null;

        private Button btnSave = null;
        private Button btnCancel = null;

        private BankContact bankContact = null;
        private bool isNew = false;
        #endregion

        public BankContactControl() {
        }

        public ControlSettings Settings {
            get {
                object o = ViewState["Settings"];
                return o != null ? (ControlSettings)o : null;
            }
            set { ViewState["Settings"] = value; }
        }

        public int? BankContactId {
            get {
                object o = ViewState["BankContactId"];
                return o != null ? (int?)Convert.ToInt32(o) : null;
            }
            set { ViewState["BankContactId"] = value; }
        }

        #region Protected overrides
        protected override void CreateChildControls() {
            //View SWIFT je povoleny iba v editacnom rezime
            this.EnableViewState = this.IsEditing;

            //Vytvorenie settings ak neexistuje
            if (this.Settings == null)
                this.Settings = new ControlSettings();

            base.CreateChildControls();

            Control detailControl = CreateDetailControl();
            if (detailControl != null)
                this.Controls.Add(detailControl);

            //Binding
            this.isNew = this.BankContactId == null;
            if (!this.BankContactId.HasValue) this.bankContact = new BankContact();
            else this.bankContact = Storage<BankContact>.ReadFirst(new BankContact.ReadById { BankContactId = this.BankContactId.Value });

            if (!IsPostBack) {
                UpdateUIFromEntity(this.bankContact);
                this.DataBind();
            }

        }
        #endregion

        /// <summary>
        /// Vytvori Control Adresy
        /// </summary>
        private Control CreateDetailControl() {
            this.txtBankName = new TextBox();
            this.txtBankName.ID = "txtBankName";
            this.txtBankName.Width = Unit.Percentage(100);

            this.txtBankCode = new TextBox();
            this.txtBankCode.ID = "txtBankCode";
            this.txtBankCode.Width = Unit.Percentage(100);

            this.txtAccountNumber = new TextBox();
            this.txtAccountNumber.ID = "txtAccountNumber";
            this.txtAccountNumber.Width = Unit.Percentage(100);

            this.txtIBAN = new TextBox();
            this.txtIBAN.ID = "txtIBAN";
            this.txtIBAN.Width = Unit.Percentage(100);

            this.txtSWIFT = new TextBox();
            this.txtSWIFT.ID = "txtSWIFT";
            this.txtSWIFT.Width = Unit.Percentage(100);

            this.btnSave = new Button();
            this.btnSave.CausesValidation = true;
            this.btnSave.Text = Resources.Controls.SaveButton_Text;
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnCancel = new Button();
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Text = Resources.Controls.CancelButton_Text;
            this.btnCancel.Click += new EventHandler(OnCancel);

            Table table = new Table();
            table.Width = this.Width;
            table.Height = this.Height;

            TableRow row = null;
            //BankName
            if (this.Settings.Visibility.BankName) {
                row = new TableRow();
                AddControlToRow(row, Resources.Controls.BankContactControl_BankName, this.txtBankName, 3, this.Settings.Require.BankName);
                table.Rows.Add(row);
            }

            //BankCode
            if (this.Settings.Visibility.BankCode) {
                row = new TableRow();
                AddControlToRow(row, Resources.Controls.BankContactControl_BankCode, this.txtBankCode, 3, this.Settings.Require.BankCode);
                table.Rows.Add(row);
            }

            //AccountNumber
            if (this.Settings.Visibility.AccountNumber) {
                row = new TableRow();
                AddControlToRow(row, Resources.Controls.BankContactControl_AccountNumber, this.txtAccountNumber, 3, this.Settings.Require.AccountNumber);
                table.Rows.Add(row);
            }

            //IBAN
            if (this.Settings.Visibility.IBAN) {
                row = new TableRow();
                AddControlToRow(row, Resources.Controls.BankContactControl_IBAN, this.txtIBAN, 3, this.Settings.Require.IBAN);
                table.Rows.Add(row);
            }

            //SWIFT
            if (this.Settings.Visibility.SWIFT) {
                row = new TableRow();
                AddControlToRow(row, Resources.Controls.BankContactControl_SWIFT, this.txtSWIFT, 3, this.Settings.Require.SWIFT);
                table.Rows.Add(row);
            }

            return table;
        }

        private void AddControlToRow(TableRow row, string labelText, Control control, int controlColspan, bool required) {
            (control as WebControl).Enabled = this.IsEditing;

            TableCell cell = new TableCell();
            cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
            cell.Text = labelText;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(control);
            cell.ColumnSpan = controlColspan;
            if (required && this.IsEditing) cell.Controls.Add(base.CreateRequiredFieldValidatorControl(control.ID));
            row.Cells.Add(cell);
        }

        #region Public methods

        public BankContact UpdateEntityFromUI(BankContact bankContact) {
            if (this.txtBankName == null) {
                return bankContact;
            }
            bankContact.BankName = this.txtBankName.Text;
            bankContact.BankCode = this.txtBankCode.Text;
            bankContact.AccountNumber = this.txtAccountNumber.Text;
            bankContact.IBAN = this.txtIBAN.Text;
            bankContact.SWIFT = this.txtSWIFT.Text;

            return bankContact;
        }

        public void UpdateUIFromEntity(BankContact bankContact) {
            this.txtBankName.Text = bankContact.BankName;
            this.txtBankCode.Text = bankContact.BankCode;
            this.txtAccountNumber.Text = bankContact.AccountNumber;
            this.txtIBAN.Text = bankContact.IBAN;
            this.txtSWIFT.Text = bankContact.SWIFT;
        }

        public BankContact UpdateBankContact(BankContact bankContact) {
            bankContact = UpdateEntityFromUI(bankContact);
            Storage<BankContact>.Update(bankContact);
            return bankContact;
        }

        public BankContact CreateBankContact(BankContact bankContact) {
            bankContact = UpdateEntityFromUI(bankContact);
            Storage<BankContact>.Create(bankContact);
            return bankContact;
        }
        #endregion

        #region Evant handlers
        void OnSave(object sender, EventArgs e) {
            if (this.isNew) CreateBankContact(this.bankContact);
            else UpdateBankContact(this.bankContact);

            Response.Redirect(this.ReturnUrl);
        }

        void OnCancel(object sender, EventArgs e) {
            Response.Redirect(this.ReturnUrl);
        }
        #endregion

        #region Settings classes
        [Serializable()]
        public class ControlSettings {
            public ControlSettings() {
                this.Visibility = new Visibility();
                this.Require = new Require();
            }
            public Visibility Visibility { get; set; }
            public Require Require { get; set; }
        }
        [Serializable()]
        public class Visibility {
            public Visibility() {
                this.BankName = true;
                this.BankCode = true;
                this.AccountNumber = true;
                this.IBAN = true;
                this.SWIFT = true;
            }

            public bool BankName { get; set; }
            public bool BankCode { get; set; }
            public bool AccountNumber { get; set; }
            public bool IBAN { get; set; }
            public bool SWIFT { get; set; }
        }

        [Serializable()]
        public class Require {
            public Require() {
                this.BankName = false;
                this.BankCode = false;
                this.AccountNumber = false;
                this.IBAN = false;
                this.SWIFT = false;
            }

            public bool BankName { get; set; }
            public bool BankCode { get; set; }
            public bool AccountNumber { get; set; }
            public bool IBAN { get; set; }
            public bool SWIFT { get; set; }
        }

        #endregion
    }
}
