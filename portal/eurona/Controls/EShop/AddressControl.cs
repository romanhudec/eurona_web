using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShpAddress = SHP.Entities.Address;
using CMS.Controls;
using System.Web.UI.HtmlControls;

namespace Eurona.Controls {
    public class AddressControl : CmsControl {
        #region Private mebers
        private TextBox txtFirstName = null;
        private TextBox txtLastName = null;
        private TextBox txtOrganization = null;
        private TextBox lblPhonePrefix = null;
        private TextBox txtPhone = null;
        private TextBox txtEmail = null;
        private TextBox txtId1 = null;
        private TextBox txtId2 = null;
        private TextBox txtId3 = null;

        private TextBox txtCity = null;
        private TextBox txtState = null;
        private TextBox txtZip = null;
        private TextBox txtStreet = null;
        private TextBox txtNotes = null;

        private Button btnSave = null;
        private Button btnCancel = null;

        private ShpAddress address = null;
        private bool isNew = false;

        private bool fNameEnabled = true;
        private bool lNameEnabled = true;
        private bool stateEnabled = true;

        private BaseValidator baseValidatorZip;
        #endregion

        public AddressControl() {
        }

        public ControlSettings Settings {
            get {
                object o = ViewState["Settings"];
                return o != null ? (ControlSettings)o : null;
            }
            set { ViewState["Settings"] = value; }
        }

        public int? AddressId {
            get {
                object o = ViewState["AddressId"];
                return o != null ? (int?)Convert.ToInt32(o) : null;
            }
            set { ViewState["AddressId"] = value; }
        }

        public string GetCityClientID() {
            return this.txtCity.ClientID;
        }
        public string GetZipClientID() {
            return this.txtZip.ClientID;
        }
        #region Protected overrides
        protected override void CreateChildControls() {
            //View State je povoleny iba v editacnom rezime
            this.EnableViewState = this.IsEditing;

            //Vytvorenie settings ak neexistuje
            if (this.Settings == null)
                this.Settings = new ControlSettings();
            //if (this.Settings.Validation.Email == null) {
            //    this.Settings.Validation.Email = new RegExpValidator();
            //    this.Settings.Validation.Email.CssClass = "ms-formvalidation";
            //    this.Settings.Validation.Email.EnableClientScript = true;
            //    this.Settings.Validation.Email.ErrorMessage = "!";
            //    this.Settings.Validation.Email.SetFocusOnError = true;
            //    this.Settings.Validation.Email.ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            //}

            if (this.Settings.Validation.State == null) {
                this.Settings.Validation.State = new RegExpValidator();
                this.Settings.Validation.State.CssClass = "ms-formvalidation";
                this.Settings.Validation.State.EnableClientScript = true;
                this.Settings.Validation.State.ErrorMessage = "CZ|SK|PL";
                this.Settings.Validation.State.SetFocusOnError = true;
                this.Settings.Validation.State.ValidationExpression = @"^(?:SK|CZ|PL)+$";
            }

            if (this.Settings.Validation.Phone == null) {
                this.Settings.Validation.Phone = new RegExpValidator();
                this.Settings.Validation.Phone.CssClass = "ms-formvalidation";
                this.Settings.Validation.Phone.EnableClientScript = true;
                this.Settings.Validation.Phone.ErrorMessage = Resources.EShopStrings.Anonymous_Register_IvalidMobile;
                this.Settings.Validation.Phone.SetFocusOnError = true;
                this.Settings.Validation.Phone.ValidationExpression = @"^([0-9]{9,9})$";
            }


            base.CreateChildControls();

            Control detailControl = CreateDetailControl();
            if (detailControl != null)
                this.Controls.Add(detailControl);

            //Binding
            this.isNew = this.AddressId == null;
            if (!this.AddressId.HasValue) this.address = new ShpAddress();
            else this.address = Storage<ShpAddress>.ReadFirst(new ShpAddress.ReadById { AddressId = this.AddressId.Value });

            if (!IsPostBack) {
                UpdateUIFromEntity(this.address);
                this.DataBind();
            }

            this.txtState.Attributes.Add("onchange", "stateChanged(this)");
            string jsStateChanged = @"function stateChanged(txtState)
            {
                var lblPhonePrefix = document.getElementById( '"+this.lblPhonePrefix.ClientID+ @"' );
                if( txtState.value == 'CZ' ) lblPhonePrefix.innerText = '+420';
                if( txtState.value == 'SK' ) lblPhonePrefix.innerText = '+421';
                if( txtState.value == 'PL' ) lblPhonePrefix.innerText = '+48';
            }";

            string js = @"$(function () {
				$('#" + this.txtCity.ClientID + @"').autocomplete({
					source: function (request, response) {
						$.ajax({
							url: """ + Page.ResolveUrl("~/getCityByState.ashx") + @"?mesto="" + request.term + ""&stat=""+document.getElementById('" + this.txtState.ClientID + @"').value,
							data: request.term,
							dataType: ""json"",
							type: ""POST"",
							contentType: ""application/json; charset=utf-8"",
							dataFilter: function (data) { return data; },
							success: function (data) {
                                
                                if( data.d.length == 0 ){
                                    document.getElementById('" + this.txtCity.ClientID + @"').style.backgroundColor='#EFB6E6';
                                    document.getElementById('" + this.txtZip.ClientID + @"').style.backgroundColor='#EFB6E6';
                                }else{
                                    document.getElementById('" + this.txtCity.ClientID + @"').style.backgroundColor='#FFFFFF';
                                    document.getElementById('" + this.txtZip.ClientID + @"').style.backgroundColor='#FFFFFF';
                                }
								response($.map(data.d, function (item) {
									return { label:item.Name + ', ' + item.Psc, value:item.Name, psc:item.Psc }
								}
                            ))
							},

							error: function (XMLHttpRequest, textStatus, errorThrown) {
								alert(textStatus);
							},
						});
					},
                    html: true, // optional (jquery.ui.autocomplete.html.js required)
                    // optional (if other layers overlap autocomplete list)
                    open: function(event, ui) {
                        $('.ui-autocomplete').css('z-index', 1000);
                    },
					select: function (event, ui) {
                        document.getElementById('" + this.txtZip.ClientID + @"').style.backgroundColor='#F0F0F0';
                        document.getElementById('" + this.txtZip.ClientID + @"').disabled=false;
						document.getElementById('" + this.txtCity.ClientID + @"').value = ui.item.value.trim();
						document.getElementById('" + this.txtZip.ClientID + @"').value = ui.item.psc.trim();
                        document.getElementById('" + this.txtZip.ClientID + @"').readOnly = true;
					},
					minLength: 2
				});
			});";

            string function_script = @"function updateASPxValidators()
                {
                   var isAllValid = true;
                   for (var i = 0; i < Page_Validators.length; i++)
                   {
                      var val = Page_Validators[i];
                      var ctrl = document.getElementById(val.controltovalidate);
                      if (ctrl != null && ctrl.style != null)
                      {
                         if (!val.isvalid){
                            ctrl.style.background = '#EFB6E6';
                            isAllValid = false;
                            return;
                         }
                         else
                            ctrl.style.backgroundColor = '';
                      }
                   }
                }";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update_function_validator", function_script, true);

            ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "js_autocomplete_incude_1", "https://code.jquery.com/jquery-1.12.4.js");
            ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "js_autocomplete_incude_2", "https://code.jquery.com/ui/1.12.1/jquery-ui.min.js");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js_autocomplete_city_" + this.ClientID, js, true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js_statechanged_" + this.lblPhonePrefix.ClientID, jsStateChanged, true);
        }

        #endregion

        /// <summary>
        /// Vytvori Control Adresy
        /// </summary>
        private Control CreateDetailControl() {
            this.txtFirstName = new TextBox();
            this.txtFirstName.ID = "txtFirstName";
            this.txtFirstName.Width = Unit.Percentage(100);
            this.txtFirstName.Enabled = fNameEnabled;

            this.txtLastName = new TextBox();
            this.txtLastName.ID = "txtLastName";
            this.txtLastName.Width = Unit.Percentage(100);
            this.txtFirstName.Enabled = lNameEnabled;

            this.txtOrganization = new TextBox();
            this.txtOrganization.ID = "txtOrganization";
            this.txtOrganization.Width = Unit.Percentage(100);
            this.txtOrganization.MaxLength = 50;

            this.txtId1 = new TextBox();
            this.txtId1.ID = "txtId1";
            this.txtId1.Width = Unit.Percentage(100);

            this.txtId2 = new TextBox();
            this.txtId2.ID = "txtId2";
            this.txtId2.Width = Unit.Percentage(100);

            this.txtId3 = new TextBox();
            this.txtId3.ID = "txtId3";
            this.txtId3.Width = Unit.Percentage(100);

            this.lblPhonePrefix = new TextBox();
            this.lblPhonePrefix.ReadOnly = true;
            this.lblPhonePrefix.Enabled = false;
            this.lblPhonePrefix.ID = "lblPhonePrefix";
            this.lblPhonePrefix.Width = Unit.Percentage(10);

            this.txtPhone = new TextBox();
            this.txtPhone.ID = "txtPhone";
            this.txtPhone.Width = Unit.Percentage(88);
            this.txtPhone.Attributes.Add("onblur", "return updateASPxValidators();");

            this.txtEmail = new TextBox();
            this.txtEmail.ID = "txtEmail";
            this.txtEmail.Width = Unit.Percentage(100);

            this.txtStreet = new TextBox();
            this.txtStreet.ID = "txtStreet";
            this.txtStreet.Width = Unit.Percentage(100);
            this.txtStreet.MaxLength = 100;

            this.txtZip = new TextBox();
            this.txtZip.ID = "txtZip";
            this.txtZip.Width = Unit.Pixel(50);

            this.txtCity = new TextBox();
            this.txtCity.ID = "txtCity";
            this.txtCity.Width = Unit.Percentage(100);
            this.txtCity.MaxLength = 50;

            this.txtState = new TextBox();
            this.txtState.ID = "txtState";
            this.txtState.Width = Unit.Percentage(100);

            this.txtNotes = new TextBox();
            this.txtNotes.ID = "txtNotes";
            this.txtNotes.Width = Unit.Percentage(100);

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
            //table.Attributes.Add( "border", "1" );

            //FirstName|Phone

            TableRow row = null;

            row = new TableRow();
            AddControlToRow(row, "<span></span>", new Label(), 0, false, null);
            BaseValidator baseValidator = null;
            if (Settings.Validation.Phone != null && this.IsEditing) {
                Settings.Validation.Phone.ControlToValidate = this.txtPhone.ID;
                baseValidator = base.CreateRegularExpressionValidatorControl(Settings.Validation.Phone);
                baseValidator.Display = ValidatorDisplay.Static;
            }
            if (baseValidator != null) {
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.Style.Add("width", "300px");
                div.Controls.Add(baseValidator);
                AddControlToRowWrap(row, "<span></span>", div, 0, false, null);
                table.Rows.Add(row);
            }

            row = new TableRow();
            Panel lcPhone = new Panel();
            AddControlToRow(row, "<span></span>", new Label(), 0, false, null);
            lcPhone.Controls.Add(new LiteralControl("<div><span class='address_notes_desription'>Telefonní číslo ve tvaru 123456789</span></div>"));
            AddControlToRow(row, "<span></span>", lcPhone, 0, false, null);
            table.Rows.Add(row);

            row = new TableRow();
            AddControlToRow(row, SHP.Resources.Controls.AddressControl_FirstName, this.txtFirstName, 0, true, null);
            AddPhoneToRow(row, SHP.Resources.Controls.AddressControl_Phone, this.lblPhonePrefix, this.txtPhone, 0, true, Settings.Validation.Phone);
            table.Rows.Add(row);

            //LastName|Email
            row = new TableRow();
            AddControlToRow(row, SHP.Resources.Controls.AddressControl_LastName, this.txtLastName, 0, true, null);
            AddControlToRow(row, SHP.Resources.Controls.AddressControl_Email, this.txtEmail, 0, true, Settings.Validation.Email);
            table.Rows.Add(row);

            //Organization|Id1
            row = new TableRow();
            AddControlToRow(row, SHP.Resources.Controls.AddressControl_Organization, this.txtOrganization, 0, false, null);
            AddControlToRow(row, SHP.Resources.Controls.AddressControl_Id1, this.txtId1, 0, false, null);
            table.Rows.Add(row);

            //Street|Id2
            row = new TableRow();
            AddControlToRow(row, SHP.Resources.Controls.AddressControl_Street, this.txtStreet, 0, true, null);
            AddControlToRow(row, SHP.Resources.Controls.AddressControl_Id2, this.txtId2, 0, false, null);
            table.Rows.Add(row);

            //City|Notes
            Panel lcNotes = new Panel();
            lcNotes.Controls.Add(this.txtNotes);
            lcNotes.Controls.Add(new LiteralControl("<div><span class='address_notes_desription'>" + Resources.EShopStrings.AddressControl_notest_description + "</span></div>"));
            row = new TableRow();
            baseValidatorZip = AddControlToRow(row, SHP.Resources.Controls.AddressControl_City, this.txtCity, 0, true, null);
            AddControlToRow(row, "<span class='address_notes_label'>" + SHP.Resources.Controls.AddressControl_Notes + "</span>", lcNotes, 0, false, null);
            table.Rows.Add(row);

            //Zip|Id3
            row = new TableRow();
            baseValidatorZip = AddZipControlToRow(row, SHP.Resources.Controls.AddressControl_Zip, this.txtZip, 0, true, Settings.Validation.Zip, Resources.EShopStrings.OrderControl_PSC_Hint);
            AddControlToRow(row, SHP.Resources.Controls.AddressControl_Id3, this.txtId3, 0, false, null);
            this.txtZip.Enabled = false;
            table.Rows.Add(row);

            //State
            row = new TableRow();
            AddControlToRow(row, SHP.Resources.Controls.AddressControl_State, this.txtState, 0, true, Settings.Validation.State);
            table.Rows.Add(row);

            return table;
        }

        void txtPhone_TextChanged(object sender, EventArgs e) {
            
        }

        private BaseValidator AddPhoneToRow(TableRow row, string labelText, TextBox phonePrefix, Control control, int controlColspan, bool required, RegExpValidator validator) {
            (control as WebControl).Enabled = this.IsEditing;

            TableCell cell = new TableCell();
            cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
            cell.Text = labelText;
            row.Cells.Add(cell);

            cell = new TableCell();
            //cell.CssClass = "form_control";
            cell.Style.Add("width", "50%");
            cell.Style.Add("padding-right", "5px");//margin: 2px;white-space: nowrap;text-align: left;
            cell.Style.Add("white-space", "nowrap");
            cell.Style.Add("text-align", "left");
            cell.Controls.Add(phonePrefix);
            cell.Controls.Add(new LiteralControl("&nbsp;"));
            cell.Controls.Add(control);
            cell.ColumnSpan = controlColspan;
            BaseValidator baseValidator = null;
            if (required && this.IsEditing) {
                baseValidator = base.CreateRequiredFieldValidatorControl(control.ID);
                cell.Controls.Add(baseValidator);
            }
            //if (validator != null && this.IsEditing) {
            //    validator.ControlToValidate = control.ID;
            //    baseValidator = base.CreateRegularExpressionValidatorControl(validator);
            //    baseValidator.Display = ValidatorDisplay.Dynamic;
            //    cell.Controls.Add(baseValidator);
            //}
            row.Cells.Add(cell);
            return baseValidator;
        }
        private BaseValidator AddControlToRowWrap(TableRow row, string labelText, Control control, int controlColspan, bool required, RegExpValidator validator) {
            if ((control is WebControl)) {
                (control as WebControl).Enabled = this.IsEditing;
            }

            TableCell cell = new TableCell();
            cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
            cell.Text = labelText;
            row.Cells.Add(cell);

            cell = new TableCell();
            //cell.CssClass = "form_control";
            cell.Style.Add("width", "50%");
            cell.Style.Add("padding-right", "5px");//margin: 2px;white-space: nowrap;text-align: left;
            cell.Style.Add("text-align", "left");
            cell.Controls.Add(control);
            cell.ColumnSpan = controlColspan;
            BaseValidator baseValidator = null;
            if (required && this.IsEditing) {
                baseValidator = base.CreateRequiredFieldValidatorControl(control.ID);
                cell.Controls.Add(baseValidator);
            }
            if (validator != null && this.IsEditing) {
                validator.ControlToValidate = control.ID;
                baseValidator = base.CreateRegularExpressionValidatorControl(validator);
                cell.Controls.Add(baseValidator);
            }
            row.Cells.Add(cell);
            return baseValidator;
        }

        private BaseValidator AddControlToRow(TableRow row, string labelText, Control control, int controlColspan, bool required, RegExpValidator validator) {
            if ((control is WebControl)) {
                (control as WebControl).Enabled = this.IsEditing;
            }

            TableCell cell = new TableCell();
            cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
            cell.Text = labelText;
            row.Cells.Add(cell);

            cell = new TableCell();
            //cell.CssClass = "form_control";
            cell.Style.Add("width", "50%");
            cell.Style.Add("padding-right", "5px");//margin: 2px;white-space: nowrap;text-align: left;
            cell.Style.Add("white-space", "nowrap");
            cell.Style.Add("text-align", "left");
            cell.Controls.Add(control);
            cell.ColumnSpan = controlColspan;
            BaseValidator baseValidator = null;
            if (required && this.IsEditing) {
                baseValidator = base.CreateRequiredFieldValidatorControl(control.ID);
                cell.Controls.Add(baseValidator);
            }
            if (validator != null && this.IsEditing) {
                validator.ControlToValidate = control.ID;
                baseValidator = base.CreateRegularExpressionValidatorControl(validator);
                cell.Controls.Add(baseValidator);
            }
            row.Cells.Add(cell);
            return baseValidator;
        }


        private BaseValidator AddZipControlToRow(TableRow row, string labelText, Control control, int controlColspan, bool required, RegExpValidator validator, string description) {
            (control as WebControl).Enabled = this.IsEditing;

            TableCell cell = new TableCell();
            cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
            cell.Text = labelText;
            row.Cells.Add(cell);
            row.VerticalAlign = VerticalAlign.Middle;
            row.Attributes.Add("valign", "middle");

            cell = new TableCell();
            //cell.CssClass = "form_control";
            cell.VerticalAlign = VerticalAlign.Middle;
            cell.Style.Add("width", "50%");
            cell.Style.Add("padding-right", "5px");//margin: 2px;white-space: nowrap;text-align: left;
            cell.Style.Add("white-space", "nowrap");
            cell.Style.Add("text-align", "left");
            cell.Controls.Add(control);
            cell.Controls.Add(new LiteralControl(string.Format("<div style='width:235px;margin-left:3px;display:inline-flex;vertical-align:middle;white-space:pre-line;' class='address_notes_desription'>{0}</div>", description)));
            cell.ColumnSpan = controlColspan;
            BaseValidator baseValidator = null;
            if (required && this.IsEditing) {
                baseValidator = base.CreateRequiredFieldValidatorControl(control.ID);
                cell.Controls.Add(baseValidator);
            }
            if (validator != null && this.IsEditing) {
                validator.ControlToValidate = control.ID;
                baseValidator = base.CreateRegularExpressionValidatorControl(validator);
                cell.Controls.Add(baseValidator);
            }
            row.Cells.Add(cell);
            return baseValidator;
        }
        

        private CustomValidator CreateCustomFieldValidatorControl(string controlToValidateId, string clientValidationFunction) {
            if (String.IsNullOrEmpty(clientValidationFunction)) return null;

            CustomValidator cv = new CustomValidator();
            cv.ID = string.Format("cv_{0}", controlToValidateId);
            cv.ControlToValidate = controlToValidateId;
            cv.Display = ValidatorDisplay.None;
            cv.ErrorMessage = "*";
            cv.ForeColor = System.Drawing.Color.Empty;
            cv.CssClass = "ms-formvalidation";
            cv.SetFocusOnError = true;
            cv.EnableClientScript = true;
            cv.ClientValidationFunction = clientValidationFunction;
            return cv;
        }

        #region Public methods

        public void EnableFirstName(bool enabled) {
            this.fNameEnabled = true;//enabled;
        }
        public void EnableLastName(bool enabled) {
            this.lNameEnabled = true;//enabled;
        }
        public void EnableState(bool enabled) {
            this.stateEnabled = enabled;
        }
        public ShpAddress UpdateEntityFromUI(ShpAddress address) {
            if (address == null) return address;
            address.FirstName = this.txtFirstName.Text;
            address.LastName = this.txtLastName.Text;
            address.Organization = this.txtOrganization.Text;
            address.Id1 = this.txtId1.Text;
            address.Id2 = this.txtId2.Text;
            address.Id3 = this.txtId3.Text;

            string prefix = GetPhonePrefixByState(this.txtState.Text);
            address.Phone = prefix + this.txtPhone.Text;
            address.Email = this.txtEmail.Text;

            address.City = this.txtCity.Text;
            address.Notes = this.txtNotes.Text;
            address.State = this.txtState.Text;
            address.Street = this.txtStreet.Text;
            address.Zip = this.txtZip.Text;

            return address;
        }

        public void UpdateUIFromEntity(ShpAddress address) {
            EnsureChildControls();

            this.txtFirstName.Text = address.FirstName;
            this.txtFirstName.Enabled = fNameEnabled;
            this.txtState.Enabled = stateEnabled;
            this.txtLastName.Text = address.LastName;
            this.txtLastName.Enabled = lNameEnabled;
            this.txtOrganization.Text = address.Organization;
            this.txtId1.Text = address.Id1;
            this.txtId2.Text = address.Id2;
            this.txtId3.Text = address.Id3;

            string prefix = GetPhonePrefixByState(address.State);
            this.lblPhonePrefix.Text = prefix;
            string phone = ExtractPhoneWithoutPrefix(address.Phone);

            this.txtPhone.Text = phone;
            this.txtEmail.Text = address.Email;

            this.txtCity.Text = address.City;
            this.txtNotes.Text = address.Notes;
            this.txtState.Text = address.State;
            this.txtStreet.Text = address.Street;
            this.txtZip.Text = address.Zip;
        }

        public ShpAddress UpdateAddress(ShpAddress address) {
            if (address == null) return null;
            address = UpdateEntityFromUI(address);
            Storage<ShpAddress>.Update(address);
            return address;
        }

        public ShpAddress CreateAddress(ShpAddress address) {
            address = UpdateEntityFromUI(address);
            Storage<ShpAddress>.Create(address);
            return address;
        }
        #endregion

        #region Evant handlers
        void OnSave(object sender, EventArgs e) {
            if (this.isNew) CreateAddress(this.address);
            else UpdateAddress(this.address);

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
                this.Validation = new Validation();
            }

            public Validation Validation { get; set; }
        }

        [Serializable()]
        public class Validation {
            public Validation() {
                this.Email = null;
                this.Phone = null;
                this.Zip = null;
                this.State = null;
            }

            public RegExpValidator Email { get; set; }
            public RegExpValidator Phone { get; set; }
            public RegExpValidator Zip { get; set; }
            public RegExpValidator State { get; set; }
        }

        #endregion
        private string GetPhonePrefixByState(string state) {
            string prefix = "+420";
            if (state.ToUpper() == "CZ") prefix = "+420";
            else if (state.ToUpper() == "SK") prefix = "+421";
            else if (state.ToUpper() == "PL") prefix = "+48";
            else prefix = "+420";
            return prefix;
        }

        private string ExtractPhoneWithoutPrefix(string fullPhone) {
            string phone = fullPhone;
            if (fullPhone.StartsWith("+420")) phone = fullPhone.Substring(4);
            if (fullPhone.StartsWith("+421")) phone = fullPhone.Substring(4);
            if (fullPhone.StartsWith("+48")) phone = fullPhone.Substring(3);
            return phone;
        }
    }


}
